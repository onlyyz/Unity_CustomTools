using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

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
      if (!Edges.Any()) return;  //no edges( no connections ) then return
      
      //创建一个新的对话容器
      var dialogueContainer = ScriptableObject.CreateInstance<DialogueContainer>();
       
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
      foreach (var dialogueNode in Nodes.Where(node=>!node.Entrybool))
      {
         //传递到容器中
         dialogueContainer.DialogueNodeData.Add(new DialogueNodeData
         {
            Guid = dialogueNode.GUID,
            DialogueText = dialogueNode.DialogueText,
            Position = dialogueNode.GetPosition().position
         });
      }


      if (!AssetDatabase.IsValidFolder("Assets/Resources"))
         AssetDatabase.CreateFolder("Assets", "Resources");
      
      AssetDatabase.CreateAsset(dialogueContainer,$"Assets/Resources/{fileName}.asset");
      AssetDatabase.SaveAssets();      
      
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

   }

  

   #region Load Data for the Container
   private void ClearGraph()
   {
      //set Entry points guid back from the save . Discard existing guid.
      Nodes.Find(x => x.Entrybool).GUID = _ContainerCache.Nodelinks[0].BaseNodeGuid;

      foreach (var node in Nodes)
      {
         if(node.Entrybool) return;
         
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
         var tempNode = _targetGraphView.CreateDialogueNode(nodeData.DialogueText);
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
      throw new System.NotImplementedException();
   }
   
   #endregion

}
