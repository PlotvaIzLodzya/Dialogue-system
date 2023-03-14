using System;
using UnityEngine;

namespace DialogueSystem
{
    [Serializable]
    public class Answer
    {
        [field: SerializeField] public DialogueLine DialogueLine { get; private set; }
        public AnswerListener AnswerListener { get; private set; }

        public Answer(DialogueLine dialogueLine)
        {
            DialogueLine = dialogueLine;
        }

        public void Init(AnswerListener answerListener)
        {
            AnswerListener = answerListener;
        }

        public void Choose(int index)
        {
            AnswerListener.Trigger(index);
        }
    }
}
