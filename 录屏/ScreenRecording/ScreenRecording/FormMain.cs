using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Point = System.Drawing.Point;
using OcvSize = OpenCvSharp.Size;
using System.Collections.Concurrent;
using System.Diagnostics;
using OpenCvSharp.Extensions;
using ScreenRecording.Properties;
using ScreenRecording.Common;

namespace ScreenRecording
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 用来存放 桌面屏幕图片的线程安全队列
        /// </summary>
        private readonly ConcurrentQueue<Bitmap> _screenImgByteArray = new ConcurrentQueue<Bitmap>();

        /// <summary>
        /// 声明一个 VideoWriter 对象，用来写入视频文件
        /// </summary>
        private VideoWriter _videoWriter = null;

        /// <summary>
        /// 视频写入中锁对象
        /// </summary>
        private readonly object _videoWriteingLock = new object();

        /// <summary>
        /// 声明一个 Rectangle 对象，用来指定矩形的位置和大小
        /// </summary>
        private readonly Rectangle _bounds = Screen.PrimaryScreen.Bounds;

        /// <summary>
        /// 视频的保存目录
        /// </summary>
        private string _saveDirectory = $"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}{Path.DirectorySeparatorChar}";

        /// <summary>
        /// 视频写入 任务
        /// </summary>
        private TimedIntervalTask _timedIntervalTaskVideoWriter;

        /// <summary>
        /// 获取屏幕 任务
        /// </summary>
        private TimedIntervalTask _timedIntervalTaskGetScreenImg;

        /// <summary>
        /// 录屏时长 任务
        /// </summary>
        private TimedIntervalTask _timedIntervalTaskScreenMins;

        /// <summary>
        /// 生成视频所需的 fps 参数值，来自界面上的选择；默认 15
        /// </summary>
        private double _fps = 15;

        /// <summary>
        /// 总帧数
        /// </summary>
        private int _totalFrames = 0;


        /// <summary>
        /// 获取鼠标位置的 Rectangle 对象值
        /// </summary>
        /// <returns></returns>
        private static Rectangle GetMousePositionRectangle()
        {
            return new Rectangle(MousePosition.X + -5, MousePosition.Y + -5, 32, 32);
        }

        /// <summary>
        /// 获取视频帧率所需的时间间隔（ 毫秒 ）
        /// </summary>
        /// <returns></returns>
        private int GetFpsTime()
        {
            switch (_fps)
            {
                default:
                    _fps = 15;
                    return 50;

                case 15: return 50;
                case 20: return 40;
                case 25: return 31;
                case 30: return 28;
            }
        }

        /// <summary>
        /// 获取 桌面屏幕图片
        /// </summary>
        /// <returns></returns>
        private Bitmap GetScreenImgByteArray()
        {
            Bitmap bitmap = new Bitmap(_bounds.Width, _bounds.Height, PixelFormat.Format24bppRgb);

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(Point.Empty, Point.Empty, _bounds.Size, CopyPixelOperation.SourceCopy);

                switch (MouseButtons)
                {
                    default:
                        graphics.DrawIcon(Resources.MousePointerImg, GetMousePositionRectangle());
                        break;

                    case MouseButtons.Left:
                        graphics.DrawIcon(Resources.MouseLeftImg, GetMousePositionRectangle());
                        break;

                    case MouseButtons.Right:
                        graphics.DrawIcon(Resources.MouseRightImg, GetMousePositionRectangle());
                        break;

                    case MouseButtons.Middle:
                        graphics.DrawIcon(Resources.MouseWheelImg, GetMousePositionRectangle());
                        break;
                }

                return bitmap;
            }
        }


        private void FormMain_Load(object sender, EventArgs e)
        {
            GcProcessor.Init(45);

            this.ComBox_fps.SelectedIndex = 0;

            _timedIntervalTaskGetScreenImg = new TimedIntervalTask(() =>
            {
                if (_timedIntervalTaskGetScreenImg.IsRunning)
                {
                    _screenImgByteArray.Enqueue(GetScreenImgByteArray());
                    _totalFrames++;
                }
            }, GetFpsTime());

            _timedIntervalTaskVideoWriter = new TimedIntervalTask(() =>
            {
                int count = _screenImgByteArray.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        Bitmap img = null;
                        try
                        {
                            if (_screenImgByteArray.TryDequeue(out img))
                            {
                                using (Mat mat = BitmapConverter.ToMat(img))
                                {
                                    using (InputArray input = InputArray.Create(mat))
                                    {
                                        lock (_videoWriteingLock)
                                        {
                                            _videoWriter?.Write(input);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                        finally
                        {
                            img?.Dispose();
                            img = null;
                        }
                    }
                }
            }, 1000);

            _timedIntervalTaskScreenMins = new TimedIntervalTask(() =>
            {
                var ts = TimeSpan.FromSeconds(_totalFrames / _fps);

                this.BeginInvoke(new Action(() =>
                {
                    this.Lab_mins.Text = $"{ts.Hours} : {ts.Minutes} : {ts.Seconds} s";
                }));
            }, 200);
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((_timedIntervalTaskGetScreenImg?.IsStarted ?? false) || (_timedIntervalTaskVideoWriter?.IsStarted ?? false))
            {
                if (!_timedIntervalTaskGetScreenImg.IsStop || !_timedIntervalTaskVideoWriter.IsStop)
                {
                    MessageBox.Show("录屏任务没有结束", "退出提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                    return;
                }
            }

            if (_videoWriter != null)
            {
                MessageBox.Show("录屏任务没有结束", "退出提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                return;
            }
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }


        private void ComBox_fps_SelectedIndexChanged(object sender, EventArgs e)
        {
            _fps = Convert.ToInt32(this.ComBox_fps.SelectedItem);
        }

        private void Btn_directorySelect_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowser = new FolderBrowserDialog())
            {
                if (folderBrowser.ShowDialog() == DialogResult.OK)
                {
                    _saveDirectory = $"{folderBrowser.SelectedPath}{Path.DirectorySeparatorChar}";
                    this.Txt_saveDirectory.Text = _saveDirectory;
                }
            }
        }

        private void Btn_openDirectory_Click(object sender, EventArgs e)
        {
            Process.Start(_saveDirectory);
        }


        private void Btn_begin_Click(object sender, EventArgs e)
        {
            string path = $"{_saveDirectory}ScreenVideo-{DateTime.Now:yyyyMMdd_HHmmss}.mp4";
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            this.Txt_saveDirectory.Text = path;

            _videoWriter = new VideoWriter(path, new FourCC(FourCC.XVID), _fps, new OcvSize(_bounds.Width, _bounds.Height));

            this.ComBox_fps.Enabled = false;
            this.Btn_begin.Enabled = false;
            this.Btn_pauseOrContinue.Enabled = true;

            _totalFrames = 0;
            _timedIntervalTaskScreenMins.Startup();
            _timedIntervalTaskVideoWriter.Startup();

            _timedIntervalTaskGetScreenImg.IntervalTime = GetFpsTime();
            _timedIntervalTaskGetScreenImg.Startup();
        }

        private void Btn_pauseOrContinue_Click(object sender, EventArgs e)
        {
            if (!_timedIntervalTaskGetScreenImg.IsStarted)
            {
                return;
            }

            if (_timedIntervalTaskGetScreenImg.IsRunning)
            {
                _timedIntervalTaskGetScreenImg.Pause();
                this.Btn_pauseOrContinue.Text = "继 续";
                this.Btn_pauseOrContinue.ForeColor = Color.Blue;
            }
            else
            {
                _timedIntervalTaskGetScreenImg.GoOn();
                this.Btn_pauseOrContinue.Text = "暂 停";
                this.Btn_pauseOrContinue.ForeColor = Color.OrangeRed;
            }
        }

        private void Btn_end_Click(object sender, EventArgs e)
        {
            if (!_timedIntervalTaskGetScreenImg.IsStarted)
            {
                return;
            }

            _timedIntervalTaskGetScreenImg.Stop();
            _timedIntervalTaskVideoWriter.IntervalTime = 500;

            this.Btn_end.Enabled = false;
            this.Btn_pauseOrContinue.Enabled = false;

            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(250);
                    if (_screenImgByteArray.Count < 1)
                    {
                        _timedIntervalTaskVideoWriter.Stop();
                        await Task.Delay(250);
                        break;
                    }
                }
                _timedIntervalTaskScreenMins.Stop();
            }).ContinueWith(t =>
            {
                Task.Run(() =>
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        MessageBox.Show("录屏视频已结束并保存完成", "录屏提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }));
                });

                lock (_videoWriteingLock)
                {
                    _videoWriter?.Dispose();
                    _videoWriter = null;
                }

                this.BeginInvoke(new Action(() =>
                {
                    this.Btn_pauseOrContinue.Text = "暂 停";
                    this.Btn_pauseOrContinue.ForeColor = Color.OrangeRed;
                    this.Btn_pauseOrContinue.Enabled = false;

                    this.Btn_begin.Enabled = true;
                    this.Btn_end.Enabled = true;
                    this.ComBox_fps.Enabled = true;
                }));
            });
        }
    }
}
