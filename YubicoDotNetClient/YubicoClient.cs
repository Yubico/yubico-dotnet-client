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
using System.Security.Cryptography;

namespace YubicoDotNetClient
{
    public class YubicoClient
    {
        private String clientId;
        private byte[] apiKey = null;
        private String sync;

        private String[] apiUrls = {
                                       "https://api.yubico.com/wsapi/2.0/verify",
                                       "https://api2.yubico.com/wsapi/2.0/verify",
                                       "https://api3.yubico.com/wsapi/2.0/verify",
                                       "https://api4.yubico.com/wsapi/2.0/verify",
                                       "https://api5.yubico.com/wsapi/2.0/verify"
                                   };

        public YubicoClient(String clientId)
        {
            this.clientId = clientId;
        }

        public void setApiKey(String apiKey)
        {
            this.apiKey = Convert.FromBase64String(apiKey);
        }

        public void setSync(String sync)
        {
            this.sync = sync;
        }

        public void setUrls(String[] urls)
        {
            this.apiUrls = urls;
        }

        public YubicoResponse verify(String otp)
        {
            if (!isOtpValidFormat(otp))
            {
                throw new FormatException("otp format is invalid");
            }

            String nonce = generateNonce();
            SortedDictionary<String, String> queryMap = new SortedDictionary<String, String>();
            queryMap.Add("id", clientId);
            queryMap.Add("nonce", nonce);
            queryMap.Add("otp", otp);
            queryMap.Add("timestamp", "1");
            if (sync != null)
            {
                queryMap.Add("sl", sync);
            }
            String query = null;
            foreach (KeyValuePair<String, String> pair in queryMap)
            {
                if (query == null)
                {
                    query = "";
                }
                else
                {
                    query += "&";
                }
                query += pair.Key + "=" + pair.Value;
            }

            if (apiKey != null)
            {
                query += "&h=" + doSignature(query, apiKey).Replace("+", "%2B");
            }

            List<String> urls = new List<String>();
            foreach (String url in apiUrls)
            {
                urls.Add(url + "?" + query);
            }
            YubicoResponse response = YubicoValidate.validate(urls.ToArray());

            if (apiKey != null && response.getStatus() != YubicoResponseStatus.BAD_SIGNATURE)
            {
                String responseString = null;
                String serverSignature = null;
                foreach (KeyValuePair<String, String> pair in response.getResponseMap())
                {
                    if (pair.Key.Equals("h"))
                    {
                        serverSignature = pair.Value;
                    }
                    else
                    {

                        if (responseString == null)
                        {
                            responseString = "";
                        }
                        else
                        {
                            responseString += "&";
                        }
                        responseString += pair.Key + "=" + pair.Value;
                    }
                }
                String clientSignature = doSignature(responseString, apiKey);
                if (serverSignature != null && !clientSignature.Equals(serverSignature))
                {
                    throw new YubicoValidationFailure("Server signature did not match our key.");
                }
            }

            if (response != null && response.getStatus() == YubicoResponseStatus.OK)
            {
                if (!response.getNonce().Equals(nonce))
                {
                    throw new YubicoValidationFailure("Nonce in request and response does not match, man in the middle?");
                }
                else if (!response.getOtp().Equals(otp))
                {
                    throw new YubicoValidationFailure("OTP in request and response does not match, man in the middle?");
                }
            }
            return response;
        }

        private static String doSignature(String message, byte[] key)
        {
            HMACSHA1 hmac = new HMACSHA1(key);
            byte[] signature = hmac.ComputeHash(Encoding.ASCII.GetBytes(message));
            return Convert.ToBase64String(signature);
        }

        private static String generateNonce()
        {
            RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();
            byte[] nonce = new byte[16];
            random.GetBytes(nonce);
            return BitConverter.ToString(nonce).Replace("-", "");
        }

        private static int OTP_MAXLENGTH = 48;
        private static int OTP_MINLENGTH = 32;
        public static bool isOtpValidFormat(String otp)
        {
            if (otp.Length > OTP_MAXLENGTH || otp.Length < OTP_MINLENGTH)
            {
                return false;
            }
            foreach (char c in otp.ToCharArray())
            {
                if (c < 0x20 || c > 0x7e)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
