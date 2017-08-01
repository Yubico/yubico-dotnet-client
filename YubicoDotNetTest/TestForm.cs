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
using System.Windows.Forms;
using System.Diagnostics;

using YubicoDotNetClient;

namespace YubicoDotNetTest
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private async void Submit(object sender, EventArgs e)
        {
            var otp = OtpInput.Text;
            var clientId = ClientIdInput.Text;
            var apiKey = ApiKeyInput.Text;
            var sync = SyncInput.Text;
            var nonce = NonceInput.Text;

            OutputField.Clear();

            var client = new YubicoClient(clientId);
            if (!string.IsNullOrEmpty(apiKey))
            {
                client.SetApiKey(apiKey);
            }
            if (!string.IsNullOrEmpty(sync))
            {
                client.SetSync(sync);
            }
            if (!string.IsNullOrEmpty(nonce))
            {
                client.SetNonce(nonce);
            }
            try
            {
                var sw = Stopwatch.StartNew();
                var response = await client.VerifyAsync(otp);
                sw.Stop();
                if (response != null)
                {
                    OutputField.AppendText(string.Format("response in: {0}{1}", sw.ElapsedMilliseconds, Environment.NewLine));
                    OutputField.AppendText(string.Format("Status: {0}{1}", response.Status, Environment.NewLine));
                    OutputField.AppendText(string.Format("Public ID: {0}{1}", response.PublicId, Environment.NewLine));
                    OutputField.AppendText(string.Format("Use/Session Counter: {0} {1}{2}", response.UseCounter, response.SessionCounter, Environment.NewLine));
                    OutputField.AppendText(string.Format("Url: {0}", response.Url));
                }
                else
                {
                    OutputField.Text = "Null result returned, error in call";
                }
            }
            catch (YubicoValidationFailure yvf)
            {
                OutputField.Text = string.Format("Failure in validation: {0}{1}", yvf.Message, Environment.NewLine);
            }
        }
    }
}
