using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Winndos
{
    using Elements;
    using Enumerations;
    using Utilities;
    
    public class DSGraphView : GraphView
    {
        private DSEditorWindow editorWindow;
        private DSSearchWindow searchWindow;
        public DSGraphView(DSEditorWindow dsEditorWindow)
        {
            editorWindow = dsEditorWindow;
            
            AddManipulators();
            AddSearchWindow();
            AddGridBackground();

            
            
            AddStyles();
            
        }

        #region Overrided Methods
   
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort ==port)
                {
                    return; 
                }
                if (startPort.node ==port.node)
                {
                    return; 
                }
                if (startPort.direction ==port.direction)
                {
                    return; 
                }
                compatiblePorts.Add(port);
            });
            
            return compatiblePorts;
        }

        #endregion

        #region Styles
        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0,gridBackground );
        }
        private void AddStyles()
        {
            this.AddStyleSheets(
                "DialogueSystem/DSGraphViewStyles.uss",
                "DialogueSystem/DSNodeStyles.uss"
                );
        }

        #endregion

        #region AddManipulators
        
        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
  
           
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new ContentDragger()); 
            this.AddManipulator(new RectangleSelector());
            
            
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Single Choice)", DSDialogueType.SingleChoice));
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Multiple Choice)", DSDialogueType.MultipleChoice));
            
            this.AddManipulator(CreateGroupContextualMenu());


        }

        private IManipulator CreateNodeContextualMenu(string actionTitle,DSDialogueType dialogueType)
        {
            ContextualMenuManipulator contextlMenuManipulartor = new ContextualMenuManipulator
            (
                menuEvet => menuEvet.menu.AppendAction(actionTitle, 
                    actionEvent => AddElement(CreateNode(dialogueType,GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))))
            );
            return contextlMenuManipulartor;
        }

        private IManipulator CreateGroupContextualMenu()
        {
            ContextualMenuManipulator contextlMenuManipulartor = new ContextualMenuManipulator(
            menuEvet => menuEvet.menu.AppendAction("Add Group", 
                    actionEvent => AddElement(CreateGroup("DialogueGroup",GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))))
                );
            return contextlMenuManipulartor;
        }
        
        
        #endregion

        #region Element Addition
        
        private void AddSearchWindow()
        {
            if (searchWindow == null)
            {
                searchWindow = ScriptableObject.CreateInstance<DSSearchWindow>();
                searchWindow.Initialize(this);
            }
            //change value name is context
            nodeCreationRequest = context => SearchWindow.Open(new 
                SearchWindowContext(context.screenMousePosition),searchWindow);
        }
        
        
        
        public DSNode CreateNode(DSDialogueType dialogueType, Vector2 position)
        {
            
            Type nodeType = Type.GetType($"DS.Elements.DS{dialogueType}Node");
            DSNode node = (DSNode) Activator.CreateInstance(nodeType);
            
            node.Initialize(position);
            node.Draw();
            
            return node;
        }

        public Group CreateGroup(String title, Vector2 position)
        {
            Group group = new Group
            {
                title = title
            };
            group.SetPosition(new Rect(position,Vector2.zero));
            
            return group;
        }
        
        #endregion

        #region Utilities

        public Vector2 GetLocalMousePosition(Vector2 mousePosition,bool isSearchWindow = false)
        {
            
            //位置换算
            Vector2 worldMousePosition = editorWindow.rootVisualElement.ChangeCoordinatesTo(
                editorWindow.rootVisualElement.parent,
                mousePosition - editorWindow.position.position);

            if (!isSearchWindow)
            {
                worldMousePosition = mousePosition;
            }
         
            Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);
            
            return localMousePosition;
            
        }

        #endregion
    }
}