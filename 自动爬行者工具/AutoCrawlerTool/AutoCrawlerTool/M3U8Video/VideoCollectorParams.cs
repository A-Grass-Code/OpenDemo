using AutoCrawlerTool.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AutoCrawlerTool.M3U8Video
{
    internal class VideoCollectorParams
    {
        private static readonly string _videoCollectorParamsFilePath =
            $"{FilesTool.ProgramRootDirectoryOther("CollectorParamsConfig")}videoCollectorParams.json";

        private static VideoCollectorParams _this = null;


        public static VideoCollectorParams GetParams()
        {
            if (_this == null)
            {
                if (File.Exists(_videoCollectorParamsFilePath))
                {
                    _this = JsonTool.JsonFileToObject<VideoCollectorParams>(_videoCollectorParamsFilePath);
                }
                else
                {
                    _this = new VideoCollectorParams();
                }
            }
            return _this;
        }

        public static void SetParams(VideoCollectorParams collectorParams)
        {
            _this = collectorParams;
            JsonTool.ObjectToJsonFile(_this, _videoCollectorParamsFilePath);
        }


        public string ResourceUrl { get; set; } = string.Empty;

        public string ResourceDirectoryUrl { get; set; } = string.Empty;

        public string SaveDirectory { get; set; } = string.Empty;

        public string SaveName { get; set; } = string.Empty;

        public string M3u8UrlMatchReg { get; set; } = string.Empty;

        public int DownSpeedIndex { get; set; } = 2;
    }
}
