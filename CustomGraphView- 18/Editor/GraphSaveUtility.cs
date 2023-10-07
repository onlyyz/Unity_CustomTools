using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using Vector2 = System.Numerics.Vector2;

public class GraphSaveUtility 
{
   private DialogueGraphView _targetGraphView;
   private DialogueContainer _ContainerCache; 

   
   //Graph view 中的所有View 和 Node
   private List<Edge> Edges => _targetGraphView.edges.ToList();
   private List<DialogueNode> Nodes => _targetGraphView.nodes.ToList().Cast<DialogueNode>().ToList();
   
   public static GraphSaveUtility GetInstance(DialogueGraphView targetGraphView)
   {
      return new GraphSaveUtility
      {
         _targetGraphView = targetGraphView
      };
      
   }

   public void SaveGraph(string fileName)
   {
      //创建一个新的对话容器
      var dialogueContainer = ScriptableObject.CreateInstance<DialogueContainer>();

      if (!SaveNodes(dialogueContainer)) return;
      SaveExposeProperties(dialogueContainer);
      
      if (!AssetDatabase.IsValidFolder("Assets/Resources"))
         AssetDatabase.CreateFolder("Assets", "Resources");
      
      AssetDatabase.CreateAsset(dialogueContainer,$"Assets/Resources/{fileName}.asset");
      AssetDatabase.SaveAssets();      
      
   }

   private bool SaveNodes(DialogueContainer dialogueContainer)
   {
      if (!Edges.Any()) return false;  //no edges( no connections ) then return
       
      //过滤一个序列
      var connectedPorts = Edges.Where(x => x.input.node != null).ToArray();
      
      //Edge Link Data
      for (int i = 0; i < connectedPorts.Length; i++)
      {
         var outputNode = connectedPorts[i].output.node as DialogueNode;
         var inputNode = connectedPorts[i].input.node as DialogueNode;
         
         //保存标识符
         dialogueContainer.Nodelinks.Add(new NodeLinkData
         {
            BaseNodeGuid = outputNode.GUID,
            PortName = connectedPorts[i].output.portName,
            TargetNodeGuid = inputNode.GUID
         });
      }

      //Node Data Nodes Data
      foreach (var dialogueNode in Nodes.Where(node=>!node.EntryPoint))
      {
         //传递到容器中
         dialogueContainer.DialogueNodeData.Add(new DialogueNodeData
         {
            Guid = dialogueNode.GUID,
            DialogueText = dialogueNode.DialogueText,
            Position = dialogueNode.GetPosition().position
         });
      }

      return true;
   }

   private void SaveExposeProperties(DialogueContainer dialogueContainer)
   {
      dialogueContainer.ExposeProperty.AddRange(_targetGraphView.ExposeProperties);
   }
   
   
   public void LoadGraph(string fileName)
   {
      _ContainerCache = Resources.Load<DialogueContainer>(fileName);

      if (_ContainerCache == null)
      {
         EditorUtility.DisplayDialog("File Not Found", "Target Dialogue Graph File does not exists!", "OK");
         return;
      }

      //清除、生成、连接 
      ClearGraph();
      CreateNodes();
      ConnectNodes();
      CreateExposeProperties();
   }

  

   #region Load Data for the Container
   private void ClearGraph()
   {
      //set Entry points guid back from the save . Discard existing guid.
      Nodes.Find(x => x.EntryPoint).GUID = _ContainerCache.Nodelinks[0].BaseNodeGuid;

      foreach (var node in Nodes)
      {
         if(node.EntryPoint) continue;
         
         //Remove edges that connected to this node;
         Edges.Where(x => x.input.node == node).ToList().
            ForEach(edge => _targetGraphView.RemoveElement(edge));
         
         //then remove the node
         _targetGraphView.RemoveElement(node);
      }
   }
   
   private void CreateNodes()
   {
      foreach (var nodeData in _ContainerCache.DialogueNodeData)
      {
         //创建节点
         var tempNode = _targetGraphView.CreateDialogueNode(nodeData.DialogueText,UnityEngine.Vector2.zero);
         tempNode.GUID = nodeData.Guid;
         
         _targetGraphView.AddElement(tempNode);
         
         //port
         var nodePorts = _ContainerCache.Nodelinks.Where
            (x => x.BaseNodeGuid == nodeData.Guid).ToList();
         nodePorts.ForEach(x =>_targetGraphView.AddchoicePort(tempNode,x.PortName));
      }
   }
   private void ConnectNodes()
   {
      for (int i = 0; i < Nodes.Count; i++)
      {
         //获取匹配的GUID
         var connections = _ContainerCache.Nodelinks.Where(x => x.BaseNodeGuid == Nodes[i].GUID).ToList();

         for (int j = 0; j < connections.Count; j++)
         {
            var targetNodeGuid = connections[j].TargetNodeGuid;
            //匹配节点GUID
            var targetNode = Nodes.First(x => x.GUID == targetNodeGuid);
            
            //连接端口
            LinkNodes(Nodes[i].outputContainer[j].Q<Port>(), (Port)targetNode.inputContainer[0]);
            //Position
            targetNode.SetPosition(new Rect(
               _ContainerCache.DialogueNodeData.First(x =>x.Guid == targetNodeGuid).Position,
               _targetGraphView.defaultNodeSize));
         }
      }
   }

   //节点连接
   private void LinkNodes(Port output, Port input)
   {
      //创建一条边
      var tempEdge = new Edge
      {
         output = output,
         input = input
      };
      
      tempEdge?.input.Connect(tempEdge);
      tempEdge?.output.Connect(tempEdge);
      
      _targetGraphView.Add(tempEdge);  
   }
   
   #endregion


   private void CreateExposeProperties()
   {
      //Clear existing Properties on hot-reload
      _targetGraphView.ClearBlackBoardAndExposeProperty();
      //add properties form data
       
      foreach (var exposeProperty in _ContainerCache.ExposeProperty)
      {
         _targetGraphView.AddPropertyToBlackBoard(exposeProperty);
      }
   }
}
