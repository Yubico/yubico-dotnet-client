namespace YubicoDotNetTest
{
    partial class TestForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.OtpInput = new System.Windows.Forms.TextBox();
            this.TestButton = new System.Windows.Forms.Button();
            this.ClientIdInput = new System.Windows.Forms.TextBox();
            this.ApiKeyInput = new System.Windows.Forms.TextBox();
            this.OtpLabel = new System.Windows.Forms.Label();
            this.ClientIdLabel = new System.Windows.Forms.Label();
            this.ApiLabel = new System.Windows.Forms.Label();
            this.SyncInput = new System.Windows.Forms.TextBox();
            this.SyncLabel = new System.Windows.Forms.Label();
            this.NonceInput = new System.Windows.Forms.TextBox();
            this.NonceLabel = new System.Windows.Forms.Label();
            this.OutputField = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // OtpInput
            // 
            this.OtpInput.Location = new System.Drawing.Point(69, 12);
            this.OtpInput.Name = "OtpInput";
            this.OtpInput.Size = new System.Drawing.Size(101, 20);
            this.OtpInput.TabIndex = 0;
            // 
            // TestButton
            // 
            this.TestButton.Location = new System.Drawing.Point(69, 146);
            this.TestButton.Name = "TestButton";
            this.TestButton.Size = new System.Drawing.Size(76, 23);
            this.TestButton.TabIndex = 1;
            this.TestButton.Text = "&Test";
            this.TestButton.UseVisualStyleBackColor = true;
            this.TestButton.Click += new System.EventHandler(this.Submit);
            // 
            // ClientIdInput
            // 
            this.ClientIdInput.Location = new System.Drawing.Point(69, 39);
            this.ClientIdInput.Name = "ClientIdInput";
            this.ClientIdInput.Size = new System.Drawing.Size(101, 20);
            this.ClientIdInput.TabIndex = 2;
            // 
            // ApiKeyInput
            // 
            this.ApiKeyInput.Location = new System.Drawing.Point(69, 66);
            this.ApiKeyInput.Name = "ApiKeyInput";
            this.ApiKeyInput.Size = new System.Drawing.Size(101, 20);
            this.ApiKeyInput.TabIndex = 3;
            // 
            // OtpLabel
            // 
            this.OtpLabel.AutoSize = true;
            this.OtpLabel.Location = new System.Drawing.Point(17, 15);
            this.OtpLabel.Name = "OtpLabel";
            this.OtpLabel.Size = new System.Drawing.Size(29, 13);
            this.OtpLabel.TabIndex = 4;
            this.OtpLabel.Text = "OTP";
            // 
            // ClientIdLabel
            // 
            this.ClientIdLabel.AutoSize = true;
            this.ClientIdLabel.Location = new System.Drawing.Point(17, 42);
            this.ClientIdLabel.Name = "ClientIdLabel";
            this.ClientIdLabel.Size = new System.Drawing.Size(42, 13);
            this.ClientIdLabel.TabIndex = 5;
            this.ClientIdLabel.Text = "ClientId";
            // 
            // ApiLabel
            // 
            this.ApiLabel.AutoSize = true;
            this.ApiLabel.Location = new System.Drawing.Point(17, 69);
            this.ApiLabel.Name = "ApiLabel";
            this.ApiLabel.Size = new System.Drawing.Size(45, 13);
            this.ApiLabel.TabIndex = 6;
            this.ApiLabel.Text = "API Key";
            // 
            // SyncInput
            // 
            this.SyncInput.Location = new System.Drawing.Point(69, 93);
            this.SyncInput.Name = "SyncInput";
            this.SyncInput.Size = new System.Drawing.Size(101, 20);
            this.SyncInput.TabIndex = 8;
            // 
            // SyncLabel
            // 
            this.SyncLabel.AutoSize = true;
            this.SyncLabel.Location = new System.Drawing.Point(17, 96);
            this.SyncLabel.Name = "SyncLabel";
            this.SyncLabel.Size = new System.Drawing.Size(31, 13);
            this.SyncLabel.TabIndex = 9;
            this.SyncLabel.Text = "Sync";
            // 
            // NonceInput
            // 
            this.NonceInput.Location = new System.Drawing.Point(69, 120);
            this.NonceInput.Name = "NonceInput";
            this.NonceInput.Size = new System.Drawing.Size(101, 20);
            this.NonceInput.TabIndex = 10;
            // 
            // NonceLabel
            // 
            this.NonceLabel.AutoSize = true;
            this.NonceLabel.Location = new System.Drawing.Point(17, 123);
            this.NonceLabel.Name = "NonceLabel";
            this.NonceLabel.Size = new System.Drawing.Size(39, 13);
            this.NonceLabel.TabIndex = 11;
            this.NonceLabel.Text = "Nonce";
            // 
            // OutputField
            // 
            this.OutputField.Location = new System.Drawing.Point(15, 184);
            this.OutputField.Multiline = true;
            this.OutputField.Name = "OutputField";
            this.OutputField.ReadOnly = true;
            this.OutputField.Size = new System.Drawing.Size(257, 115);
            this.OutputField.TabIndex = 12;
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 311);
            this.Controls.Add(this.OutputField);
            this.Controls.Add(this.NonceLabel);
            this.Controls.Add(this.NonceInput);
            this.Controls.Add(this.SyncLabel);
            this.Controls.Add(this.SyncInput);
            this.Controls.Add(this.ApiLabel);
            this.Controls.Add(this.ClientIdLabel);
            this.Controls.Add(this.OtpLabel);
            this.Controls.Add(this.ApiKeyInput);
            this.Controls.Add(this.ClientIdInput);
            this.Controls.Add(this.TestButton);
            this.Controls.Add(this.OtpInput);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TestForm";
            this.Text = "Test Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox OtpInput;
        private System.Windows.Forms.Button TestButton;
        private System.Windows.Forms.TextBox ClientIdInput;
        private System.Windows.Forms.TextBox ApiKeyInput;
        private System.Windows.Forms.Label OtpLabel;
        private System.Windows.Forms.Label ClientIdLabel;
        private System.Windows.Forms.Label ApiLabel;
        private System.Windows.Forms.TextBox SyncInput;
        private System.Windows.Forms.Label SyncLabel;
        private System.Windows.Forms.TextBox NonceInput;
        private System.Windows.Forms.Label NonceLabel;
        private System.Windows.Forms.TextBox OutputField;
    }
}

