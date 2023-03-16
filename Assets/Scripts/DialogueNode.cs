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
        [SerializeField] private Answer[] _answers;

        [field: SerializeField] public DialogueLine DialogueLine { get; private set; }
        [field: SerializeField] public NodeID NodeID { get; private set; }
        [field: SerializeField] public DialogueNodeType Type { get; private set; }

        //For undo redo puprosese
        [HideInInspector] public string GUID;
        [HideInInspector] public List<DialogueNode> Parents = new List<DialogueNode>();
        [HideInInspector] public List<DialogueNode> Childrens = new List<DialogueNode>();
        [HideInInspector] public List<EdgeData> Edges = new List<EdgeData>();
        [HideInInspector] public Vector2 EditorPosition;

        public void AddEdge(EdgeData edge)
        {
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
        }

        public bool TryGetEdgeData(string outputGUID, out EdgeData edgeData)
        {
            edgeData = Edges.FirstOrDefault(edge => edge.OutputGUID == outputGUID && edge.Connected == false);

            return edgeData != default;
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
