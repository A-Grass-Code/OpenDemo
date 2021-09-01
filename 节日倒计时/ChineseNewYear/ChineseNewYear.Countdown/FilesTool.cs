using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChineseNewYear.Countdown
{
    /// <summary>
    /// 一个简单的文件操作工具类
    /// </summary>
    public static class FilesTool
    {
        /// <summary>
        /// 获取程序根目录（ ..\ ）
        /// </summary>
        public static string ProgramRootDirectory
        {
            get
            {
                string root = AppDomain.CurrentDomain.BaseDirectory;
                if (root.Last<char>() != '\\')
                {
                    root += '\\';
                }
                return root;
            }
        }

        /// <summary>
        /// 获取程序根目录下的 其他 目录（ ..\{otherDirectory}\ ）
        /// </summary>
        /// <param name="otherDirectory">
        /// <para>可以是一个文件夹名字符串，例如："folder"；</para>
        /// <para>也可以是一个文件夹目录字符串，例如："folder\childFolder"</para>
        /// </param>
        /// <returns></returns>
        public static string ProgramRootDirectoryOther(string otherDirectory)
        {
            return $"{ProgramRootDirectory}{otherDirectory}\\";
        }

        /// <summary>
        /// 获取当前桌面目录（ ..\ ）
        /// </summary>
        public static string CurrentDesktopDirectory
        {
            get
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                if (path.Last<char>() != '\\')
                {
                    path += '\\';
                }
                return path;
            }
        }


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
        public static void WriteFileAppend(string path, string content, bool isEndLine = false, Encoding encoding = null)
        {
            try
            {
                if (encoding == null)
                {
                    encoding = Encoding.UTF8;
                }

                using (FileStream fs = new FileStream(CreateFilePathDirectory(path), FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs, encoding))
                    {
                        if (isEndLine)
                        {
                            sw.Write($"{content}\n");
                        }
                        else
                            sw.Write(content);
                    }
                }
            }
            catch (Exception)
            {
                //忽略异常 不做任何处理
            }
        }

        /// <summary>
        /// Create【新建】方式写入文件
        /// </summary>
        /// <param name="path">文件绝对路径</param>
        /// <param name="content">写入内容</param>
        /// <param name="encoding">字符编码，默认 UTF-8</param>
        public static void WriteFileCreate(string path, string content, Encoding encoding = null)
        {
            try
            {
                if (encoding == null)
                {
                    encoding = Encoding.UTF8;
                }

                using (FileStream fs = new FileStream(CreateFilePathDirectory(path), FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs, encoding))
                    {
                        sw.Write(content);
                    }
                }
            }
            catch (Exception)
            {
                //忽略异常 不做任何处理
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
            try
            {
                if (encoding == null)
                {
                    encoding = Encoding.UTF8;
                }

                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader sr = new StreamReader(fs, encoding))
                    {
                        return sr.ReadToEnd();
                    }
                }
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
            bool b;
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                b = true;
            }
            catch (Exception)
            {
                b = false;
            }
            return b;
        }

        /// <summary>
        /// 删除目录（ 包括其里面的子目录 ）
        /// </summary>
        /// <param name="directoryPath">目录在本机上的绝对路径</param>
        /// <returns></returns>
        public static bool DeleteDirectory(string directoryPath)
        {
            bool b;
            try
            {
                if (Directory.Exists(directoryPath))
                {
                    Directory.Delete(directoryPath, true);
                }
                b = true;
            }
            catch (Exception)
            {
                b = false;
            }
            return b;
        }


        /// <summary>
        /// 获取文件大小；单位：字节
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <returns></returns>
        public static long GetFileSize(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    return fileInfo.Length;
                }
                catch (Exception)
                {
                    DeleteFile(filePath);
                    return 0;
                }
            }
            else
                return 0;
        }
    }
}
