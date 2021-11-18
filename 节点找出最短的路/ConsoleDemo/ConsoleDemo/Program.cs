using System;
using System.Collections.Generic;

namespace ConsoleDemo
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine();
            Console.WriteLine();

            #region 初始化一些节点数据
            Node a = new Node("a");
            a.EveryPath.Add("b", 2);
            a.EveryPath.Add("c", 6);

            Node b = new Node("b");
            b.EveryPath.Add("e", 5);
            b.EveryPath.Add("d", 5);

            Node c = new Node("c");
            c.EveryPath.Add("e", 3);
            c.EveryPath.Add("f", 7);
            c.EveryPath.Add("d", 8);

            Node d = new Node("d");
            d.EveryPath.Add("f", 10);

            Node e = new Node("e");
            e.EveryPath.Add("f", 7);

            Node f = new Node("f");
            #endregion

            Dictionary<string, Node> nodes = new Dictionary<string, Node>()
            {
                { nameof(a), a },
                { nameof(b), b },
                { nameof(c), c },
                { nameof(d), d },
                { nameof(e), e },
                { nameof(f), f }
            };

            NodePath nodesPath = new NodePath(nodes);

            string startNode = "c";
            string endNode = "f";
            var shortestPath = nodesPath.ComputeShortestPath(startNode, endNode);

            Console.WriteLine($"节点 {startNode} --> 节点 {endNode}");
            Console.WriteLine();
            Console.WriteLine("路长\t路径");
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine();

            if (shortestPath.AllPath == null)
            {
                Console.WriteLine($"此路不通");
            }
            else
            {
                Console.WriteLine("最短路径：");
                Console.WriteLine($"{shortestPath.Length}\t{shortestPath.Path}");
                Console.WriteLine();
                Console.WriteLine("所有路径：");
                foreach (var item in shortestPath.AllPath)
                {
                    Console.WriteLine($"{item.Value}\t{item.Key}");
                }
            }

            Console.WriteLine();
            Console.WriteLine("---------------------------------------------");

            Console.ReadKey();
        }
    }
}
