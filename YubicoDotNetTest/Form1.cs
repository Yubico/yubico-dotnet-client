using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
            YubicoResponse response = client.verify(otp);
            if (response != null)
            {
                output.Text = response.getStatus().ToString() + "\r\n" +
                    response.getPublicId() + "\r\n" +
                    response.getUseCounter() + " " + response.getSessionCounter();
            }
        }
    }
}
