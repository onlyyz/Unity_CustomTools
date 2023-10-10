using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Elements
{
    using Enumerations;
    using Utilities;
    public class DSNode : Node
    {
        public string DialogueName { get; set; }
        public List<string> Choices { get; set; }
        public string Text { get; set; }
        public DSDialogueType DialogueType { get; set; }

        public virtual void Initialize(Vector2 position)
        {
            DialogueName = "DialogueName";
            Choices = new List<string>();
            Text = "Dialogue text.";
            SetPosition(new Rect(position,Vector2.zero));
            
            extensionContainer.AddToClassList("ds-node_extension-container");
        }

        public virtual void Draw()
        {

            TextField dialogueNameTextField = DSElementUtility.CreateTextField(DialogueName);
            
            dialogueNameTextField.AddToClassList("ds-node_textfield");
            dialogueNameTextField.AddToClassList("ds-node_filename-textfield");
            dialogueNameTextField.AddToClassList("ds-node_textfield");
            titleContainer.Insert(0,dialogueNameTextField);
            
            /* INPUT PORT */
            Port inputPort = this.CreatePort("Dialogue Connection",Orientation.Horizontal, Direction.Input,Port.Capacity.Multi);
          
            inputContainer.Add(inputPort);

            /* EXTENSION CONTAINER */
            VisualElement customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("ds-node_custom-data-container");

            Foldout textFoldout = DSElementUtility.CreateFoldout("Dialogue Text");
            TextField textFile = DSElementUtility.CreateTextArea(DialogueName);
            
            textFile.AddToClassList("ds-node_textfield");
            textFile.AddToClassList("ds-node_quote-textfield");
            
            textFoldout.Add(textFile);
            customDataContainer.Add(textFoldout);
            extensionContainer.Add(customDataContainer);
        }
    } 
}
