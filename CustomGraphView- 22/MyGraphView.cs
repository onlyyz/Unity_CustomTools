using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace MU5Editor.NodeEditor
{
    public class MyGraphView : GraphView
    {
        public EntryNode entryNode;
        public ExitNode exitNode;
        public GraphWindow graphWindow;

        //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
        public MyGraphView(GraphWindow graphWindow) : base()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            Insert(0, new GridBackground());

            //增加控制器
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new ContentDragger()); 
            this.AddManipulator(new RectangleSelector());
            
            //搜索树结构
            SearchWindowProvider searchWindowProvider = ScriptableObject.CreateInstance(typeof(SearchWindowProvider)) as SearchWindowProvider;
            searchWindowProvider.Initialize(this, graphWindow);

            nodeCreationRequest += context =>
            {
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindowProvider);
            };

            //创建节点
            CreateBasicNodes();
        }

        //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
        public void CreateBasicNodes()
        {
            entryNode = new EntryNode();
            entryNode.SetPosition(new Rect(0, 0, 0, 0));
            AddElement(entryNode);

            exitNode = new ExitNode();
            exitNode.SetPosition(new Rect(580, 0, 0, 0));
            AddElement(exitNode);
        }

        //端点判断，Edge 是否 不符合条件
        public override List<Port> GetCompatiblePorts(Port startAnchor, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();
            foreach (var port in ports.ToList())
            {
                if (startAnchor.node == port.node ||
                    startAnchor.direction == port.direction ||
                    startAnchor.portType != port.portType)
                {
                    continue;
                }

                compatiblePorts.Add(port);
            }
            return compatiblePorts;
        }

        public void DeleteAllElements()
        {
            foreach (var element in graphElements)
            {
                RemoveElement(element);
            }
        }

        #region Load Data

        //加载节点数据
        public void LoadNodeData(NodeData nodeData)
        {
            MU5Node node = (MU5Node)Activator.CreateInstance(nodeData.nodeType);
            node.LoadData(nodeData);
            AddElement(node);
        }

        //加载链接数据
        public void LoadEdgeData(EdgeData edgeData)
        {
            Edge edge = new Edge();
            
            MU5Node outputNode = GetMU5NodeByUid(edgeData.uid_outputNode);
            MU5Node inputNode = GetMU5NodeByUid(edgeData.uid_inputNode);

            edge.output = outputNode.port_dict[edgeData.uid_outputPort];
            outputNode.port_dict[edgeData.uid_outputPort].Connect(edge);
            edge.input = inputNode.port_dict[edgeData.uid_inputPort];
            inputNode.port_dict[edgeData.uid_inputPort].Connect(edge);

            AddElement(edge);
        }

        public MU5Node GetMU5NodeByUid(string _uid)
        {
            MU5Node node = null;
            foreach (var graphElement in graphElements)
            {
                MU5Node _node = graphElement as MU5Node;
                if (_node == null) continue;
                if (_node.uid != _uid) continue;

                node = _node;
                break;
            }
            return node;
        }
        
        #endregion
    }
}
