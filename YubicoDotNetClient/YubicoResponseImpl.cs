using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace YubicoDotNetClient
{
    class YubicoResponseImpl : YubicoResponse
    {
        private String H;
        private String T;
        private YubicoResponseStatus status;
        private int timestamp;
        private int sessionCounter;
        private int useCounter;
        private String sync;
        private String otp;
        private String nonce;
        private SortedDictionary<String, String> responseMap;

        public YubicoResponseImpl(String response)
        {
            StringReader reader = new StringReader(response);
            String line;
            responseMap = new SortedDictionary<String, String>();
            while ((line = reader.ReadLine()) != null)
            {
                String[] parts = line.Split(new char[] { '=' }, 2);
                switch (parts[0])
                {
                    case "H":
                        H = parts[1];
                        break;
                    case "T":
                        T = parts[1];
                        break;
                    case "status":
                        status = (YubicoResponseStatus)Enum.Parse(typeof(YubicoResponseStatus), parts[1], true);
                        break;
                    case "timestamp":
                        timestamp = int.Parse(parts[1]);
                        break;
                    case "sessioncounter":
                        sessionCounter = int.Parse(parts[1]);
                        break;
                    case "sessionuse":
                        useCounter = int.Parse(parts[1]);
                        break;
                    case "sl":
                        sync = parts[1];
                        break;
                    case "otp":
                        otp = parts[1];
                        break;
                    case "nonce":
                        nonce = parts[1];
                        break;
                }
                responseMap.Add(parts[0], parts[1]);
            }
        }

        public String getH()
        {
            return H;
        }

        public String getT()
        {
            return T;
        }

        public YubicoResponseStatus getStatus()
        {
            return status;
        }

        public int getTimestamp()
        {
            return timestamp;
        }

        public int getSessionCounter()
        {
            return sessionCounter;
        }

        public int getUseCounter()
        {
            return useCounter;
        }

        public String getSync()
        {
            return sync;
        }

        public String getOtp()
        {
            return otp;
        }

        public String getNonce()
        {
            return nonce;
        }

        public SortedDictionary<String, String> getResponseMap()
        {
            return responseMap;
        }
    }
}
