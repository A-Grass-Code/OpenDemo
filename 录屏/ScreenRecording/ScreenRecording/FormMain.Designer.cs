
namespace ScreenRecording
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.ComBox_fps = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Btn_openDirectory = new System.Windows.Forms.Button();
            this.Lab_mins = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Btn_directorySelect = new System.Windows.Forms.Button();
            this.Txt_saveDirectory = new System.Windows.Forms.TextBox();
            this.Btn_end = new System.Windows.Forms.Button();
            this.Btn_pauseOrContinue = new System.Windows.Forms.Button();
            this.Btn_begin = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ComBox_fps
            // 
            this.ComBox_fps.BackColor = System.Drawing.Color.White;
            this.ComBox_fps.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComBox_fps.ForeColor = System.Drawing.Color.Black;
            this.ComBox_fps.FormattingEnabled = true;
            this.ComBox_fps.Items.AddRange(new object[] {
            "15",
            "20",
            "25",
            "30"});
            this.ComBox_fps.Location = new System.Drawing.Point(74, 79);
            this.ComBox_fps.Name = "ComBox_fps";
            this.ComBox_fps.Size = new System.Drawing.Size(59, 25);
            this.ComBox_fps.TabIndex = 5;
            this.ComBox_fps.SelectedIndexChanged += new System.EventHandler(this.ComBox_fps_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(12, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "帧率 FPS";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.ForestGreen;
            this.label2.Location = new System.Drawing.Point(451, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 17);
            this.label2.TabIndex = 9;
            this.label2.Text = "录屏参考时间";
            // 
            // Btn_openDirectory
            // 
            this.Btn_openDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_openDirectory.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn_openDirectory.ForeColor = System.Drawing.Color.SteelBlue;
            this.Btn_openDirectory.Location = new System.Drawing.Point(576, 67);
            this.Btn_openDirectory.Name = "Btn_openDirectory";
            this.Btn_openDirectory.Size = new System.Drawing.Size(101, 46);
            this.Btn_openDirectory.TabIndex = 11;
            this.Btn_openDirectory.Text = "打开视频目录";
            this.Btn_openDirectory.UseVisualStyleBackColor = true;
            this.Btn_openDirectory.Click += new System.EventHandler(this.Btn_openDirectory_Click);
            // 
            // Lab_mins
            // 
            this.Lab_mins.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Lab_mins.AutoSize = true;
            this.Lab_mins.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.Lab_mins.Location = new System.Drawing.Point(451, 93);
            this.Lab_mins.Name = "Lab_mins";
            this.Lab_mins.Size = new System.Drawing.Size(82, 17);
            this.Lab_mins.TabIndex = 10;
            this.Lab_mins.Text = "00 : 00 : 00 s";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Green;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 34);
            this.label1.TabIndex = 1;
            this.label1.Text = "选择录屏视频保存的位置\r\n默认保存在桌面";
            // 
            // Btn_directorySelect
            // 
            this.Btn_directorySelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_directorySelect.ForeColor = System.Drawing.Color.Green;
            this.Btn_directorySelect.Location = new System.Drawing.Point(627, 12);
            this.Btn_directorySelect.Name = "Btn_directorySelect";
            this.Btn_directorySelect.Size = new System.Drawing.Size(50, 41);
            this.Btn_directorySelect.TabIndex = 3;
            this.Btn_directorySelect.Text = ". . .";
            this.Btn_directorySelect.UseVisualStyleBackColor = true;
            this.Btn_directorySelect.Click += new System.EventHandler(this.Btn_directorySelect_Click);
            // 
            // Txt_saveDirectory
            // 
            this.Txt_saveDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Txt_saveDirectory.BackColor = System.Drawing.Color.White;
            this.Txt_saveDirectory.ForeColor = System.Drawing.Color.Black;
            this.Txt_saveDirectory.Location = new System.Drawing.Point(158, 12);
            this.Txt_saveDirectory.Multiline = true;
            this.Txt_saveDirectory.Name = "Txt_saveDirectory";
            this.Txt_saveDirectory.ReadOnly = true;
            this.Txt_saveDirectory.Size = new System.Drawing.Size(463, 41);
            this.Txt_saveDirectory.TabIndex = 2;
            // 
            // Btn_end
            // 
            this.Btn_end.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn_end.ForeColor = System.Drawing.Color.ForestGreen;
            this.Btn_end.Location = new System.Drawing.Point(350, 67);
            this.Btn_end.Name = "Btn_end";
            this.Btn_end.Size = new System.Drawing.Size(80, 46);
            this.Btn_end.TabIndex = 8;
            this.Btn_end.Text = "结 束";
            this.Btn_end.UseVisualStyleBackColor = true;
            this.Btn_end.Click += new System.EventHandler(this.Btn_end_Click);
            // 
            // Btn_pauseOrContinue
            // 
            this.Btn_pauseOrContinue.Enabled = false;
            this.Btn_pauseOrContinue.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn_pauseOrContinue.ForeColor = System.Drawing.Color.OrangeRed;
            this.Btn_pauseOrContinue.Location = new System.Drawing.Point(254, 67);
            this.Btn_pauseOrContinue.Name = "Btn_pauseOrContinue";
            this.Btn_pauseOrContinue.Size = new System.Drawing.Size(80, 46);
            this.Btn_pauseOrContinue.TabIndex = 7;
            this.Btn_pauseOrContinue.Text = "暂 停";
            this.Btn_pauseOrContinue.UseVisualStyleBackColor = true;
            this.Btn_pauseOrContinue.Click += new System.EventHandler(this.Btn_pauseOrContinue_Click);
            // 
            // Btn_begin
            // 
            this.Btn_begin.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn_begin.ForeColor = System.Drawing.Color.Blue;
            this.Btn_begin.Location = new System.Drawing.Point(158, 67);
            this.Btn_begin.Name = "Btn_begin";
            this.Btn_begin.Size = new System.Drawing.Size(80, 46);
            this.Btn_begin.TabIndex = 6;
            this.Btn_begin.Text = "开 始";
            this.Btn_begin.UseVisualStyleBackColor = true;
            this.Btn_begin.Click += new System.EventHandler(this.Btn_begin_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(689, 125);
            this.Controls.Add(this.ComBox_fps);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Btn_openDirectory);
            this.Controls.Add(this.Lab_mins);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Btn_directorySelect);
            this.Controls.Add(this.Txt_saveDirectory);
            this.Controls.Add(this.Btn_end);
            this.Controls.Add(this.Btn_pauseOrContinue);
            this.Controls.Add(this.Btn_begin);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "录屏小工具";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ComBox_fps;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Btn_openDirectory;
        private System.Windows.Forms.Label Lab_mins;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Btn_directorySelect;
        private System.Windows.Forms.TextBox Txt_saveDirectory;
        private System.Windows.Forms.Button Btn_end;
        private System.Windows.Forms.Button Btn_pauseOrContinue;
        private System.Windows.Forms.Button Btn_begin;
    }
}

