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
        public DSGraphView()
        {
            AddManipulators();
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
                    actionEvent => AddElement(CreateNode(dialogueType,actionEvent.eventInfo.localMousePosition)))
            );
            return contextlMenuManipulartor;
        }

        private IManipulator CreateGroupContextualMenu()
        {
            ContextualMenuManipulator contextlMenuManipulartor = new ContextualMenuManipulator(
            menuEvet => menuEvet.menu.AppendAction("Add Group", 
                    actionEvent => AddElement(CreateGroup("DialogueGroup",actionEvent.eventInfo.localMousePosition)))
                );
            return contextlMenuManipulartor;
        }
        
        
        #endregion

        #region Element Addition
        
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
    }
}