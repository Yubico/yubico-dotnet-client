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

namespace YubicoDotNetClient
{
    public interface IYubicoResponse
    {
        /// <summary>
        /// Get the servers signature of the response.
        /// </summary>
        /// <returns>Base64 of hmac-sha1 of the response concatenated as url</returns>
        string H { get; }

        /// <summary>
        /// Get the servers response of the timestamp.
        /// </summary>
        /// <returns>timestamp in UTC</returns>
        string T { get; }

        /// <summary>
        /// The response status
        /// </summary>
        /// <returns>status of the response</returns>
        YubicoResponseStatus Status { get; }

        /// <summary>
        /// The YubiKey internal timestamp when OTP was generated
        /// </summary>
        /// <returns>YubiKey internal 8hz timestamp</returns>
        int Timestamp { get; }

        /// <summary>
        /// The YubiKey internal sessionCounter
        /// </summary>
        /// <returns>the YubiKey session counter, counting up for each key press</returns>
        int SessionCounter { get; }

        /// <summary>
        /// The YubiKey internal useCounter
        /// </summary>
        /// <returns>the YubiKey use counter, counts up for each powerup</returns>
        int UseCounter { get; }

        /// <summary>
        /// The Syncronization achieved
        /// </summary>
        /// <returns>syncronization achieved in percent</returns>
        string Sync { get; }

        /// <summary>
        /// The OTP asked about
        /// </summary>
        /// <returns>the OTP that the server is returning a result for</returns>
        string Otp { get; }

        /// <summary>
        /// The nonce that was sent in the request
        /// </summary>
        /// <returns>the nonce that was sent to the server in the request</returns>
        string Nonce { get; }

        /// <summary>
        /// A map of all results returned
        /// </summary>
        /// <returns>map of the results returned from the server</returns>
        IEnumerable<KeyValuePair<string, string>> ResponseMap { get; }

        /// <summary>
        /// The publicId of the OTP this response is about
        /// </summary>
        /// <returns>the publicId for the OTP</returns>
        string PublicId { get; }

    }
}
