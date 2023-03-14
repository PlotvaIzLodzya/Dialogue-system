using UnityEditor;
using UnityEngine;

namespace DialogueSystem
{
    public class Dialogue: MonoBehaviour
    {
        [SerializeField] private DialogueData _dialogueData;
        [SerializeField] private DialogueView _dialogueView;

        private NodeID _nodeID = new NodeID("0");
        private DialogueNode _currentDialogueNode;
        private AnswerListener _answerListener = new AnswerListener();

        private void Start()
        {
            GetNextDialogueNode(_nodeID);
            _answerListener.AnswerChoosen += OnAnswerChoosen;
        }

        public void OnAnswerChoosen(int index)
        {
            if(_currentDialogueNode.Type == DialogueNodeType.End)
            {
                EndDialogue();
                return;
            }    

            _nodeID.AddIndex(index);
            GetNextDialogueNode(_nodeID);
        }

        public void GetNextDialogueNode(NodeID nodeID)
        {
            var node = _dialogueData.GetDialogueNode(nodeID);
            _currentDialogueNode = node;

            _dialogueView.Create(node, _answerListener);
        }

        private void EndDialogue()
        {
            print("End");
        }
    }
}
