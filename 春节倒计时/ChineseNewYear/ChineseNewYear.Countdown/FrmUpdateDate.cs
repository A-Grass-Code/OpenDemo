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


        public Point MainLocation { get; set; }

        public Size MainSize { get; set; }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                FilesTool.WriteFileCreate(FilesTool.ConfigPath, this.dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                this.Close();

                FrmMain.CreateInstrance().StartCountdown();
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
            this.Location = new Point(MainLocation.X + MainSize.Width + 30, MainLocation.Y);
            try
            {
                this.dateTimePicker1.Value = Countdown.NextChineseNewYearDate;
            }
            catch (Exception ex)
            {
                Countdown.ConfigLastChangeTime = default(DateTime);
            }
        }
    }
}
