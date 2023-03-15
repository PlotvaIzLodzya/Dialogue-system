using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem
{
    [Serializable]
    public class DialogueNode : ScriptableObject
    {
        [SerializeField] private Answer[] _answers;

        [field: SerializeField] public DialogueLine DialogueLine { get; private set; }
        [field: SerializeField] public NodeID NodeID { get; private set; }
        [field: SerializeField] public DialogueNodeType Type { get; private set; }

        [HideInInspector] public DialogueNodeEditorData EditorData;

        public void Init()
        {
            EditorData = ScriptableObject.CreateInstance(nameof(DialogueNodeEditorData)) as DialogueNodeEditorData;
            EditorData.name = "EditorData";
            AssetDatabase.AddObjectToAsset(EditorData, this);
            AssetDatabase.SaveAssets();

            EditorUtility.SetDirty(this);
        }

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
