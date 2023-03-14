using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialogueView: MonoBehaviour
    {
        [SerializeField] private AnswerCreator _answerCreator;
        [SerializeField] private LineCreator _lineCreator;
        [SerializeField] private ScrollRect _scrollRect;

        private DialogueAnimator _dialogueAnimator;

        private void Awake()
        {
            _dialogueAnimator = new DialogueAnimator(_scrollRect);
        }

        public void Create(DialogueNode node, AnswerListener answerListener)
        {
            _dialogueAnimator.Scroll(1f, 0);
            _answerCreator.Create(node.GetAnswers(), answerListener);
            _lineCreator.Create(node.DialogueLine);
        }
    }
}
