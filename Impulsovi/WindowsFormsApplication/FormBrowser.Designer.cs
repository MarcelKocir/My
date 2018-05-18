namespace WindowsFormsApplication
{
    partial class FormBrowser
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
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bStart = new System.Windows.Forms.Button();
            this.bCaptureImage = new System.Windows.Forms.Button();
            this.bRefresh = new System.Windows.Forms.Button();
            this.lInfo = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // webBrowser
            // 
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.Location = new System.Drawing.Point(0, 0);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(645, 331);
            this.webBrowser.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.bStart);
            this.panel1.Controls.Add(this.bCaptureImage);
            this.panel1.Controls.Add(this.bRefresh);
            this.panel1.Controls.Add(this.lInfo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 331);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(645, 61);
            this.panel1.TabIndex = 1;
            // 
            // bStart
            // 
            this.bStart.Location = new System.Drawing.Point(25, 17);
            this.bStart.Name = "bStart";
            this.bStart.Size = new System.Drawing.Size(75, 23);
            this.bStart.TabIndex = 2;
            this.bStart.Text = "START";
            this.bStart.UseVisualStyleBackColor = true;
            this.bStart.Click += new System.EventHandler(this.bStart_Click);
            // 
            // bCaptureImage
            // 
            this.bCaptureImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bCaptureImage.Location = new System.Drawing.Point(433, 17);
            this.bCaptureImage.Name = "bCaptureImage";
            this.bCaptureImage.Size = new System.Drawing.Size(75, 23);
            this.bCaptureImage.TabIndex = 1;
            this.bCaptureImage.Text = "Capture";
            this.bCaptureImage.UseVisualStyleBackColor = true;
            this.bCaptureImage.Click += new System.EventHandler(this.bCaptureImage_Click);
            // 
            // bRefresh
            // 
            this.bRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bRefresh.Location = new System.Drawing.Point(524, 17);
            this.bRefresh.Name = "bRefresh";
            this.bRefresh.Size = new System.Drawing.Size(75, 23);
            this.bRefresh.TabIndex = 0;
            this.bRefresh.Text = "Refresh";
            this.bRefresh.UseVisualStyleBackColor = true;
            this.bRefresh.Click += new System.EventHandler(this.bRefresh_Click);
            // 
            // lInfo
            // 
            this.lInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lInfo.Location = new System.Drawing.Point(131, 17);
            this.lInfo.Name = "lInfo";
            this.lInfo.Size = new System.Drawing.Size(296, 23);
            this.lInfo.TabIndex = 0;
            this.lInfo.Text = "Stopped";
            this.lInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FormBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(645, 392);
            this.Controls.Add(this.webBrowser);
            this.Controls.Add(this.panel1);
            this.Name = "FormBrowser";
            this.Text = "FormBrowser";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button bRefresh;
        private System.Windows.Forms.Button bCaptureImage;
        private System.Windows.Forms.Button bStart;
        private System.Windows.Forms.Label lInfo;
    }
}