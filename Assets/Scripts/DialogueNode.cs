using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem
{
    [Serializable]
    public class DialogueNode : ScriptableObject
    {
        [SerializeField] private List<Answer> _answers = new List<Answer>();

        [field: SerializeField] public DialogueLine DialogueLine { get; private set; }
        [field: SerializeField] public NodeID NodeID { get; private set; }
        [field: SerializeField] public DialogueNodeType Type { get; private set; }

        //For undo redo puprosese
        public string GUID;
        public List<EdgeData> Edges = new List<EdgeData>();
        public Vector2 EditorPosition;

        public void SetLine(string text)
        {
            DialogueLine.SetText(text);
        }

        public void AddAnswer(string guid)
        {
            var line = new DialogueLine("");
            var answer = new Answer(guid, line);
   
            _answers.Add(answer);
        }

        public bool TryGetAnswer(string guid, out Answer answer)
        {
            answer = _answers.FirstOrDefault(answer => answer.GUID == guid);
            return answer != null;
        }

        public void SetAnswerText(string guid, string text)
        {
            Answer answer = _answers.First(cachedAnswer => cachedAnswer.GUID == guid);
            Debug.Log($"{guid} {text}");
            answer.DialogueLine.SetText(text);
        }

        public void DeleteAnswer(string guid)
        {
            var cachedAnswer = _answers.FirstOrDefault(answer => answer.GUID == guid);

            if(cachedAnswer!= default)
                _answers.Remove(cachedAnswer);
        }

        public void AddEdge(EdgeData edge)
        {
            if (edge == null)
                return;

            var oldEdge = Edges.FirstOrDefault(cachedEdge => cachedEdge.OutputGUID == edge.OutputGUID && cachedEdge.ChildNodeGUID == edge.ChildNodeGUID);

            if (oldEdge != null)
            {
                oldEdge.SetEdgeData(edge.ChildNodeGUID, edge.Edge);
            }
            else
            {
                Edges.Add(edge);
            }
            EditorUtility.SetDirty(this);
        }

        public void DeleteEdge(string outputGUID, string childGUID)
        {
            var edge = Edges.FirstOrDefault(edge => edge.OutputGUID == outputGUID && edge.ChildNodeGUID == childGUID);

            if (edge != null)
            {
                Edges.RemoveAll(edge => edge.OutputGUID == outputGUID && edge.Connected == false);
                edge.DeleteChildData();
            }
            EditorUtility.SetDirty(this);
        }

        public bool TryGetEdgeData(string outputGUID, out EdgeData edgeData)
        {
            edgeData = Edges.FirstOrDefault(edge => edge.OutputGUID == outputGUID && edge.Connected == false);

            return edgeData != default;
        }

        public Answer[] GetAnswers()
        {
            var answers = new List<Answer>();

            answers.AddRange(_answers);

            return answers.ToArray();
        }
    }
}
