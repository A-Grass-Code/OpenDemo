using System.Collections.Generic;

namespace ConsoleDemo
{
    /// <summary>
    /// 节点
    /// </summary>
    public class Node
    {
        public Node() { }

        public Node(string nodeName)
        {
            this.NodeName = nodeName;
        }

        public Node(string nodeName, Dictionary<string, double> everyPath)
        {
            this.NodeName = nodeName;
            this.EveryPath = everyPath;
        }


        /// <summary>
        /// 节点名
        /// </summary>
        public string NodeName { get; set; }

        /// <summary>
        /// 当前节点的每一条路径
        /// <para>字典集合中的 key 是当前节点所指向的下一个节点名</para>
        /// <para>字典集合中的 value 是两个节点之间的距离</para>
        /// </summary>
        public Dictionary<string, double> EveryPath { get; set; } = new Dictionary<string, double>();
    }
}
