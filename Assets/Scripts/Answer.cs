using System;
using UnityEngine;

namespace DialogueSystem
{
    [Serializable]
    public class Answer
    {
        [field: SerializeField] public DialogueLine DialogueLine { get; private set; }
        public AnswerListener AnswerListener { get; private set; }
        public string GUID;
        public string ChildGUID;

        public Answer(string outputGUID, string childGUID, DialogueLine dialogueLine)
        {
            DialogueLine = dialogueLine;
            GUID = outputGUID;
            ChildGUID = childGUID;
        }

        public Answer(string outputGUID, DialogueLine dialogueLine)
        {
            DialogueLine = dialogueLine;
            GUID = outputGUID;
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
