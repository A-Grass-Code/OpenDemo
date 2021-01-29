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
        private static bool IsRunningCountdown { get; set; } = false;


        /// <summary>
        /// 启动倒计时
        /// </summary>
        public void StartCountdown()
        {
            if (IsRunningCountdown)
            {
                return;
            }

            Task.Run(async () =>
            {
                IsRunningCountdown = true;
                while (true)
                {
                    try
                    {
                        dynamic dyData = DateTimeTool.ComputeHowLong(DateTime.Now, Countdown.NextChineseNewYearDate);
                        this.BeginInvoke(new Action(() =>
                        {
                            this.lab_year.Text = $"距离 {Countdown.NextChineseNewYearDate.Year} 年春节";
                            this.lab_countdown.Text = $"{dyData.Days}天 {dyData.Hours}时 {dyData.Minutes}分 {dyData.Seconds}秒";
                        }));
                    }
                    catch (Exception)
                    {
                        this.BeginInvoke(new Action(() =>
                        {
                            MessageBox.Show("请更新春节日期！", "异常提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            IsRunningCountdown = false;
                            Countdown.ConfigLastChangeTime = default(DateTime);

                            if (IsRunningCountdown)
                            {
                                this.toolStripButton2.Enabled = false;
                            }
                            else
                            {
                                this.toolStripButton2.Enabled = true;
                            }
                        }));
                        break;
                    }
                    finally
                    {
                        this.BeginInvoke(new Action(() =>
                        {
                            if (IsRunningCountdown)
                            {
                                this.toolStripButton2.Enabled = false;
                            }
                            else
                            {
                                this.toolStripButton2.Enabled = true;
                            }
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
