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

            YubicoClient client = new YubicoClient(clientId);
            if (!apiKey.Equals(""))
            {
                client.setApiKey(apiKey);
            }
            YubicoResponse response = client.verify(otp);
            output.Text = response.getStatus().ToString();
        }
    }
}
