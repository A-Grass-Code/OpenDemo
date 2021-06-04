using IZhy.Common.Const;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace IZhy.Common.SimpleTools
{
    /// <summary>
    /// 一个简单的文件操作工具类
    /// </summary>
    public static class FilesTool
    {
        /// <summary>
        /// 当前程序所在磁盘上的根目录
        /// </summary>
        private static readonly string _programRootDirectory;

        /// <summary>
        /// 当前系统的桌面目录
        /// </summary>
        private static readonly string _currentDesktopDirectory;

        static FilesTool()
        {
            // 获取当前程序所在磁盘上的根目录
            {
                // 参考 https://blog.csdn.net/weixin_34025151/article/details/86001814
                string root = AppContext.BaseDirectory;

                if (root.Last<char>() != Path.DirectorySeparatorChar)
                {
                    root += Path.DirectorySeparatorChar;
                }
                _programRootDirectory = root;
            }

            // 获取当前系统的桌面目录
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                if (desktopPath.Last<char>() != Path.DirectorySeparatorChar)
                {
                    desktopPath += Path.DirectorySeparatorChar;
                }
                _currentDesktopDirectory = desktopPath;
            }
        }


        /// <summary>
        /// 获取程序根目录（ ..\ ）
        /// </summary>
        public static string ProgramRootDirectory => _programRootDirectory;

        /// <summary>
        /// 获取当前桌面目录（ ..\ ）
        /// </summary>
        public static string CurrentDesktopDirectory => _currentDesktopDirectory;


        /// <summary>
        /// 获取程序根目录下的 NecessaryConfig 目录（ ..\NecessaryConfig\ ）
        /// </summary>
        public static string ProgramRootDirectoryNecessaryConfig =>
            $"{ProgramRootDirectory}{FolderNameConst.NecessaryConfig}{Path.DirectorySeparatorChar}";

        /// <summary>
        /// 获取程序根目录下的 RunningConfig 目录（ ..\RunningConfig\ ）
        /// </summary>
        public static string ProgramRootDirectoryRunningConfig =>
            $"{ProgramRootDirectory}{FolderNameConst.RunningConfig}{Path.DirectorySeparatorChar}";

        /// <summary>
        /// 获取程序根目录下的 Logs 目录（ ..\Logs\ ）
        /// </summary>
        public static string ProgramRootDirectoryLogs =>
            $"{ProgramRootDirectory}{FolderNameConst.Logs}{Path.DirectorySeparatorChar}";

        /// <summary>
        /// 获取程序根目录下的 CommonConfig 目录（ ..\CommonConfig\ ）
        /// </summary>
        public static string ProgramRootDirectoryCommonConfig =>
            $"{ProgramRootDirectory}{FolderNameConst.CommonConfig}{Path.DirectorySeparatorChar}";


        /// <summary>
        /// 获取程序根目录下的 其他 目录（ ..\{otherDirectory}\ ）
        /// </summary>
        /// <param name="otherDirectory">
        /// <para>可以是一个文件夹名字符串，例如："folder"；</para>
        /// <para>也可以是一个文件夹目录字符串，例如："folder\childFolder"</para>
        /// </param>
        /// <returns></returns>
        public static string ProgramRootDirectoryOther(string otherDirectory) =>
            $"{ProgramRootDirectory}{otherDirectory}{Path.DirectorySeparatorChar}";


        /// <summary>
        /// <para>创建文件路径的目录</para>
        /// <para>给一个绝对路径，如果该路径的目录不存在，该方法会自动创建这个路径的目录</para>
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string CreateFilePathDirectory(string path)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            return path;
        }


        /// <summary>
        /// Append【追加】方式写入文件
        /// </summary>
        /// <param name="path">文件绝对路径</param>
        /// <param name="content">写入内容</param>
        /// <param name="isEndLine">内容末尾处是否自带换行符，默认 false</param>
        /// <param name="encoding">字符编码，默认 UTF-8</param>
        /// <returns></returns>
        public static bool WriteFileAppend(string path, string content, bool isEndLine = false, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            try
            {
                using (FileStream fs = new FileStream(CreateFilePathDirectory(path), FileMode.Append, FileAccess.Write))
                using (StreamWriter sw = new StreamWriter(fs, encoding))
                    if (isEndLine)
                    {
                        sw.Write($"{content}{Environment.NewLine}");
                    }
                    else
                        sw.Write(content);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Create【新建】方式写入文件
        /// </summary>
        /// <param name="path">文件绝对路径</param>
        /// <param name="content">写入内容</param>
        /// <param name="encoding">字符编码，默认 UTF-8</param>
        /// <returns></returns>
        public static bool WriteFileCreate(string path, string content, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            try
            {
                using (FileStream fs = new FileStream(CreateFilePathDirectory(path), FileMode.Create, FileAccess.Write))
                using (StreamWriter sw = new StreamWriter(fs, encoding))
                    sw.Write(content);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 读取全部文件内容为字符串；返回 null 表示失败
        /// </summary>
        /// <param name="path">文件绝对路径</param>
        /// <param name="encoding">字符编码，默认 UTF-8</param>
        /// <returns></returns>
        public static string ReadFileToAll(string path, Encoding encoding = null)
        {
            if (!File.Exists(path))
            {
                throw new Exception($"该路径找不到目标文件 => [ {path} ]");
            }

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                using (StreamReader sr = new StreamReader(fs, encoding))
                    return sr.ReadToEnd();
            }
            catch (Exception)
            {
                return null;
            }
        }


        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath">文件在本机上的绝对路径</param>
        /// <returns></returns>
        public static bool DeleteFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 删除目录（ 包括其里面的子目录 ）
        /// </summary>
        /// <param name="directoryPath">目录在本机上的绝对路径</param>
        /// <returns></returns>
        public static bool DeleteDirectory(string directoryPath)
        {
            try
            {
                if (Directory.Exists(directoryPath))
                {
                    Directory.Delete(directoryPath, true);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取文件大小；单位：字节
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <returns></returns>
        public static long GetFileSize(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    return fileInfo.Length;
                }
                else
                    return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }


        /// <summary>
        /// 清除某一个目录下 指定天数前的 积累性文件
        /// </summary>
        /// <param name="directoryPath">指定的目录路径</param>
        /// <param name="days">积累性文件保留的天数；默认值：10</param>
        public static void ClearDirectoryBeforeSpecifiedDaysFile(string directoryPath, int days = 10)
        {
            try
            {
                if (Directory.Exists(directoryPath))
                {
                    DirectoryInfo[] directoryInfos = new DirectoryInfo(directoryPath).GetDirectories();
                    if (directoryInfos != null && directoryInfos.Length > 0)
                    {
                        foreach (DirectoryInfo item in directoryInfos)
                        {
                            try
                            {
                                if (!Directory.Exists(item.FullName))
                                {
                                    continue;
                                }

                                if (item.CreationTime >= DateTime.Now.AddDays(-days))
                                {
                                    continue;
                                }

                                DeleteDirectory(item.FullName);
                            }
                            catch (Exception)
                            {
                                // 忽略异常 不做任何处理
                                continue;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // 忽略异常 不做任何处理
            }
        }
    }
}
