using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    [Serializable]
    public class DialogueNode:ScriptableObject
    {
        [SerializeField] private Answer[] _answers;

        [field: SerializeField] public DialogueLine DialogueLine { get; private set; }

        [field: SerializeField] public NodeID NodeID { get; private set; }
        [field: SerializeField] public DialogueNodeType Type { get; private set; }

        /*[HideInInspector]*/ public DialogueNodeEditorData DialogueNodeEditorData = new DialogueNodeEditorData();

        public Answer[] GetAnswers()
        {
            var answers = new List<Answer>();

            if (_answers.Length < 1)
            {
                answers.Add(new Answer(new DialogueLine("Далее")));
            }

            answers.AddRange(_answers);

            return answers.ToArray();
        }
    }
}
