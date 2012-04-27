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
                queryMap.Add("sync", sync);
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
                query += "&h=" + doSignature(query, apiKey);
            }

            List<String> urls = new List<String>();
            foreach (String url in apiUrls)
            {
                urls.Add(url + "?" + query);
            }
            YubicoResponse response = YubicoValidate.validate(urls.ToArray());

            if (apiKey != null)
            {
                String responseString = null;
                String serverSignature = null;
                foreach (KeyValuePair<String, String> pair in response.getResponseMap())
                {
                    if (pair.Key.Equals("H"))
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
                if (!clientSignature.Equals(serverSignature))
                {
                    throw new YubicoValidationFailure("Server signature did not match our key.");
                }
            }

            if (response.getStatus() == YubicoResponseStatus.OK)
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
            return Convert.ToBase64String(signature).Replace("+", "%2B");
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
