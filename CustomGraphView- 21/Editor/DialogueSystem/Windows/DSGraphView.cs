using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Winndos
{
    using Elements;
    public class DSGraphView : GraphView
    {
        public DSGraphView()
        {
            AddManipulators();
            AddGridBackground();

           
            AddStyles();
            
        }

        public DSNode CreateNode(Vector2 position)
        {
            DSNode node = new DSNode();
            
            node.Initialize(position);
            node.Draw();
            
            // AddElement(node);
            return node;
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
            styleSheets.Add(styleSheet);
        }

        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
  
            //增加控制器
            this.AddManipulator(CreateNodeContextualMenu());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new ContentDragger()); 
            this.AddManipulator(new RectangleSelector());
        }

        private IManipulator CreateNodeContextualMenu()
        {
            ContextualMenuManipulator contextlMenuManipulartor = new ContextualMenuManipulator
            (
                menuEvet => menuEvet.menu.AppendAction("Add Node", 
                    actionEvent => AddElement(CreateNode(actionEvent.eventInfo.localMousePosition)))
            );
            return contextlMenuManipulartor;
        }
        #endregion
    }
}
