using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChineseWords.gif
{
    public partial class FormMain : Form
    {
        private HttpClient _httpClient = new HttpClient();

        private string _currentHanzi = string.Empty;

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.ToolStatus_msg.Text = "正在检查网络...";
            Task.Run(async () =>
            {
                if (await CheckInternetTool.InternetIsUseableAsync())
                {
                    this.Invoke(new Action(() =>
                    {
                        this.ToolStatus_msg.Text = "网络正常";
                        this.Btn_get.Enabled = true;
                    }));
                }
                else
                {
                    this.Invoke(new Action(() =>
                    {
                        this.ToolStatus_msg.Text = "网络不可用";
                        this.Btn_get.Enabled = false;
                    }));
                }
            });
        }

        private void Btn_get_Click(object sender, EventArgs e)
        {
            this.Btn_get.Enabled = false;

            string hanzi = this.Txt_hanzi.Text.Trim();
            if (string.IsNullOrWhiteSpace(hanzi) || hanzi.Length < 1)
            {
                MessageBox.Show("请输入一个有效汉字", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Btn_get.Enabled = true;
                return;
            }
            hanzi = hanzi.Substring(0, 1);

            if (hanzi == _currentHanzi)
            {
                this.Btn_get.Enabled = true;
                return;
            }
            else
            {
                _currentHanzi = hanzi;
            }

            Task.Run(async () =>
            {
                string res = await _httpClient.GetStringAsync($"https://hanyu.baidu.com/s?wd={ hanzi }&ptype=zici&tn=sug_click");
                res = Regex.Match(res, "id=\"word_bishun\".*?data-gif=\"(.*?)\"", RegexOptions.Singleline).Groups[1].Value.Trim();
                return res;
            }).ContinueWith(t =>
            {
                if (string.IsNullOrWhiteSpace(t.Result))
                {
                    this.Invoke(new Action(() =>
                    {
                        MessageBox.Show("未能获取到汉字笔顺图，请检查", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.Btn_get.Enabled = true;
                    }));
                    return;
                }

                this.BeginInvoke(new Action<string>(s =>
                {
                    this.Btn_get.Enabled = true;
                    this.Pic_Hanzi_gif.ImageLocation = s;
                }), t.Result);
            });
        }
    }
}
