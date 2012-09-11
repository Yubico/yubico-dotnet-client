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
    /// <summary>
    /// Validation client for the Yubico validation protocol version 2.0
    /// </summary>
    /// <example>
    /// YubicoClient client = new YubicoClient(clientId, apiKey);
    /// IYubicoResponse response = client.verify(otp);
    /// if(response.getStatus() == YubicoResponseStatus.OK) {
    ///   // validation succeeded
    /// } else {
    ///  // validation failure
    /// }
    /// </example>
    public class YubicoClient
    {
        private const int OtpMaxlength = 48;
        private const int OtpMinlength = 32;

        private readonly String _clientId;
        private byte[] _apiKey;
        private String _sync;
        private String _nonce;
        private String _userAgent;

        private String[] _apiUrls = {
                                       "https://api.yubico.com/wsapi/2.0/verify",
                                       "https://api2.yubico.com/wsapi/2.0/verify",
                                       "https://api3.yubico.com/wsapi/2.0/verify",
                                       "https://api4.yubico.com/wsapi/2.0/verify",
                                       "https://api5.yubico.com/wsapi/2.0/verify"
                                   };
        /// <summary>
        /// Constructor for YubicoClient with clientId.
        /// </summary>
        /// <param name="clientId">ClientId from https://upgrade.yubico.com/getapikey/ </param>
        public YubicoClient(String clientId)
        {
            this._clientId = clientId;
        }

        /// <summary>
        /// Constructor for YubicoClient with clientId and apiKey.
        /// </summary>
        /// <param name="clientId">ClientId from https://upgrade.yubico.com/getapikey/ </param>
        /// <param name="apiKey">ApiKey from https://upgrade.yubico.com/getapikey/ </param>
        public YubicoClient(String clientId, String apiKey)
        {
            this._clientId = clientId;
            SetApiKey(apiKey);
        }

        /// <summary>
        /// Set the api key
        /// </summary>
        /// <param name="apiKey">ApiKey from http://upgrade.yubico.com/getapikey/ </param>
        /// <exception cref="FormatException"/>
        public void SetApiKey(String apiKey)
        {
            this._apiKey = Convert.FromBase64String(apiKey);
        }

        /// <summary>
        /// Set the desired sync level that the validation server should reach before sending reply.
        /// </summary>
        /// <param name="sync">Desired sync level in percent</param>
        public void SetSync(String sync)
        {
            this._sync = sync;
        }

        /// <summary>
        /// Set the list of validation urls to do validation against.
        /// </summary>
        /// <param name="urls">list of urls to do validation to</param>
        public void SetUrls(String[] urls)
        {
            this._apiUrls = urls;
        }

        /// <summary>
        /// Set the nonce to be used for the next requests. If this is unset a random nonce will be used.
        /// </summary>
        /// <param name="nonce">nonce to be used for the next request</param>
        public void SetNonce(String nonce)
        {
            this._nonce = nonce;
        }

        /// <summary>
        /// Set the user agent used in requests. If this isn't set one will be generated.
        /// </summary>
        /// <param name="userAgent">the user agent to be used in verification requests</param>
        public void SetUserAgent(String userAgent)
        {
            this._userAgent = userAgent;
        }

        /// <summary>
        /// Do verification of OTP
        /// </summary>
        /// <param name="otp">The OTP from a YubiKey in modhex</param>
        /// <returns>IYubicoResponse indicating status of the request</returns>
        /// <exception cref="YubicoValidationFailure"/>
        /// <exception cref="FormatException"/>
        public IYubicoResponse Verify(String otp)
        {
            if (!IsOtpValidFormat(otp))
            {
                throw new FormatException("otp format is invalid");
            }

            if (_nonce == null)
            {
                _nonce = GenerateNonce();
            }

            SortedDictionary<String, String> queryMap = new SortedDictionary<String, String>();
            queryMap.Add("id", _clientId);
            queryMap.Add("nonce", _nonce);
            queryMap.Add("otp", otp);
            queryMap.Add("timestamp", "1");
            if (_sync != null)
            {
                queryMap.Add("sl", _sync);
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
                query += pair.Key + "=" +  Uri.EscapeDataString(pair.Value);
            }

            if (_apiKey != null)
            {
                query += "&h=" + Uri.EscapeDataString(DoSignature(query, _apiKey));
            }

            List<String> urls = new List<String>();
            foreach (String url in _apiUrls)
            {
                urls.Add(url + "?" + query);
            }
            IYubicoResponse response = YubicoValidate.validate(urls, _userAgent);

            if (_apiKey != null && response.Status != YubicoResponseStatus.BAD_SIGNATURE)
            {
                String responseString = null;
                String serverSignature = null;
                foreach (KeyValuePair<String, String> pair in response.ResponseMap)
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
                String clientSignature = DoSignature(responseString, _apiKey);
                if (serverSignature == null || !clientSignature.Equals(serverSignature))
                {
                    throw new YubicoValidationFailure("Server signature did not match our key.");
                }
            }

            if (response != null && response.Status == YubicoResponseStatus.OK)
            {
                if (!response.Nonce.Equals(_nonce))
                {
                    throw new YubicoValidationFailure("Nonce in request and response does not match, man in the middle?");
                }
                else if (!response.Otp.Equals(otp))
                {
                    throw new YubicoValidationFailure("OTP in request and response does not match, man in the middle?");
                }
            }

            // set nonce to null so we will generate a new one for the next request
            _nonce = null;
            return response;
        }

        private static String DoSignature(String message, byte[] key)
        {
            HMACSHA1 hmac = new HMACSHA1(key);
            byte[] signature = hmac.ComputeHash(Encoding.ASCII.GetBytes(message));
            return Convert.ToBase64String(signature);
        }

        private static String GenerateNonce()
        {
            RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();
            byte[] nonce = new byte[16];
            random.GetBytes(nonce);
            return BitConverter.ToString(nonce).Replace("-", "");
        }

        /// <summary>
        /// Verify an OTP is valid format for authentication
        /// </summary>
        /// <param name="otp">The otp from a YubiKey in modhex.</param>
        /// <returns>bool indicating valid or not</returns>
        public static bool IsOtpValidFormat(String otp)
        {
            if (otp.Length > OtpMaxlength || otp.Length < OtpMinlength)
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
