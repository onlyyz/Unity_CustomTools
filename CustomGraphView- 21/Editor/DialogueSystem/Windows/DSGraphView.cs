using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Winndos
{
    using Elements;
    using Enumerations;
    public class DSGraphView : GraphView
    {
        public DSGraphView()
        {
            AddManipulators();
            AddGridBackground();

           
            AddStyles();
            
        }

        


        
        #region Styles
        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0,gridBackground );
        }
        private void AddStyles()
        {
            StyleSheet styleSheet = (StyleSheet) EditorGUIUtility.Load("DialogueSystem/DSGraphViewStyles.uss");
            StyleSheet nodeStyleSheet = (StyleSheet) EditorGUIUtility.Load("DialogueSystem/DSNodeStyles.uss");
            styleSheets.Add(styleSheet);
            styleSheets.Add(nodeStyleSheet);
        }

        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
  
           
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new ContentDragger()); 
            this.AddManipulator(new RectangleSelector());
            
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Single Choice)", DSDialogueType.SingleChoice));
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Multiple Choice)", DSDialogueType.MultipleChoice));
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
        #endregion

        #region Node
        
        public DSNode CreateNode(DSDialogueType dialogueType, Vector2 position)
        {
            
            Type nodeType = Type.GetType($"DS.Elements.DS{dialogueType}Node");
            DSNode node = (DSNode) Activator.CreateInstance(nodeType);
            
            node.Initialize(position);
            node.Draw();
            
            return node;
        } 
        
        #endregion
    }
}
