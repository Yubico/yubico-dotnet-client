namespace YubicoDotNetTest
{
    partial class Form1
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
            this.otpInput = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.clientInput = new System.Windows.Forms.TextBox();
            this.keyInput = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.output = new System.Windows.Forms.Label();
            this.syncInput = new System.Windows.Forms.TextBox();
            this.syncLabel = new System.Windows.Forms.Label();
            this.nonceInput = new System.Windows.Forms.TextBox();
            this.nonce = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // otpInput
            // 
            this.otpInput.Location = new System.Drawing.Point(13, 27);
            this.otpInput.Name = "otpInput";
            this.otpInput.Size = new System.Drawing.Size(100, 20);
            this.otpInput.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 161);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.submit);
            // 
            // clientInput
            // 
            this.clientInput.Location = new System.Drawing.Point(13, 54);
            this.clientInput.Name = "clientInput";
            this.clientInput.Size = new System.Drawing.Size(100, 20);
            this.clientInput.TabIndex = 2;
            // 
            // keyInput
            // 
            this.keyInput.Location = new System.Drawing.Point(13, 81);
            this.keyInput.Name = "keyInput";
            this.keyInput.Size = new System.Drawing.Size(100, 20);
            this.keyInput.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(120, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "OTP";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(120, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "ClientId";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(120, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "apiKey";
            // 
            // output
            // 
            this.output.AutoSize = true;
            this.output.Location = new System.Drawing.Point(10, 212);
            this.output.Name = "output";
            this.output.Size = new System.Drawing.Size(0, 13);
            this.output.TabIndex = 7;
            // 
            // syncInput
            // 
            this.syncInput.Location = new System.Drawing.Point(13, 108);
            this.syncInput.Name = "syncInput";
            this.syncInput.Size = new System.Drawing.Size(100, 20);
            this.syncInput.TabIndex = 8;
            // 
            // syncLabel
            // 
            this.syncLabel.AutoSize = true;
            this.syncLabel.Location = new System.Drawing.Point(120, 108);
            this.syncLabel.Name = "syncLabel";
            this.syncLabel.Size = new System.Drawing.Size(29, 13);
            this.syncLabel.TabIndex = 9;
            this.syncLabel.Text = "sync";
            // 
            // nonceInput
            // 
            this.nonceInput.Location = new System.Drawing.Point(13, 135);
            this.nonceInput.Name = "nonceInput";
            this.nonceInput.Size = new System.Drawing.Size(100, 20);
            this.nonceInput.TabIndex = 10;
            // 
            // nonce
            // 
            this.nonce.AutoSize = true;
            this.nonce.Location = new System.Drawing.Point(120, 135);
            this.nonce.Name = "nonce";
            this.nonce.Size = new System.Drawing.Size(37, 13);
            this.nonce.TabIndex = 11;
            this.nonce.Text = "nonce";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 311);
            this.Controls.Add(this.nonce);
            this.Controls.Add(this.nonceInput);
            this.Controls.Add(this.syncLabel);
            this.Controls.Add(this.syncInput);
            this.Controls.Add(this.output);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.keyInput);
            this.Controls.Add(this.clientInput);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.otpInput);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox otpInput;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox clientInput;
        private System.Windows.Forms.TextBox keyInput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label output;
        private System.Windows.Forms.TextBox syncInput;
        private System.Windows.Forms.Label syncLabel;
        private System.Windows.Forms.TextBox nonceInput;
        private System.Windows.Forms.Label nonce;
    }
}

