using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleDemo
{
    /// <summary>
    /// 节点的最短路径
    /// </summary>
    public class NodePath
    {
        public NodePath(Dictionary<string, Node> nodes)
        {
            this.Nodes = nodes;
        }


        private readonly StringBuilder _sbPath = new StringBuilder();

        private readonly List<string> _pathList = new List<string>();

        private readonly Dictionary<string, double> _pathLengthDic = new Dictionary<string, double>();

        /// <summary>
        /// 用来存放所有的节点
        /// </summary>
        public Dictionary<string, Node> Nodes { get; set; }


        /// <summary>
        /// 找出 路
        /// </summary>
        /// <param name="startNode"></param>
        /// <param name="endNode"></param>
        private void FindoutPath(string startNode, string endNode)
        {
            if (!Nodes.ContainsKey(startNode))
            {
                return;
            }

            if (startNode == endNode)
            {
                _sbPath.Append($"-{endNode}");
                _sbPath.AppendLine();
                return;
            }

            var nodePath = Nodes[startNode].EveryPath;
            foreach (var item in nodePath)
            {
                _sbPath.Append($"-{startNode}");
                FindoutPath(item.Key, endNode);
            }
        }

        /// <summary>
        /// 获取两节点之间的所有路径
        /// </summary>
        /// <param name="startNode"></param>
        /// <param name="endNode"></param>
        /// <returns></returns>
        private void GetAllPath(string startNode, string endNode)
        {
            _sbPath.Clear();
            _pathList.Clear();
            _pathLengthDic.Clear();

            FindoutPath(startNode, endNode);

            var pathArr = _sbPath.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in pathArr)
            {
                if (!item.Contains($"-{startNode}"))
                {
                    _pathList.Add($"-{startNode}{item}");
                }
                else
                {
                    _pathList.Add(item);
                }
            }

            for (int i = 0; i < _pathList.Count; i++)
            {
                _pathList[i] = _pathList[i].Remove(0, 1);
            }
        }


        /// <summary>
        /// 计算最短路径
        /// </summary>
        /// <param name="startNode"></param>
        /// <param name="endNode"></param>
        /// <returns></returns>
        public (string Path, double Length, List<KeyValuePair<string, double>> AllPath) ComputeShortestPath(string startNode, string endNode)
        {
            GetAllPath(startNode, endNode);

            foreach (var item in _pathList)
            {
                var nodeIds = item.Split("-", StringSplitOptions.RemoveEmptyEntries);
                string tempNodeId = startNode;
                double length = 0.0;
                foreach (var nodeId in nodeIds)
                {
                    if (nodeId == startNode)
                    {
                        continue;
                    }

                    length += Nodes[tempNodeId].EveryPath[nodeId];

                    tempNodeId = nodeId;
                }
                _pathLengthDic.Add(item, length);
            }

            var temp = _pathLengthDic.OrderBy(x => x.Value).ToList();

            return (temp[0].Key, temp[0].Value, temp);
        }
    }
}
