using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem
{
    [System.Serializable]
    public class DialogueNodeEditorData : ScriptableObject
    {
        public string GUID;
        public List<DialogueNode> Parents = new List<DialogueNode>();
        public List<DialogueNode> Childrens = new List<DialogueNode>();
        public List<EdgeData> Edges = new List<EdgeData>();
        public Vector2 EditorPosition;

        public void AddEdge(EdgeData edge)
        {
            Edges.Add(edge);
        }

        public bool TryGetEdgeData(string outputGUID, out EdgeData edgeData)
        {
            edgeData = Edges.FirstOrDefault(edge => edge.OutputGUID == outputGUID && edge.Connected == false);

            return edgeData != default;
        }
    }
}
