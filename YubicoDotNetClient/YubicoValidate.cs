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
        public static YubicoResponse validate(String[] urls)
        {
            List<Task<YubicoResponse>> tasks = new List<Task<YubicoResponse>>();
            CancellationTokenSource cancellation = new CancellationTokenSource();
            foreach (String url in urls)
            {
                
                Task<YubicoResponse> task = new Task<YubicoResponse>(() =>
                    {
                        return DoVerify(url);
                    }, cancellation.Token);
                task.ContinueWith((t) => { }, TaskContinuationOptions.OnlyOnFaulted);
                tasks.Add(task);
                task.Start();
            }
            while (tasks.Count != 0)
            {
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

        private static YubicoResponse DoVerify(String url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = "YubicoDotNetClient version:" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            request.Timeout = 15000;
            HttpWebResponse rawResponse = (HttpWebResponse)request.GetResponse();
            Stream dataStream = rawResponse.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            YubicoResponse response = new YubicoResponseImpl(reader.ReadToEnd());
            if (response.getStatus() == YubicoResponseStatus.REPLAYED_REQUEST)
            {
                //throw new YubicoValidationException("Replayed request, this otp & nonce combination has been seen before.");
                return null;
            }
            return response;
        }
    }
}
