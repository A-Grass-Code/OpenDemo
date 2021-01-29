using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace WordFileDemo
{
    /// <summary>
    /// Word 工具类
    /// </summary>
    public static class WordTool
    {

        /// <summary>
        /// Word 模板文件 目录
        /// <para>程序根目录\\WordFiles\\Templates\\</para>
        /// </summary>
        public static string WordTemplateDirectory
        {
            get
            {
                return $"{SimpleFilesTool.ProgramRootDirectory}WordFiles\\Templates\\";
            }
        }

        /// <summary>
        /// Word 文件的输出目录
        /// <para>程序根目录\\WordFiles\\Output\\</para>
        /// </summary>
        public static string WordOutputDirectory
        {
            get
            {
                return $"{SimpleFilesTool.ProgramRootDirectory}WordFiles\\Output\\";
            }
        }


        /// <summary>
        /// 测试用的图片路径
        /// </summary>
        public static string TestImagePath
        {
            get
            {
                return $"{SimpleFilesTool.ProgramRootDirectory}TestImages\\test.jpg";
            }
        }


        /// <summary>
        /// Word 模板 替换
        /// <para>当前适用的字段模板形如：[=Name]，其中 Name 就是字段名</para>
        /// <para>返回 true 表示成功</para>
        /// </summary>
        /// <param name="tempPath">Word 文件 模板路径</param>
        /// <param name="newWordPath">生成的新 Word 文件的路径</param>
        /// <param name="textDic">文字字典集合</param>
        /// <param name="imgDic">图片字典集合</param>
        /// <returns></returns>
        public static bool WordTemplateReplace(string tempPath, string newWordPath,
            Dictionary<string, string> textDic,
            Dictionary<string, WordImg> imgDic = null)
        {
            try
            {
                var doc = DocX.Load(tempPath);  // 加载 Word 模板文件

                #region 字段替换文字

                if (textDic != null && textDic.Count > 0)
                {
                    foreach (var paragraph in doc.Paragraphs)   // 遍历当前 Word 文件中的所有（段落）段
                    {
                        foreach (var texts in textDic)
                        {
                            try
                            {
                                paragraph.ReplaceText($"[={texts.Key}]", texts.Value);  // 替换段落中的文字
                            }
                            catch (Exception ex)
                            {
                                // 不处理
                                continue;
                            }
                        }
                    }

                    foreach (var table in doc.Tables)   // 遍历当前 Word 文件中的所有表格
                    {
                        foreach (var row in table.Rows) // 遍历表格中的每一行
                        {
                            foreach (var cell in row.Cells)     //遍历每一行中的每一列
                            {
                                foreach (var paragraph in cell.Paragraphs)  // 遍历当前表格里的所有（段落）段
                                {
                                    foreach (var texts in textDic)
                                    {
                                        try
                                        {
                                            paragraph.ReplaceText($"[={texts.Key}]", texts.Value);  // 替换段落中的文字
                                        }
                                        catch (Exception ex)
                                        {
                                            // 不处理
                                            continue;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                #endregion 

                #region 字段替换图片

                if (imgDic != null && imgDic.Count > 0)
                {
                    foreach (var paragraph in doc.Paragraphs)
                    {
                        foreach (var imgItem in imgDic)
                        {
                            try
                            {
                                var list = paragraph.FindAll($"[={imgItem.Key}]");
                                if (list != null && list.Count > 0)
                                {
                                    Image img = doc.AddImage(imgItem.Value.Path);
                                    Picture pic = img.CreatePicture();
                                    pic.Width = imgItem.Value.Width;
                                    pic.Height = imgItem.Value.Height;
                                    var p = paragraph.InsertPicture(pic, list[0]);
                                    p.ReplaceText($"[={imgItem.Key}]", string.Empty);
                                }
                            }
                            catch (Exception ex)
                            {
                                // 不处理
                                continue;
                            }
                        }
                    }

                    foreach (var table in doc.Tables)
                    {
                        foreach (var row in table.Rows)
                        {
                            foreach (var cell in row.Cells)
                            {
                                foreach (var paragraph in cell.Paragraphs)
                                {
                                    foreach (var imgItem in imgDic)
                                    {
                                        try
                                        {
                                            var list = paragraph.FindAll($"[={imgItem.Key}]");
                                            if (list != null && list.Count > 0)
                                            {
                                                Image img = doc.AddImage(imgItem.Value.Path);
                                                Picture pic = img.CreatePicture();
                                                pic.Width = imgItem.Value.Width;
                                                pic.Height = imgItem.Value.Height;
                                                var p = paragraph.InsertPicture(pic, list[0]);
                                                p.ReplaceText($"[={imgItem.Key}]", string.Empty);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            // 不处理
                                            continue;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                #endregion

                doc.SaveAs(SimpleFilesTool.CreateFilePathDirectory(newWordPath));

                return true;
            }
            catch (Exception ex)
            {
                // 不处理
                return false;
            }
        }

    }


    /// <summary>
    /// 向 Word 文件中插入图片所需的结构体
    /// </summary>
    public struct WordImg
    {
        /// <summary>
        /// WordImg 构造函数
        /// </summary>
        /// <param name="path">目标图片的路径</param>
        /// <param name="width">图片在 Word 文件中所显示的宽度</param>
        /// <param name="height">图片在 Word 文件中所显示的高度</param>
        public WordImg(string path, int width, int height)
        {
            Path = path;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// 目标图片的路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 图片在 Word 文件中所显示的宽度
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 图片在 Word 文件中所显示的高度
        /// </summary>
        public int Height { get; set; }
    }
}
