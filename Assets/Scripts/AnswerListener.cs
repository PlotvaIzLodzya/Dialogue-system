using System;

namespace DialogueSystem
{
    public class AnswerListener
    {
        public event Action<int> AnswerChoosen;

        public void Trigger(int index)
        {
            AnswerChoosen?.Invoke(index);
        }
    }
}
