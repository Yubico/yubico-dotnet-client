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
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using System.IO;

namespace YubicoDotNetClient
{
    public sealed class YubicoValidate
    {
        public static IYubicoResponse Validate(IEnumerable<string> urls, string userAgent)
        {
            var tasks = new List<Task<IYubicoResponse>>();
            var cancellation = new CancellationTokenSource();            
            
            foreach (var url in urls)
            {
                var thisUrl = url;
                var task = new Task<IYubicoResponse>(() => DoVerify(thisUrl, userAgent), cancellation.Token);
                task.ContinueWith(t => { }, TaskContinuationOptions.OnlyOnFaulted);
                tasks.Add(task);
                task.Start();
            }

            while (tasks.Count != 0)
            {
                // TODO: handle exceptions from the verify task. Better to be able to propagate cause for error.
                var completed = Task.WaitAny(tasks.Cast<Task>().ToArray());
                var task = tasks[completed];
                tasks.Remove(task);
                if (task.Result != null)
                {
                    cancellation.Cancel();
                    return task.Result;
                }
            }

            return null;
        }

        private static IYubicoResponse DoVerify(string url, string userAgent)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);            
            
            if (userAgent == null)
            {
                request.UserAgent = "YubicoDotNetClient version:" + Assembly.GetExecutingAssembly().GetName().Version;
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

            using (var dataStream = rawResponse.GetResponseStream())
            {
                if (dataStream != null)
                {
                    using (var reader = new StreamReader(dataStream))
                    {
                        IYubicoResponse response;
                        
                        try
                        {
                            response = new YubicoResponse(reader.ReadToEnd());
                        }
                        catch (ArgumentException)
                        {
                            return null;
                        }

                        if (response.Status == YubicoResponseStatus.ReplayedRequest)
                        {
                            //throw new YubicoValidationException("Replayed request, this otp & nonce combination has been seen before.");
                            return null;
                        }

                        return response;
                    }
                }
            }

            throw new YubicoValidationException();
        }
    }
}
