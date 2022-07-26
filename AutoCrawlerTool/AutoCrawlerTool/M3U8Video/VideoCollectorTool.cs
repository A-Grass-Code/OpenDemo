using AutoCrawlerTool.Common;
using BrowserCrawler;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AutoCrawlerTool.M3U8Video
{
    internal static class VideoCollectorTool
    {
        private static readonly HttpClient _http = new HttpClient();

        public static async Task<List<string>> GetM3U8TsUrlsAsync(string resourceUrl, string m3u8UrlMatchReg, string m3u8FileSavePath)
        {
            List<string> tsUrls = new List<string>();

            string m3u8Url;
            {
                if (resourceUrl.Substring(resourceUrl.Length - 5).ToLower() == ".m3u8")
                {
                    m3u8Url = resourceUrl;
                }
                else
                {
                    using Page page = await ChromiumBrowser.NewPageAndInitAsync(await MainForm.ChromiumAsync(), true);
                    await page.GotoWaitAfterAsync(resourceUrl);
                    string html = await page.GetContentAsync();
                    m3u8Url = Regex.Match(html, m3u8UrlMatchReg, RegexOptions.Singleline).Groups[1].Value.Trim();
                }

                using Stream stream = await _http.GetStreamAsync(m3u8Url);
                using StreamReader sr = new StreamReader(stream);

                FilesTool.WriteFileCreate(m3u8FileSavePath, await sr.ReadToEndAsync());
            }

            string[] lines = File.ReadAllLines(m3u8FileSavePath);
            foreach (string item in lines)
            {
                string url = item.Trim();
                if (url.Substring(0, 1) != "#")
                {
                    tsUrls.Add(url);
                }
            }

            return tsUrls;
        }

        public static void BytesWriteFileAppend(byte[] bytes, string filePath)
        {
            if (bytes == null || bytes.Length < 1)
                return;

            FilesTool.CreateFilePathDirectory(filePath);

            // 把 byte[] 写入文件
            using FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write);
            using BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(bytes);
        }

        public static async Task<(bool, byte[])> DownloadVideoClipAsync(string url)
        {
            byte[] res = null;
            for (int i = 0; i < 5; i++)
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
                await Task.Delay(new Random().Next(1000, 2000));
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
            bool isSucc = false;

            if (File.Exists(outputVideoPath))
            {
                File.Delete(outputVideoPath);
            }

            if (!Directory.Exists(tsFilesDirectory))
            {
                return false;
            }

            var files = Directory.GetFiles(tsFilesDirectory);
            bool isTs = false;
            foreach (var item in files)
            {
                FileInfo info = new FileInfo(item);
                if (info.Extension.ToLower() == ".ts")
                {
                    isTs = true;
                    break;
                }
            }
            if (!isTs)
            {
                return false;
            }

            FilesTool.CreateFilePathDirectory(outputVideoPath);
            try
            {
                using Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true; // 不显示程序窗口
                process.Start();

                process.StandardInput.WriteLine($"copy /b {tsFilesDirectory}\\*.ts {tsFilesDirectory}\\new.ts");
                for (int i = 0; i < 60; i++)
                {
                    await Task.Delay(1000);
                    if (File.Exists($"{tsFilesDirectory}\\new.ts"))
                    {
                        break;
                    }
                }

                process.StandardInput.WriteLine($"copy /b {tsFilesDirectory}\\*.ts {outputVideoPath}");
                for (int i = 0; i < 300; i++)
                {
                    await Task.Delay(1000);
                    if (File.Exists(outputVideoPath))
                    {
                        isSucc = true;
                        break;
                    }
                }

                process.Close();
            }
            catch (Exception)
            {
                isSucc = false;
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
