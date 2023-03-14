using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem
{
    [System.Serializable]
    public class DialogueNodeEditorData
    {
        public string GUID;
        public List<OutputPortsData> OutputPortsDatas= new List<OutputPortsData>();
        public List<DialogueNode> Parents = new List<DialogueNode>();
        public List<DialogueNode> Childrens = new List<DialogueNode>();
        public List<EdgeData> Edges = new List<EdgeData>();
        public Vector2 EditorPosition;

        public void AddEdge(EdgeData edge)
        {
            if (Edges.Any(cachedEdge => cachedEdge.GUIDOutput == edge.GUIDOutput))
                return;

            Edges.Add(edge);
        }
    }
}
