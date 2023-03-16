using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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
            var oldEdge = Edges.FirstOrDefault(cachedEdge => cachedEdge.OutputGUID == edge.OutputGUID && cachedEdge.ChildNodeGUID == edge.ChildNodeGUID);

            if(oldEdge != null)
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

            if(edge != null)
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
    }
}
