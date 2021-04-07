using IZhy.Common.Const;
using IZhy.Common.SimpleTools;
using System;
using System.Collections.Generic;
using System.IO;

namespace IZhy.Common.BasicTools
{
    /// <summary>
    /// 公共配置文件 读取工具类
    /// </summary>
    public static class CommonConfigTool
    {
        /// <summary>
        /// 配置文件路径
        /// </summary>
        private static readonly string _configPath = $"{FilesTool.ProgramRootDirectoryCommonConfig}{FileNameConst.CommonConfigJson}";

        /// <summary>
        /// 用来存放 CommonConfig.json 配置文件 的 最后修改时间
        /// </summary>
        public static DateTime CommonConfigLastChangeTime { get; private set; }

        /// <summary>
        /// 获取一个 bool 值，表示 CommonConfig.json 配置文件 是否已被重新修改；true/false 是/否
        /// </summary>
        public static bool IsChangeConfigFile()
        {
            if (!File.Exists(_configPath))
            {
                throw new Exception($"【CommonConfigTool】 数据库连接信息的配置文件不存在，请检查 => [ {_configPath} ]");
            }

            try
            {
                FileInfo fileInfo = new FileInfo(_configPath);
                if (fileInfo.LastWriteTime != CommonConfigLastChangeTime)
                {
                    CommonConfigLastChangeTime = fileInfo.LastWriteTime;
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogsTool.WriteEXLog("【CommonConfigTool】 获取一个 bool 值，表示 CommonConfig.json 配置文件 是否已被重新修改，发生异常", ex.ToString());
                return true;
            }
            return false;
        }


        private static Dictionary<string, object> _allConfigKeyValues = new Dictionary<string, object>();

        /// <summary>
        /// 获取 CommonConfig.json 配置文件里的所有配置信息
        /// </summary>
        public static Dictionary<string, object> AllConfigKeyValues()
        {
            try
            {
                if (_allConfigKeyValues == null || _allConfigKeyValues.Count < 1 || IsChangeConfigFile())
                {
                    _allConfigKeyValues = JsonTool.JsonFileToObject<Dictionary<string, object>>(_configPath);
                }
            }
            catch (Exception ex)
            {
                LogsTool.WriteEXLog("【CommonConfigTool】 获取 CommonConfig.json 配置文件里的配置信息，发生异常", ex);
                _allConfigKeyValues = new Dictionary<string, object>();
            }
            return _allConfigKeyValues;
        }


        /// <summary>
        /// 根据键获取 CommonConfig.json 配置文件里的信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetConfig(string key)
        {
            Dictionary<string, object> dic = AllConfigKeyValues();
            if (dic.ContainsKey(key))
            {
                return dic[key];
            }
            return null;
        }
    }
}
