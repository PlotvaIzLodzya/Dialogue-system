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

        public DialogueNodeEditorData EditorData;

        public void CreateData()
        {
            EditorData = ScriptableObject.CreateInstance(nameof(DialogueNodeEditorData)) as DialogueNodeEditorData;
            EditorData.name = "EditorData";
            //Undo.RecordObject(this, "DialogueEditor (Create Dialogue Editor Data)");
            AssetDatabase.AddObjectToAsset(EditorData, this);
            Undo.RegisterCreatedObjectUndo(EditorData, "DialogueEditor (Create Dialogue Editor Data)");
            AssetDatabase.SaveAssets();

            EditorUtility.SetDirty(this);
        }

        public void RemoveData()
        {
            Undo.RecordObject(this, "DialogueEditor (Delete Dialogue Editor Data)");
            //AssetDatabase.RemoveObjectFromAsset(EditorData);
            Undo.DestroyObjectImmediate(EditorData);
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
