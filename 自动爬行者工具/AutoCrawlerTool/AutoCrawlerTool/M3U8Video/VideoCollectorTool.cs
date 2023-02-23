using AutoCrawlerTool.Common;
using BrowserCrawler;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AutoCrawlerTool.M3U8Video
{
    internal static class VideoCollectorTool
    {
        private static readonly HttpClient _http = new HttpClient();

        private static bool RunCmdCommand(string command)
        {
            try
            {
                using (Process process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo(@"C:\Windows\system32\cmd.exe")
                    {
                        UseShellExecute = false,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true, // 不显示程序窗口
                    };
                    process.Start();
                    process.StandardInput.WriteLine(command);
                    process.StandardInput.AutoFlush = true;
                    process.StandardInput.WriteLine("exit");
                    process.WaitForExit();
                    process.Close();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static async Task<List<string>> GetM3U8TsUrlsAsync(string resourceUrl, string m3u8UrlMatchReg, string m3u8FileSavePath,
            string resourceDirectoryUrl = null)
        {
            resourceUrl = resourceUrl.Replace("\\", string.Empty);
            string m3u8Url;
            {
                if (resourceUrl.Substring(resourceUrl.Length - 5).ToLower() == ".m3u8")
                {
                    m3u8Url = resourceUrl;
                }
                else
                {
                    using (Page page = await ChromiumBrowser.NewPageAndInitAsync(await MainForm.ChromiumAsync(), true))
                    {
                        await page.GotoWaitAfterAsync(resourceUrl);
                        string html = await page.GetContentAsync();
                        m3u8Url = Regex.Match(html, m3u8UrlMatchReg, RegexOptions.Singleline).Groups[1].Value.Trim().Replace("\\", string.Empty);
                    }
                }
            }

            List<string> lines = new List<string>();
            Uri uri = default;
            for (int c = 0; c < 6; c++)
            {
                try
                {
                    uri = new Uri(m3u8Url);
                    using (Stream stream = await _http.GetStreamAsync(m3u8Url))
                    {
                        using (StreamReader sr = new StreamReader(stream))
                            FilesTool.WriteFileCreate(m3u8FileSavePath, await sr.ReadToEndAsync());
                    }
                    lines = File.ReadAllLines(m3u8FileSavePath).ToList();
                    for (int i = 0; i < lines.Count; i++)
                    {
                        lines[i] = lines[i].Trim();
                    }

                    if (lines.Contains("#EXT-X-ENDLIST"))
                    {
                        break;
                    }
                    else
                    {
                        string newUrl = lines.Find(x => x.Substring(0, 1) != "#");
                        if (newUrl.Contains("http"))
                        {
                            m3u8Url = newUrl;
                        }
                        else
                        {
                            m3u8Url = $"{uri.Scheme}://{uri.Host}/{newUrl}";
                        }

                        lines.Clear();
                        await Task.Delay(new Random().Next(500, 1500));
                    }
                }
                catch (Exception)
                {
                    lines.Clear();
                    await Task.Delay(new Random().Next(500, 1500));
                }
            }

            List<string> tsUrls = new List<string>();
            foreach (string item in lines)
            {
                string url = item.Trim();
                if (url.Substring(0, 1) != "#")
                {
                    if (!string.IsNullOrWhiteSpace(resourceDirectoryUrl))
                    {
                        url = resourceDirectoryUrl.Trim() + url;
                    }

                    if (url.Contains("http"))
                    {
                        tsUrls.Add(url);
                    }
                    else
                    {
                        tsUrls.Add($"{uri?.Scheme}://{uri?.Host}/{url}");
                    }
                }
            }

            lines.Clear();
            return tsUrls;
        }

        public static void BytesWriteFileAppend(byte[] bytes, string filePath)
        {
            if (bytes == null || bytes.Length < 1)
                return;

            FilesTool.CreateFilePathDirectory(filePath);

            // 把 byte[] 写入文件
            using (FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                    bw.Write(bytes);
            }
        }

        public static async Task<(bool, byte[])> DownloadVideoClipAsync(string url)
        {
            byte[] res = null;
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    res = await _http.GetByteArrayAsync(url);
                    break;
                }
                catch (Exception)
                {
                    res = null;
                }
                await Task.Delay(new Random().Next(2000, 3000));
            }

            if (res == null || res.Length < 1)
            {
                return (false, null);
            }
            else
            {
                return (true, res);
            }
        }

        public static async Task<bool> TsFilesMergeMP4Async(string tsFilesDirectory, string outputVideoPath)
        {
            if (File.Exists(outputVideoPath))
            {
                File.Delete(outputVideoPath);
            }

            if (!Directory.Exists(tsFilesDirectory))
            {
                return false;
            }

            var files = Directory.GetFiles(tsFilesDirectory);
            bool isExistsTs = false;
            foreach (var item in files)
            {
                FileInfo info = new FileInfo(item);
                if (info.Extension.ToLower() == ".ts")
                {
                    isExistsTs = true;
                    break;
                }
            }

            if (isExistsTs)
                FilesTool.CreateFilePathDirectory(outputVideoPath);
            else
                return false;

            bool isSucc = RunCmdCommand($"copy /b \"{tsFilesDirectory}\\*.ts\" \"{tsFilesDirectory}\\new.ts\"");
            if (isSucc)
            {
                for (int i = 0; i < 60; i++)
                {
                    await Task.Delay(1000);
                    if (File.Exists($"{tsFilesDirectory}\\new.ts"))
                    {
                        isSucc = true;
                        break;
                    }
                    else
                    {
                        isSucc = false;
                    }
                }
            }
            else
            {
                return false;
            }

            if (!isSucc)
            {
                return false;
            }

            isSucc = RunCmdCommand($"copy /b \"{tsFilesDirectory}\\*.ts\" \"{outputVideoPath}\"");
            if (isSucc)
            {
                for (int i = 0; i < 150; i++)
                {
                    await Task.Delay(1000);
                    if (File.Exists(outputVideoPath))
                    {
                        isSucc = true;
                        break;
                    }
                    else
                    {
                        isSucc = false;
                    }
                }
            }
            else
            {
                return false;
            }

            if (isSucc)
            {
                _ = Task.Run(async () =>
                {
                    for (int i = 0; i < 5; i++)
                    {
                        await Task.Delay(1000);
                        if (FilesTool.DeleteDirectory(tsFilesDirectory))
                        {
                            break;
                        }
                    }
                });
            }
            return isSucc;
        }

    }
}
