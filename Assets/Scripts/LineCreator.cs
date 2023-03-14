using UnityEngine;

namespace DialogueSystem
{
    public class LineCreator: MonoBehaviour
    {
        [SerializeField] private DialogueLineView _dialogueLineView;
        [SerializeField] private Transform _dialogueLineContainer;

        public void Create(DialogueLine dialogueLine)
        {
            var answerView = Instantiate(_dialogueLineView, _dialogueLineContainer);
            answerView.SetText(dialogueLine);
        }
    }
}

