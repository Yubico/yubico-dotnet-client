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
using System.IO;

namespace YubicoDotNetClient
{
    public sealed class YubicoResponse : IYubicoResponse
    {
        public string H { get; private set; }
        public string T { get; private set; }
        public YubicoResponseStatus Status { get; private set; }
        public int Timestamp { get; private set; }
        public int SessionCounter { get; private set; }
        public int UseCounter { get; private set; }
        public string Sync { get; private set; }
        public string Otp { get; private set; }
        public string Nonce { get; private set; }
        public IEnumerable<KeyValuePair<string, string>> ResponseMap { get; private set; }
        public string PublicId { get; private set; }
        public string Url { get; private set; }

        public YubicoResponse(string response, string url)
        {
            var reader = new StringReader(response);
            string line;
            
            var responseMap = new SortedDictionary<string, string>();
            ResponseMap = responseMap;
            
            while ((line = reader.ReadLine()) != null)
            {
                var unhandled = false;
                var parts = line.Split(new[] { '=' }, 2);

                switch (parts[0])
                {
                    case "h":
                        H = parts[1];
                        break;
                    case "t":
                        T = parts[1];
                        break;
                    case "status":
                        var statusCode = parts[1];
                        if (statusCode.Equals("EMPTY", StringComparison.OrdinalIgnoreCase))
                        {
                            Status = YubicoResponseStatus.Empty;
                        }
                        else if (statusCode.Equals("OK", StringComparison.OrdinalIgnoreCase))
                        {
                            Status = YubicoResponseStatus.Ok;
                        }
                        else if (statusCode.Equals("BAD_OTP", StringComparison.OrdinalIgnoreCase))
                        {
                            Status = YubicoResponseStatus.BadOtp;
                        }
                        else if (statusCode.Equals("REPLAYED_OTP", StringComparison.OrdinalIgnoreCase))
                        {
                            Status = YubicoResponseStatus.ReplayedOtp;
                        }
                        else if (statusCode.Equals("BAD_SIGNATURE", StringComparison.OrdinalIgnoreCase))
                        {
                            Status = YubicoResponseStatus.BadSignature;
                        }
                        else if (statusCode.Equals("MISSING_PARAMETER", StringComparison.OrdinalIgnoreCase))
                        {
                            Status = YubicoResponseStatus.MissingParameter;
                        }
                        else if (statusCode.Equals("NO_SUCH_CLIENT", StringComparison.OrdinalIgnoreCase))
                        {
                            Status = YubicoResponseStatus.NoSuchClient;                                
                        }
                        else if (statusCode.Equals("OPERATION_NOT_ALLOWED", StringComparison.OrdinalIgnoreCase))
                        {
                            Status = YubicoResponseStatus.OperationNotAllowed;
                        }
                        else if (statusCode.Equals("BACKEND_ERROR", StringComparison.OrdinalIgnoreCase))
                        {
                            Status = YubicoResponseStatus.BackendError;
                        }
                        else if (statusCode.Equals("NOT_ENOUGH_ANSWERS", StringComparison.OrdinalIgnoreCase))
                        {
                            Status = YubicoResponseStatus.NotEnoughAnswers;
                        }
                        else if (statusCode.Equals("REPLAYED_REQUEST", StringComparison.OrdinalIgnoreCase))
                        {
                            Status = YubicoResponseStatus.ReplayedRequest;
                        }
                        else
                        {
                            throw new ArgumentException("Response doesn't look like a validation response.");
                        }
                        break;
                    case "timestamp":
                        Timestamp = int.Parse(parts[1]);
                        break;
                    case "sessioncounter":
                        SessionCounter = int.Parse(parts[1]);
                        break;
                    case "sessionuse":
                        UseCounter = int.Parse(parts[1]);
                        break;
                    case "sl":
                        Sync = parts[1];
                        break;
                    case "otp":
                        Otp = parts[1];
                        break;
                    case "nonce":
                        Nonce = parts[1];
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
            if (Status == YubicoResponseStatus.Empty)
            {
                throw new ArgumentException("Response doesn't look like a validation response.");
            }

            if (Otp != null && Otp.Length > 32 && YubicoClient.IsOtpValidFormat(Otp))
            {
                PublicId = Otp.Substring(0, Otp.Length - 32);
            }

            Url = url;
        }        
    }
}
