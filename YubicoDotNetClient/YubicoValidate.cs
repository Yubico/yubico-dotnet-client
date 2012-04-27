using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace YubicoDotNetClient
{
    class YubicoValidate
    {
        public static YubicoResponse validate(String[] urls)
        {
            Task<YubicoResponse>[] tasks = new Task<YubicoResponse>[urls.Length];
            for (int i = 0; i < urls.Length; i++)
            {
                tasks[i] = Task<YubicoResponse>.Factory.StartNew(() =>
                {
                    return DoVerify(urls[i]);
                });

            }
            int completed = Task.WaitAny(tasks);

            foreach (Task task in tasks)
            {
                task.ContinueWith(t => t.Exception, TaskContinuationOptions.OnlyOnFaulted);
            }

            return tasks[completed].Result;
        }

        private static YubicoResponse DoVerify(String url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse rawResponse = (HttpWebResponse)request.GetResponse();
            Stream dataStream = rawResponse.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            YubicoResponse response = new YubicoResponseImpl(reader.ReadToEnd());
            if (response.getStatus() == YubicoResponseStatus.REPLAYED_REQUEST)
            {
                throw new YubicoValidationException("Replayed request, this otp & nonce combination has been seen before.");
            }
            return response;
        }
    }
}
