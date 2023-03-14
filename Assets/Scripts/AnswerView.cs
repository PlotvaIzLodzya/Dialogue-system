using System;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DialogueSystem
{
    public partial class AnswerView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TMP_Text _answerLine;

        private Answer _answer;
        private int _index;

        public event Action<int> AnswerChoosen;

        public void Init(Answer answer, int index)
        {
            _answer = answer;
            _index = index;
            SetText(answer.DialogueLine.Text, index);
        }

        public void SetText(string text, int index)
        {
            _answerLine.text = $"{index}. {text}";
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnAnswerChoosen();
        }

        private void OnAnswerChoosen()
        {
            _answer.Choose(_index);
        }
    }
}

