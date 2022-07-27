using AutoCrawlerTool.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoCrawlerTool.M3U8Video
{
    public partial class VideoCollectorForm : Form
    {
        public VideoCollectorForm()
        {
            InitializeComponent();
        }


        private readonly ConcurrentDictionary<int, byte[]> _tsVideos = new ConcurrentDictionary<int, byte[]>();

        private void SetVideoCollectorParams()
        {
            VideoCollectorParams collectorParams = new VideoCollectorParams();
            collectorParams.ResourceUrl = this.Txt_Url.Text;
            collectorParams.SaveDirectory = this.Txt_SaveDirectory.Text;
            collectorParams.SaveName = this.Txt_VideoName.Text;
            collectorParams.M3u8UrlMatchReg = this.Txt_m3u8UrlReg.Text;
            collectorParams.DownSpeedIndex = this.ComBox_Speed.SelectedIndex;
            VideoCollectorParams.SetParams(collectorParams);
        }


        private void VideoCollectorForm_Load(object sender, EventArgs e)
        {
            VideoCollectorParams collectorParams = VideoCollectorParams.GetParams();
            this.Txt_Url.Text = collectorParams.ResourceUrl;
            this.Txt_SaveDirectory.Text = collectorParams.SaveDirectory;
            this.Txt_VideoName.Text = collectorParams.SaveName;
            this.Txt_m3u8UrlReg.Text = collectorParams.M3u8UrlMatchReg;
            this.ComBox_Speed.SelectedIndex = collectorParams.DownSpeedIndex;
        }

        private void VideoCollectorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainForm.CollectorFormCount--;
            SetVideoCollectorParams();
        }


        private void Btn_SaveDirectory_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择视频保存的目录";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    this.Txt_SaveDirectory.Text = dialog.SelectedPath;
                }
            }
        }

        private void Btn_OpenDirectory_Click(object sender, EventArgs e)
        {
            string videoSaveDirectory = this.Txt_SaveDirectory.Text.Trim();
            if (string.IsNullOrWhiteSpace(videoSaveDirectory))
            {
                return;
            }
            Process.Start("explorer.exe", videoSaveDirectory + "\\");
        }


        private void Btn_Get_Click(object sender, EventArgs e)
        {
            string resourceUrl = this.Txt_Url.Text.Trim();
            string videoSaveDirectory = this.Txt_SaveDirectory.Text.Trim();
            string videoName = this.Txt_VideoName.Text.Trim();

            if (string.IsNullOrWhiteSpace(resourceUrl)
                || string.IsNullOrWhiteSpace(videoSaveDirectory)
                || string.IsNullOrWhiteSpace(videoName))
            {
                MessageBox.Show($"请确保 视频资源链接、保存目录和视频名 输入的是有效信息", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int downSpeed = Convert.ToInt32(this.ComBox_Speed.Text);
            string m3u8UrlReg = this.Txt_m3u8UrlReg.Text.Trim();

            string cacheDirectory = $"{FilesTool.ProgramRootDirectoryOther("缓存")}{videoName}";
            // 验证文件名是否合法（可用）
            try
            {
                FilesTool.DeleteDirectory(cacheDirectory);
                string testPath = $"{cacheDirectory}\\{videoName}.txt";
                FilesTool.CreateFilePathDirectory(testPath);
                File.WriteAllText(testPath, "测试");
            }
            catch (Exception)
            {
                MessageBox.Show($"文件名格式不正确", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 输入验证通过后，设置一些参数（本地化保存）
            SetVideoCollectorParams();

            // 重置界面上的一些参数
            {
                this.Btn_Get.Enabled = false;
                this.progressBar1.Visible = true;

                this.RTxt_Log.Clear();
                this.Lab_ElapsedTime.Text = $"耗时 00m 00.00s";
                this.Lab_ElapsedTime.Visible = true;
                this.Lab_Msg.Text = "视频保存？？";
                this.Lab_Msg.Visible = true;
            }

            // 计算耗时
            bool isFinish = false;
            {
                DateTime beginTime = DateTime.Now;
                _ = Task.Run(async () =>
                {
                    while (!isFinish)
                    {
                        await Task.Delay(500);
                        this.BeginInvoke(new Action(() =>
                        {
                            var times = DateTime.Now - beginTime;
                            this.Lab_ElapsedTime.Text = $"耗时 {(int)times.TotalMinutes}m {times.Seconds}s";
                        }));
                    }
                });
            }

            _ = Task.Run(async () =>
            {
                try
                {
                    List<string> tsUrls = await VideoCollectorTool.GetM3U8TsUrlsAsync(resourceUrl, m3u8UrlReg, $"{cacheDirectory}\\m3u8.txt");
                    List<object> tsInfoList = new List<object>();
                    int index = 0;
                    foreach (string item in tsUrls)
                    {
                        index++;
                        tsInfoList.Add((index, item));
                    }
                    this.BeginInvoke(new Action<int>(n =>
                    {
                        this.progressBar1.Maximum = n;
                    }), tsInfoList.Count);

                    #region 自动化任务下载视频片段
                    async Task func(object tsInfo)
                    {
                        (int Index, string Url) ts = ((int, string))tsInfo;
                        (bool IsDown, byte[] Bytes) res = await VideoCollectorTool.DownloadVideoClipAsync(ts.Url);
                        string msg = res.IsDown ? "成功" : "失败";

                        try
                        {
                            _tsVideos.AddOrUpdate(ts.Index, res.Bytes, (k, v) => v);
                        }
                        catch (Exception)
                        {
                            _tsVideos.AddOrUpdate(ts.Index, default(byte[]), (k, v) => default(byte[]));
                            msg = "异常";
                        }

                        this.BeginInvoke(new Action<(int Index, string Msg, string Url)>(x =>
                        {
                            if (x.Msg == "成功")
                            {
                                this.RTxt_Log.SelectionColor = Color.Green;
                            }
                            else if (x.Msg == "失败")
                            {
                                this.RTxt_Log.SelectionColor = Color.Orange;
                            }
                            else
                            {
                                this.RTxt_Log.SelectionColor = Color.Red;
                            }

                            this.RTxt_Log.AppendText($"{x.Index} - {x.Msg} - {x.Url}{Environment.NewLine}");
                        }), (ts.Index, msg, ts.Url));
                    }
                    var autoAction = new AutoActionControl(func, tsInfoList, downSpeed, 1000).StartExecute();
                    #endregion

                    #region 视频片段写入 .ts 文件
                    string tsFilePath = $"{cacheDirectory}\\0ts.ts";
                    await Task.Run(async () =>
                    {
                        int key = 1;
                        while (key <= index)
                        {
                            int count = 600; // 5分钟
                            for (int i = 0; i <= count; i++)
                            {
                                this.BeginInvoke(new Action<int>(n =>
                                {
                                    this.progressBar1.Value = n;
                                }), autoAction.RunCompleted);

                                if (_tsVideos.ContainsKey(key) && _tsVideos.TryRemove(key, out byte[] v))
                                {
                                    VideoCollectorTool.BytesWriteFileAppend(v, tsFilePath);
                                    key++;
                                    break;
                                }
                                else
                                {
                                    if (i >= count)
                                    {
                                        this.BeginInvoke(new Action<int>(n =>
                                        {
                                            this.RTxt_Log.SelectionColor = Color.Red;
                                            this.RTxt_Log.AppendText($"{n} - 视频片段写入失败 {Environment.NewLine}");
                                        }), key);
                                        key++;
                                        break;
                                    }
                                    await Task.Delay(500);
                                }
                            }
                        }
                    });
                    #endregion

                    bool isSucc = await VideoCollectorTool.TsFilesMergeMP4Async(cacheDirectory, $"{videoSaveDirectory}\\{videoName}.mp4");
                    this.BeginInvoke(new Action<bool>(b =>
                    {
                        if (b)
                        {
                            this.Lab_Msg.ForeColor = Color.Green;
                            this.Lab_Msg.Text = "视频保存成功";
                        }
                        else
                        {
                            this.Lab_Msg.ForeColor = Color.Red;
                            this.Lab_Msg.Text = "视频保存失败";
                        }
                    }), isSucc);
                }
                catch (Exception ex)
                {
                    this.BeginInvoke(new Action<Exception>(err =>
                    {
                        this.RTxt_Log.AppendText($"{Environment.NewLine}{err}{Environment.NewLine}{Environment.NewLine}");
                    }), ex);
                }
            }).ContinueWith(t =>
            {
                isFinish = true;
                this.BeginInvoke(new Action(() =>
                {
                    this.Btn_Get.Enabled = true;
                    this.progressBar1.Visible = false;
                    this.progressBar1.Value = 0;
                }));
            });
        }
    }
}
