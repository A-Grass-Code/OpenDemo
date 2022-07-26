using AutoCrawlerTool.Common;
using AutoCrawlerTool.M3U8Video;
using BrowserCrawler;
using PuppeteerSharp;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoCrawlerTool
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }


        private static LaunchOptions _launchOptions;

        private static Browser _browser;

        private static Page _page;

        private static LaunchOptions ChromiumOptions
        {
            get
            {
                if (_launchOptions == null)
                {
                    _launchOptions = ChromiumBrowser.GetNativeChromiumBrowser();
#if DEBUG
                    _launchOptions = ChromiumBrowser.GetNativeChromiumBrowser(ChromiumBrowser.NativeEdgePath, true);
#endif          
                }
                return _launchOptions;
            }
        }

        public static int CollectorFormCount = 0;

        public static async Task<Browser> ChromiumAsync()
        {
            if (_browser == null || _browser.IsClosed)
            {
                _browser = await Puppeteer.LaunchAsync(ChromiumOptions);
                _page = await ChromiumBrowser.NewPageAndInitAsync(_browser);
                await _page.GoToAsync("https://www.baidu.com/");
            }
            return _browser;
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            var p = this.Location;
            this.Location = new Point(p.X + 200, 20);

            Task.Run(async () =>
            {
                bool isSucc = await CheckInternetTool.InternetIsUseableAsync();
                if (!isSucc)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        MessageBox.Show("请检查网络", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }));
                }
            });

            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(1000 * 60);
                    GC.Collect();
                }
            });
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Invoke(new Action(async () =>
            {
                await ((_page?.CloseAsync()) ?? Task.CompletedTask);
                _page?.Dispose();

                await ((_browser?.CloseAsync()) ?? Task.CompletedTask);
                _browser?.Dispose();
            }));
            Environment.Exit(0);
        }


        private void ToolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            VideoCollectorForm form = new VideoCollectorForm();
            form.Show();
            CollectorFormCount++;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CollectorFormCount > 0)
            {
                if (MessageBox.Show("存在未关闭的采集器窗口，可能采集任务尚未完成！\n确定要强制关闭吗？",
                    "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
