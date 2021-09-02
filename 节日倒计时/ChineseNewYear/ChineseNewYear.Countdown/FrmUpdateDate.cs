using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChineseNewYear.Countdown
{
    public partial class FrmUpdateDate : Form
    {
        #region 单例模式

        private FrmUpdateDate()
        {
            InitializeComponent();
        }

        private static readonly object _lockCreateInstrance = new object();

        private static FrmUpdateDate _thisForm = null;

        public static FrmUpdateDate CreateInstrance()
        {
            if (_thisForm == null || _thisForm.IsDisposed)
            {
                lock (_lockCreateInstrance)
                {
                    if (_thisForm == null || _thisForm.IsDisposed)
                    {
                        _thisForm = new FrmUpdateDate();
                    }
                }
            }
            return _thisForm;
        }

        #endregion


        internal Point MainLocation { get; set; }

        internal Size MainSize { get; set; }


        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txt_festivalName.Text))
            {
                MessageBox.Show("请输入“节日名称”！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                FilesTool.WriteFileCreate(Countdown.ConfigPath,
                    $"{this.txt_festivalName.Text.Trim()}{Environment.NewLine}{this.date_festivalDate.Value:yyyy-MM-dd}");
                this.Close();

                var frm = FrmMain.CreateInstrance();
                frm.FestivalInfo = Countdown.GetFestivalInfo();
                frm.StartCountdown();
            }
            catch (Exception)
            {
                MessageBox.Show("保存失败，请重新操作！", "异常提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmUpdateDate_Load(object sender, EventArgs e)
        {
            this.Location = new Point(MainLocation.X, MainLocation.Y + MainSize.Height + 10);
            try
            {
                (string FestivalName, DateTime FestivalDate) festivalInfo = Countdown.GetFestivalInfo();
                this.txt_festivalName.Text = festivalInfo.FestivalName;
                this.date_festivalDate.Value = festivalInfo.FestivalDate;
            }
            catch (Exception)
            {
                this.txt_festivalName.Text = string.Empty;
                this.date_festivalDate.Value = DateTime.Now;
            }
        }
    }
}
