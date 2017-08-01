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
using System.Threading.Tasks;

namespace YubicoDotNetClient
{
    /// <summary>
    /// Validation client for the Yubico validation protocol version 2.0
    /// </summary>
    /// <example>
    /// YubicoClient client = new YubicoClient(clientId, apiKey);
    /// YubicoResponse response = client.Verify(otp);
    /// if(response.Status == YubicoResponseStatus.Ok) 
    /// {
    ///     // validation succeeded
    /// } 
    /// else 
    /// {
    ///     // validation failure
    /// }
    /// </example>
    public sealed class YubicoClient
    {
        private const int OtpMaxlength = 48;
        private const int OtpMinlength = 32;

        private readonly string _clientId;
        private byte[] _apiKey;
        private string _sync;
        private string _nonce;
        private string _userAgent;

        private string[] _apiUrls = 
        {
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
        public YubicoClient(string clientId)
        {
            _clientId = clientId;
        }

        /// <summary>
        /// Constructor for YubicoClient with clientId and apiKey.
        /// </summary>
        /// <param name="clientId">ClientId from https://upgrade.yubico.com/getapikey/ </param>
        /// <param name="apiKey">ApiKey from https://upgrade.yubico.com/getapikey/ </param>
        public YubicoClient(string clientId, string apiKey)
        {
            _clientId = clientId;
            SetApiKey(apiKey);
        }

        /// <summary>
        /// Set the api key
        /// </summary>
        /// <param name="apiKey">ApiKey from http://upgrade.yubico.com/getapikey/ </param>
        /// <exception cref="FormatException"/>
        public void SetApiKey(string apiKey)
        {
            _apiKey = Convert.FromBase64String(apiKey);
        }

        /// <summary>
        /// Set the desired sync level that the validation server should reach before sending reply.
        /// </summary>
        /// <param name="sync">Desired sync level in percent</param>
        public void SetSync(string sync)
        {
            _sync = sync;
        }

        /// <summary>
        /// Set the list of validation urls to do validation against.
        /// </summary>
        /// <param name="urls">list of urls to do validation to</param>
        public void SetUrls(string[] urls)
        {
            _apiUrls = urls;
        }

        /// <summary>
        /// Set the nonce to be used for the next requests. If this is unset a random nonce will be used.
        /// </summary>
        /// <param name="nonce">nonce to be used for the next request</param>
        public void SetNonce(string nonce)
        {
            _nonce = nonce;
        }

        /// <summary>
        /// Set the user agent used in requests. If this isn't set one will be generated.
        /// </summary>
        /// <param name="userAgent">the user agent to be used in verification requests</param>
        public void SetUserAgent(string userAgent)
        {
            _userAgent = userAgent;
        }

        /// <summary>
        /// Do verification of OTP
        /// </summary>
        /// <param name="otp">The OTP from a YubiKey in modhex</param>
        /// <returns>IYubicoResponse indicating status of the request</returns>
        /// <exception cref="YubicoValidationFailure"/>
        /// <exception cref="FormatException"/>
        public async Task<IYubicoResponse> VerifyAsync(string otp)
        {
            if (!IsOtpValidFormat(otp))
            {
                throw new FormatException("otp format is invalid");
            }

            if (_nonce == null)
            {
                _nonce = GenerateNonce();
            }

            var queryMap = new SortedDictionary<string, string>
            {
                {"id",        _clientId}, 
                {"nonce",     _nonce}, 
                {"otp",       otp}, 
                {"timestamp", "1"}
            };

            if (_sync != null)
            {
                queryMap.Add("sl", _sync);
            }

            StringBuilder queryBuilder = null;
            foreach (var pair in queryMap)
            {
                if (queryBuilder == null)
                {
                    queryBuilder = new StringBuilder();
                }
                else
                {
                    queryBuilder.Append("&");
                }
                queryBuilder.AppendFormat("{0}={1}", pair.Key, Uri.EscapeDataString(pair.Value));
            }

            if (_apiKey != null && queryBuilder != null)
            {
                var querySoFar = queryBuilder.ToString();
                queryBuilder.AppendFormat("&h={0}", Uri.EscapeDataString(DoSignature(querySoFar, _apiKey)));
            }

            var urls = _apiUrls.Select(url => string.Format("{0}?{1}", url, queryBuilder)).ToList();
            var response = await YubicoValidate.ValidateAsync(urls, _userAgent);

            if (_apiKey != null && response != null && response.Status != YubicoResponseStatus.BadSignature)
            {
                StringBuilder responseStringBuilder = null;
                string serverSignature = null;
                foreach (var pair in response.ResponseMap)
                {
                    if (pair.Key.Equals("h"))
                    {
                        serverSignature = pair.Value;
                    }
                    else
                    {
                        if (responseStringBuilder == null)
                        {
                            responseStringBuilder = new StringBuilder();
                        }
                        else
                        {
                            responseStringBuilder.Append("&");
                        }
                        responseStringBuilder.AppendFormat("{0}={1}", pair.Key, pair.Value);
                    }
                }
                
                if (responseStringBuilder != null)
                {                    
                    var clientSignature = DoSignature(responseStringBuilder.ToString(), _apiKey);
                    if (serverSignature == null || !clientSignature.Equals(serverSignature))
                    {
                        throw new YubicoValidationFailure("Server signature did not match our key.");
                    }
                }
            }

            if (response != null && response.Status == YubicoResponseStatus.Ok)
            {
                if (!response.Nonce.Equals(_nonce))
                {
                    throw new YubicoValidationFailure("Nonce in request and response does not match, man in the middle?");
                }

                if (!response.Otp.Equals(otp))
                {
                    throw new YubicoValidationFailure("OTP in request and response does not match, man in the middle?");
                }
            }

            // set nonce to null so we will generate a new one for the next request
            _nonce = null;
            return response;
        }

        private static string DoSignature(string message, byte[] key)
        {
            using (var hmac = new HMACSHA1(key))
            {
                var signature = hmac.ComputeHash(Encoding.ASCII.GetBytes(message));
                return Convert.ToBase64String(signature);
            }
        }

        private static string GenerateNonce()
        {
#if NET451
            using(var random = new RNGCryptoServiceProvider())
#else
            using (var random = RandomNumberGenerator.Create())
#endif
            {
                var nonce = new byte[16];
                random.GetBytes(nonce);
                return BitConverter.ToString(nonce).Replace("-", "");
            }
        }

        /// <summary>
        /// Verify an OTP is valid format for authentication
        /// </summary>
        /// <param name="otp">The otp from a YubiKey in modhex.</param>
        /// <returns>bool indicating valid or not</returns>
        public static bool IsOtpValidFormat(string otp)
        {
            if (otp.Length > OtpMaxlength || otp.Length < OtpMinlength)
            {
                return false;
            }
            return otp.ToCharArray().All(c => c >= 0x20 && c <= 0x7e);
        }
    }
}
