using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

namespace MU5Editor.NodeEditor
{
    public class GraphWindow : EditorWindow
    {
        MyGraphView graphView;
        ObjectField objectField;
        public ScenarioData scenarioData { get { return (ScenarioData)objectField.value; } }
        //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー 
        [MenuItem("Tool/22-ScriptGraph")]
        public static void Open()
        {
            GetWindow<GraphWindow>("Node Editor");
        }
        //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
        void OnEnable()
        {
            //工具栏
            Toolbar toolbar = new Toolbar();
            toolbar.style.flexDirection = FlexDirection.Row;
            
            //文件路径
            objectField = new ObjectField();
            objectField.objectType = typeof(ScenarioData);
            Button loadBtn = new Button(LoadData) { text = "加載" };
            Button saveBtn = new Button(SaveData) { text = "保存" };
            
            
            toolbar.Add(objectField);
            toolbar.Add(loadBtn);
            toolbar.Add(saveBtn);
            rootVisualElement.Add(toolbar);

            
            
            graphView = new MyGraphView(this)
            {
                style = { flexGrow = 1 }
            };
            rootVisualElement.Add(graphView);
        }

        void LoadData()
        {
            if (scenarioData == null) return;

            graphView.DeleteAllElements();

            foreach (var nodeData in scenarioData.nodeData_list)
            {
                graphView.LoadNodeData(nodeData);
            }
            foreach (var edgeData in scenarioData.edgeData_list)
            {
                graphView.LoadEdgeData(edgeData);
            }

            Debug.Log($"ロード完了");
        }

        void SaveData()
        {
            if (scenarioData == null) return;

            scenarioData.nodeData_list.Clear();
            scenarioData.edgeData_list.Clear();

            foreach (var graphElement in graphView.graphElements)
            {
                if (graphElement is MU5Node) SaveData_Node(graphElement);
                else if (graphElement is Edge) SaveData_Edge(graphElement);
                else Debug.LogWarning($"Find a non-surported graphElement type: {graphElement.GetType()}");
            }

            EditorUtility.SetDirty(objectField.value);
            AssetDatabase.SaveAssets();

            Debug.Log($"保存完了");
        }

        void SaveData_Node(GraphElement _graphElement)
        {
            MU5Node node = _graphElement as MU5Node;
            NodeData nodeData = new NodeData()
            {
                uid = node.uid,
                nodeType_str = node.GetType().ToString(),
                localBound = node.localBound
            };
            scenarioData.nodeData_list.Add(nodeData);
        }

        void SaveData_Edge(GraphElement _graphElement)
        {
            Edge edge = _graphElement as Edge;

            Port inputPort = edge.input;
            Port outputPort = edge.output;
            MU5Node inputNode = edge.input.node as MU5Node;
            MU5Node outputNode = edge.output.node as MU5Node;
            string uid_inputPort_target = inputNode.port_dict.FirstOrDefault(x => x.Value.Equals(inputPort)).Key;
            string uid_outputPort_target = outputNode.port_dict.FirstOrDefault(x => x.Value.Equals(outputPort)).Key;

            EdgeData edgeData = new EdgeData()
            {
                uid_outputNode = outputNode.uid,
                uid_outputPort = uid_outputPort_target,
                uid_inputNode = inputNode.uid,
                uid_inputPort = uid_inputPort_target
            };
            scenarioData.edgeData_list.Add(edgeData);
        }
    }
}