using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordFileDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("程序开始...");

            Task.Run(() =>
            {
                WordTool.WordTemplateReplace($"{WordTool.WordTemplateDirectory}测试模板-0.docx",
                    $"{WordTool.WordOutputDirectory}模板输出-{DateTime.Now.ToString("HH_mm_ss_ffff")}.docx",
                    new Dictionary<string, string>()
                    {
                        ["name"] = "张三",
                        ["sex"] = "23",
                        ["age"] = "男",
                    },
                    new Dictionary<string, WordImg>()
                    {
                        ["picture"] = new WordImg(WordTool.TestImagePath, 70, 30),
                        ["picture2"] = new WordImg(WordTool.TestImagePath, 70, 30),
                        ["picture3"] = new WordImg(WordTool.TestImagePath, 70, 30),
                    });

                WordTool.WordTemplateReplace($"{WordTool.WordTemplateDirectory}新模板1.docx",
                    $"{WordTool.WordOutputDirectory}模板输出--{DateTime.Now.ToString("HH_mm_ss_ffff")}.docx",
                    new Dictionary<string, string>()
                    {
                        ["nd"] = "2021",
                        ["yd"] = "01",
                        ["rd"] = "28",
                    },
                    new Dictionary<string, WordImg>()
                    {
                        ["picture"] = new WordImg(WordTool.TestImagePath, 70, 30),
                    });

            }).ContinueWith(t =>
            {
                Console.WriteLine("程序运行完毕！");
                Console.WriteLine("\n按任意键退出！");
            });

            Console.ReadKey();
        }
    }
}
