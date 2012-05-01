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
using System.IO;

namespace YubicoDotNetClient
{
    class YubicoResponseImpl : YubicoResponse
    {
        private String h;
        private String t;
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
                bool unhandled = false;
                String[] parts = line.Split(new char[] { '=' }, 2);
                switch (parts[0])
                {
                    case "h":
                        h = parts[1];
                        break;
                    case "t":
                        t = parts[1];
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
                    default:
                        unhandled = true;
                        break;
                }
                if (!unhandled)
                {
                    responseMap.Add(parts[0], parts[1]);
                }
            }
            if (status == YubicoResponseStatus.EMPTY)
            {
                throw new ArgumentException("Response doesn't look like a validation response.");
            }
        }

        public String getH()
        {
            return h;
        }

        public String getT()
        {
            return t;
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

        public String getPublicId()
        {
            if (otp == null || !YubicoClient.isOtpValidFormat(otp))
            {
                return null;
            }
            return otp.Substring(0, otp.Length - 32);
        }
    }
}
