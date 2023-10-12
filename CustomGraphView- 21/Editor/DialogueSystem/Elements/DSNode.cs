using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Elements
{
    using Enumerations;
    using Utilities;
    using Winndos;
    public class DSNode : Node
    {
        public string DialogueName { get; set; }
        public List<string> Choices { get; set; }
        public string Text { get; set; }
        public DSDialogueType DialogueType { get; set; }
        public DSGroup Group { get; set; }

        private DSGraphView graphView;
        
        
        
        private Color defaultBackgroundColor;

        public virtual void Initialize(DSGraphView DSGraphView, Vector2 position)
        {
            this.graphView = DSGraphView;
            defaultBackgroundColor = new Color(29/255, 29/255, 30/255);
            
            
            DialogueName = "DialogueName";
            Choices = new List<string>();
            Text = "Dialogue text.";
            SetPosition(new Rect(position,Vector2.zero));
            
            extensionContainer.AddToClassList("ds-node_extension-container");
        }

        public virtual void Draw()
        {

            TextField dialogueNameTextField = DSElementUtility.CreateTextField(DialogueName,
                callback =>
                {
                    if (Group==null)
                    {
                        graphView.RemoveUngroundedNode(this);
                        DialogueName = callback.newValue;
                        graphView.AddUngroupedNode(this);
                        return;
                    }

                    DSGroup currenGroup = Group;
                    
                    graphView.RemoveGroupedNode(this,Group);
                    DialogueName = callback.newValue;
                    graphView.AddGroupedNode(this,currenGroup);
                    
                });

            dialogueNameTextField.AddClasses
                (
                    "ds-node_textfield",
                    "ds-node_filename-textfield",
                    "ds-node_textfield"
                );
            
            titleContainer.Insert(0,dialogueNameTextField);
            
            /* INPUT PORT */
            Port inputPort = this.CreatePort("Dialogue Connection",Orientation.Horizontal, Direction.Input,Port.Capacity.Multi);
          
            inputContainer.Add(inputPort);

            /* EXTENSION CONTAINER */
            VisualElement customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("ds-node_custom-data-container");

            Foldout textFoldout = DSElementUtility.CreateFoldout("Dialogue Text");
            TextField textFile = DSElementUtility.CreateTextArea(DialogueName);
            
            
            textFile.AddClasses
            (
                "ds-node_textfield",
                "ds-node_quote-textfield"
            );
            
            textFoldout.Add(textFile);
            customDataContainer.Add(textFoldout);
            extensionContainer.Add(customDataContainer);
        }

        #region Style
        
        public void SetErrorStyle(Color color)
        {
            mainContainer.style.backgroundColor = color;
        }


        public void ResetStyle()
        {
            mainContainer.style.backgroundColor = defaultBackgroundColor;
        }
        
        #endregion
    } 
}
