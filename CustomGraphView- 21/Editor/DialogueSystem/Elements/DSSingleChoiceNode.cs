using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DS.Elements
{
    using Enumerations;
    using Utilities;
    using Winndos;
    public class DSSingleChoiceNode : DSNode
    {
        public override void Initialize(DSGraphView DSGraphView, Vector2 position)
        {
            base.Initialize(DSGraphView,position);
            DialogueType = DSDialogueType.SingleChoice;
            Choices.Add("Next Dialogue");
        }

        /* OUTPUT PORT */
        public override void Draw()
        {
            base.Draw();
            foreach (var choice in Choices)
            {
                Port choicePort = this.CreatePort(choice);
                choicePort.portName = choice;
                outputContainer.Add(choicePort);
            }
            RefreshExpandedState();
        }
    }
}

