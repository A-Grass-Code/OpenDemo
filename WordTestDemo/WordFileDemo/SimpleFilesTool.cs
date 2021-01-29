using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordFileDemo
{
    /// <summary>
    /// 一个简单的文件工具类
    /// </summary>
    public static class SimpleFilesTool
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

    }
}
