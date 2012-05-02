/**
 * Copyright (c) 2012, Yubico AB.  All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 *
 * * Redistributions of source code must retain the above copyright
 *   notice, this list of conditions and the following disclaimer.
 *
 * * Redistributions in binary form must reproduce the above copyright
 *   notice, this list of conditions and the following
 *   disclaimer in the documentation and/or other materials provided
 *   with the distribution.
 *
 *  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND
 *  CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
 *  INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF
 *  MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 *  DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS
 *  BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
 *  EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
 *  TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 *  DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
 *  ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR
 *  TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF
 *  THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF
 *  SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace YubicoDotNetClient
{
    class YubicoValidate
    {
        public static YubicoResponse validate(List<String> urls, String userAgent)
        {
            List<Task<YubicoResponse>> tasks = new List<Task<YubicoResponse>>();
            CancellationTokenSource cancellation = new CancellationTokenSource();
            foreach (String url in urls)
            {
                
                Task<YubicoResponse> task = new Task<YubicoResponse>(() =>
                    {
                        return DoVerify(url, userAgent);
                    }, cancellation.Token);
                task.ContinueWith((t) => { }, TaskContinuationOptions.OnlyOnFaulted);
                tasks.Add(task);
                task.Start();
            }
            while (tasks.Count != 0)
            {
                // TODO: handle exceptions from the verify task. Better to be able to propagate cause for error.
                int completed = Task.WaitAny(tasks.ToArray());
                Task<YubicoResponse> task = tasks[completed];
                tasks.Remove(task);
                if (task.Result != null)
                {
                    cancellation.Cancel();
                    return task.Result;
                }
            }
            return null;
        }

        private static YubicoResponse DoVerify(String url, String userAgent)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            if (userAgent == null)
            {
                request.UserAgent = "YubicoDotNetClient version:" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            }
            else
            {
                request.UserAgent = userAgent;
            }
            request.Timeout = 15000;
            HttpWebResponse rawResponse;
            try
            {
                rawResponse = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException)
            {
                return null;
            }
            Stream dataStream = rawResponse.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            YubicoResponse response;
            try
            {
                response = new YubicoResponseImpl(reader.ReadToEnd());
            }
            catch (ArgumentException)
            {
                return null;
            }
            if (response.getStatus() == YubicoResponseStatus.REPLAYED_REQUEST)
            {
                //throw new YubicoValidationException("Replayed request, this otp & nonce combination has been seen before.");
                return null;
            }
            return response;
        }
    }
}
