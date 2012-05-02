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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using YubicoDotNetClient;

namespace YubicoDotNetTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void submit(object sender, EventArgs e)
        {
            String otp = otpInput.Text;
            String clientId = clientInput.Text;
            String apiKey = keyInput.Text;
            String sync = syncInput.Text;
            String nonce = nonceInput.Text;
            output.Text = "";

            YubicoClient client = new YubicoClient(clientId);
            if (!apiKey.Equals(""))
            {
                client.setApiKey(apiKey);
            }
            if (!sync.Equals(""))
            {
                client.setSync(sync);
            }
            if (!nonce.Equals(""))
            {
                client.setNonce(nonce);
            }
            try
            {
                Stopwatch sw = Stopwatch.StartNew();
                YubicoResponse response = client.verify(otp);
                sw.Stop();
                if (response != null)
                {
                    output.Text = "response in: " + sw.ElapsedMilliseconds + "\r\n" +
                        response.getStatus().ToString() + "\r\n" +
                        response.getPublicId() + "\r\n" +
                        response.getUseCounter() + " " + response.getSessionCounter();
                }
                else
                {
                    output.Text = "Null result returned, error in call";
                }
            }
            catch (YubicoValidationFailure yvf)
            {
                output.Text = "Failure in validation: " + yvf.Message;
            }
        }
    }
}
