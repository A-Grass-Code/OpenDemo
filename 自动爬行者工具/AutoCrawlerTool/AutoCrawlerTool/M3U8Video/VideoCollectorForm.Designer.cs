
namespace AutoCrawlerTool.M3U8Video
{
    partial class VideoCollectorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VideoCollectorForm));
            this.label1 = new System.Windows.Forms.Label();
            this.Txt_Url = new System.Windows.Forms.TextBox();
            this.Btn_Get = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Btn_SaveDirectory = new System.Windows.Forms.Button();
            this.Txt_SaveDirectory = new System.Windows.Forms.TextBox();
            this.Txt_VideoName = new System.Windows.Forms.TextBox();
            this.RTxt_Log = new System.Windows.Forms.RichTextBox();
            this.Btn_OpenDirectory = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.Lab_Msg = new System.Windows.Forms.Label();
            this.ComBox_Speed = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Lab_ElapsedTime = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.Txt_m3u8UrlReg = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 15);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "视频资源链接";
            // 
            // Txt_Url
            // 
            this.Txt_Url.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Txt_Url.Location = new System.Drawing.Point(140, 12);
            this.Txt_Url.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Txt_Url.Name = "Txt_Url";
            this.Txt_Url.Size = new System.Drawing.Size(541, 23);
            this.Txt_Url.TabIndex = 1;
            // 
            // Btn_Get
            // 
            this.Btn_Get.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_Get.Location = new System.Drawing.Point(605, 137);
            this.Btn_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Btn_Get.Name = "Btn_Get";
            this.Btn_Get.Size = new System.Drawing.Size(76, 28);
            this.Btn_Get.TabIndex = 12;
            this.Btn_Get.Text = "获 取";
            this.Btn_Get.UseVisualStyleBackColor = true;
            this.Btn_Get.Click += new System.EventHandler(this.Btn_Get_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 73);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "视频保存目录";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 102);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "视频名";
            // 
            // Btn_SaveDirectory
            // 
            this.Btn_SaveDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_SaveDirectory.Location = new System.Drawing.Point(605, 69);
            this.Btn_SaveDirectory.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Btn_SaveDirectory.Name = "Btn_SaveDirectory";
            this.Btn_SaveDirectory.Size = new System.Drawing.Size(34, 25);
            this.Btn_SaveDirectory.TabIndex = 6;
            this.Btn_SaveDirectory.Text = "...";
            this.Btn_SaveDirectory.UseVisualStyleBackColor = true;
            this.Btn_SaveDirectory.Click += new System.EventHandler(this.Btn_SaveDirectory_Click);
            // 
            // Txt_SaveDirectory
            // 
            this.Txt_SaveDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Txt_SaveDirectory.Location = new System.Drawing.Point(140, 70);
            this.Txt_SaveDirectory.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Txt_SaveDirectory.Name = "Txt_SaveDirectory";
            this.Txt_SaveDirectory.ReadOnly = true;
            this.Txt_SaveDirectory.Size = new System.Drawing.Size(457, 23);
            this.Txt_SaveDirectory.TabIndex = 5;
            // 
            // Txt_VideoName
            // 
            this.Txt_VideoName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Txt_VideoName.Location = new System.Drawing.Point(140, 99);
            this.Txt_VideoName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Txt_VideoName.Name = "Txt_VideoName";
            this.Txt_VideoName.Size = new System.Drawing.Size(541, 23);
            this.Txt_VideoName.TabIndex = 9;
            // 
            // RTxt_Log
            // 
            this.RTxt_Log.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RTxt_Log.Location = new System.Drawing.Point(12, 171);
            this.RTxt_Log.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.RTxt_Log.Name = "RTxt_Log";
            this.RTxt_Log.ReadOnly = true;
            this.RTxt_Log.Size = new System.Drawing.Size(669, 159);
            this.RTxt_Log.TabIndex = 16;
            this.RTxt_Log.Text = "";
            // 
            // Btn_OpenDirectory
            // 
            this.Btn_OpenDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_OpenDirectory.Location = new System.Drawing.Point(647, 69);
            this.Btn_OpenDirectory.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Btn_OpenDirectory.Name = "Btn_OpenDirectory";
            this.Btn_OpenDirectory.Size = new System.Drawing.Size(34, 25);
            this.Btn_OpenDirectory.TabIndex = 7;
            this.Btn_OpenDirectory.Text = "O";
            this.Btn_OpenDirectory.UseVisualStyleBackColor = true;
            this.Btn_OpenDirectory.Click += new System.EventHandler(this.Btn_OpenDirectory_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(140, 139);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(238, 23);
            this.progressBar1.TabIndex = 14;
            this.progressBar1.Visible = false;
            // 
            // Lab_Msg
            // 
            this.Lab_Msg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Lab_Msg.AutoSize = true;
            this.Lab_Msg.Location = new System.Drawing.Point(386, 142);
            this.Lab_Msg.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lab_Msg.Name = "Lab_Msg";
            this.Lab_Msg.Size = new System.Drawing.Size(80, 17);
            this.Lab_Msg.TabIndex = 15;
            this.Lab_Msg.Text = "视频保存？？";
            this.Lab_Msg.Visible = false;
            // 
            // ComBox_Speed
            // 
            this.ComBox_Speed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ComBox_Speed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComBox_Speed.FormattingEnabled = true;
            this.ComBox_Speed.Items.AddRange(new object[] {
            "2",
            "5",
            "10",
            "15",
            "20",
            "25",
            "30"});
            this.ComBox_Speed.Location = new System.Drawing.Point(558, 138);
            this.ComBox_Speed.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ComBox_Speed.Name = "ComBox_Speed";
            this.ComBox_Speed.Size = new System.Drawing.Size(39, 25);
            this.ComBox_Speed.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(474, 142);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 17);
            this.label4.TabIndex = 10;
            this.label4.Text = "下载倍速   X";
            // 
            // Lab_ElapsedTime
            // 
            this.Lab_ElapsedTime.AutoSize = true;
            this.Lab_ElapsedTime.Location = new System.Drawing.Point(12, 142);
            this.Lab_ElapsedTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lab_ElapsedTime.Name = "Lab_ElapsedTime";
            this.Lab_ElapsedTime.Size = new System.Drawing.Size(109, 17);
            this.Lab_ElapsedTime.TabIndex = 13;
            this.Lab_ElapsedTime.Text = "耗时 000m 00.00s";
            this.Lab_ElapsedTime.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 44);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(119, 17);
            this.label5.TabIndex = 2;
            this.label5.Text = "m3u8 URL 正则匹配";
            // 
            // Txt_m3u8UrlReg
            // 
            this.Txt_m3u8UrlReg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Txt_m3u8UrlReg.Location = new System.Drawing.Point(140, 41);
            this.Txt_m3u8UrlReg.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Txt_m3u8UrlReg.Name = "Txt_m3u8UrlReg";
            this.Txt_m3u8UrlReg.Size = new System.Drawing.Size(541, 23);
            this.Txt_m3u8UrlReg.TabIndex = 3;
            // 
            // VideoCollectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 341);
            this.Controls.Add(this.Txt_m3u8UrlReg);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Lab_ElapsedTime);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ComBox_Speed);
            this.Controls.Add(this.Lab_Msg);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.Btn_OpenDirectory);
            this.Controls.Add(this.RTxt_Log);
            this.Controls.Add(this.Txt_VideoName);
            this.Controls.Add(this.Txt_SaveDirectory);
            this.Controls.Add(this.Btn_SaveDirectory);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Btn_Get);
            this.Controls.Add(this.Txt_Url);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MinimumSize = new System.Drawing.Size(550, 300);
            this.Name = "VideoCollectorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "m3u8 视频采集器";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.VideoCollectorForm_FormClosed);
            this.Load += new System.EventHandler(this.VideoCollectorForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Txt_Url;
        private System.Windows.Forms.Button Btn_Get;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button Btn_SaveDirectory;
        private System.Windows.Forms.TextBox Txt_SaveDirectory;
        private System.Windows.Forms.TextBox Txt_VideoName;
        private System.Windows.Forms.RichTextBox RTxt_Log;
        private System.Windows.Forms.Button Btn_OpenDirectory;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label Lab_Msg;
        private System.Windows.Forms.ComboBox ComBox_Speed;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label Lab_ElapsedTime;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox Txt_m3u8UrlReg;
    }
}