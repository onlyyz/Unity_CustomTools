using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Elements
{
    using Enumerations;
    using Utilities;
    
    public class DSMultipleChoiceNode : DSNode
    {
        public override void Initialize(Vector2 position)
        {
            base.Initialize(position);
            DialogueType = DSDialogueType.MultipleChoice;
            Choices.Add("New Choice");
        }

       
        public override void Draw() 
        {
            base.Draw();

            /* MAIN CONTAINER */
            Button addChoiceButton = DSElementUtility.CreateButton("Add Choice", () =>
            {
                Port choicePort = CreateChoicePort("New Choice");
                
                Choices.Add("New Choice");
                outputContainer.Add(choicePort);
            });
            addChoiceButton.AddToClassList("ds-node_button");
            mainContainer.Insert(1,addChoiceButton);
            
            /* OUTPUT PORT */
            foreach (var choice in Choices)
            {
                Port choicePort = CreateChoicePort(choice);
                outputContainer.Add(choicePort);
            }
            RefreshExpandedState();
        }

        #region Elements  Creation

        private Port CreateChoicePort(string choice)
        {
            Port choicePort = this.CreatePort();

            Button deleteChoiceButton = DSElementUtility.CreateButton("X");
            deleteChoiceButton.AddToClassList("ds-node_button");

            TextField choiceTextField = DSElementUtility.CreateTextField(choice);
            choiceTextField.AddClasses
            (
                "ds-node_textfield",
                "ds-node_choice-textfield",
                "ds-node_textfield_hidden"
            );
                
            choiceTextField.style.flexDirection = FlexDirection.Column;
                
            choicePort.Add(choiceTextField);
            choicePort.Add(deleteChoiceButton);
           
            return choicePort;
        }
        
        #endregion
    }
}