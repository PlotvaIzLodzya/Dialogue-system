using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    public class AnswerCreator : MonoBehaviour
    {
        [SerializeField] private AnswerView _answerView;
        [SerializeField] private Transform _answerContainer;

        private List<AnswerView> _previousAnswer = new List<AnswerView>();

        public void Create(Answer[] answers, AnswerListener answerListener)
        {
            DeletePreviousAnswer();

            for (int i = 0; i < answers.Length; i++)
            {
                var answerView = Instantiate(_answerView, _answerContainer);
                answers[i].Init(answerListener);
                answerView.Init(answers[i], i);
                _previousAnswer.Add(answerView);
            }
        }

        private void DeletePreviousAnswer()
        {
            foreach (var previousAnswer in _previousAnswer)
            {
                previousAnswer.Destroy();
            }

            _previousAnswer.Clear();
        }
    }
}

