
namespace ChineseWords.gif
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
            this.label1 = new System.Windows.Forms.Label();
            this.Txt_hanzi = new System.Windows.Forms.TextBox();
            this.Btn_get = new System.Windows.Forms.Button();
            this.Pic_Hanzi_gif = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ToolStatus_msg = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_Hanzi_gif)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "输入汉字";
            // 
            // Txt_hanzi
            // 
            this.Txt_hanzi.Font = new System.Drawing.Font("楷体", 20F);
            this.Txt_hanzi.ForeColor = System.Drawing.Color.Red;
            this.Txt_hanzi.Location = new System.Drawing.Point(12, 51);
            this.Txt_hanzi.MaxLength = 10;
            this.Txt_hanzi.Name = "Txt_hanzi";
            this.Txt_hanzi.Size = new System.Drawing.Size(125, 38);
            this.Txt_hanzi.TabIndex = 1;
            // 
            // Btn_get
            // 
            this.Btn_get.Enabled = false;
            this.Btn_get.ForeColor = System.Drawing.Color.Green;
            this.Btn_get.Location = new System.Drawing.Point(160, 51);
            this.Btn_get.Name = "Btn_get";
            this.Btn_get.Size = new System.Drawing.Size(82, 38);
            this.Btn_get.TabIndex = 2;
            this.Btn_get.Text = "获 取";
            this.Btn_get.UseVisualStyleBackColor = true;
            this.Btn_get.Click += new System.EventHandler(this.Btn_get_Click);
            // 
            // Pic_Hanzi_gif
            // 
            this.Pic_Hanzi_gif.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pic_Hanzi_gif.ImageLocation = "";
            this.Pic_Hanzi_gif.Location = new System.Drawing.Point(12, 104);
            this.Pic_Hanzi_gif.Name = "Pic_Hanzi_gif";
            this.Pic_Hanzi_gif.Size = new System.Drawing.Size(230, 230);
            this.Pic_Hanzi_gif.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Pic_Hanzi_gif.TabIndex = 3;
            this.Pic_Hanzi_gif.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("楷体", 10F);
            this.label2.ForeColor = System.Drawing.Color.Olive;
            this.label2.Location = new System.Drawing.Point(107, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 14);
            this.label2.TabIndex = 4;
            this.label2.Text = "（只取第一个汉字）";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStatus_msg});
            this.statusStrip1.Location = new System.Drawing.Point(0, 345);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(254, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ToolStatus_msg
            // 
            this.ToolStatus_msg.Name = "ToolStatus_msg";
            this.ToolStatus_msg.Size = new System.Drawing.Size(56, 17);
            this.ToolStatus_msg.Text = "消息提示";
            // 
            // FormMain
            // 
            this.AcceptButton = this.Btn_get;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(254, 367);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Pic_Hanzi_gif);
            this.Controls.Add(this.Btn_get);
            this.Controls.Add(this.Txt_hanzi);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("楷体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "汉字笔顺图";
            this.Load += new System.EventHandler(this.FormMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Pic_Hanzi_gif)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Txt_hanzi;
        private System.Windows.Forms.Button Btn_get;
        private System.Windows.Forms.PictureBox Pic_Hanzi_gif;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ToolStatus_msg;
    }
}

