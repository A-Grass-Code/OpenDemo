using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChineseNewYear.Countdown
{
    public partial class FrmMain : Form
    {
        #region 单例模式

        private FrmMain()
        {
            InitializeComponent();
        }

        private static readonly object _lockCreateInstrance = new object();

        private static FrmMain _thisForm = null;

        public static FrmMain CreateInstrance()
        {
            if (_thisForm == null || _thisForm.IsDisposed)
            {
                lock (_lockCreateInstrance)
                {
                    if (_thisForm == null || _thisForm.IsDisposed)
                    {
                        _thisForm = new FrmMain();
                    }
                }
            }
            return _thisForm;
        }

        #endregion


        /// <summary>
        /// 是否正在运行倒计时
        /// </summary>
        private bool IsRunningCountdown { get; set; } = false;

        /// <summary>
        /// 节日信息
        /// </summary>
        internal (string FestivalName, DateTime FestivalDate) FestivalInfo { get; set; }


        /// <summary>
        /// 启动倒计时
        /// </summary>
        internal void StartCountdown()
        {
            if (IsRunningCountdown)
            {
                return;
            }

            Task.Run(async () =>
            {
                IsRunningCountdown = true;
                FestivalInfo = Countdown.GetFestivalInfo();
                while (true)
                {
                    try
                    {
                        this.BeginInvoke(new Action(() =>
                        {
                            if (FestivalInfo.FestivalDate.Year < DateTime.Now.Year || string.IsNullOrWhiteSpace(FestivalInfo.FestivalName))
                            {
                                this.lab_year.Text = $"距离 ... 年";
                                this.lab_festivalName.Text = "... 节日";
                                this.lab_countdown.Text = $"{0}天 {0}时 {0}分 {0}秒";
                            }
                            else
                            {
                                dynamic dyData = DateTimeTool.ComputeHowLong(DateTime.Now, FestivalInfo.FestivalDate);
                                this.lab_year.Text = $"距离 {FestivalInfo.FestivalDate.Year} 年";
                                this.lab_festivalName.Text = FestivalInfo.FestivalName;
                                this.lab_festivalDate.Text = FestivalInfo.FestivalDate.ToString("yyyy年M月d日");
                                this.lab_countdown.Text = $"{dyData.Days}天 {dyData.Hours}时 {dyData.Minutes}分 {dyData.Seconds}秒";
                            }
                        }));
                    }
                    catch (Exception)
                    {
                        IsRunningCountdown = false;
                        this.BeginInvoke(new Action(() =>
                        {
                            MessageBox.Show("请更新春节日期！", "异常提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            this.toolStripButton2.Enabled = !IsRunningCountdown;
                        }));
                        break;
                    }
                    finally
                    {
                        this.BeginInvoke(new Action(() =>
                        {
                            this.toolStripButton2.Enabled = !IsRunningCountdown;
                        }));
                    }
                    await Task.Delay(100);
                }
            });
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            StartCountdown();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            FrmUpdateDate frm = FrmUpdateDate.CreateInstrance();
            frm.MainLocation = this.Location;
            frm.MainSize = this.Size;
            frm.ShowDialog();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            StartCountdown();
        }
    }
}
