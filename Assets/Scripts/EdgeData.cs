using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem
{
    [System.Serializable]
    public class EdgeData
    {
        [field:SerializeField] public string DataGUID { get; private set; }
        [field:SerializeField] public string ParentNodeGUID { get; private set; } = "Not setted";
        [field:SerializeField] public string ChildNodeGUID { get; private set; } = "Not setted";
        [field:SerializeField] public string InputGUID { get; private set; } = "Not setted";
        [field:SerializeField] public string OutputGUID { get; private set; } = "Not setted";
        [field: SerializeField] public Edge Edge { get; private set; }

        public bool Connected;

        public EdgeData(string outputGUID, string nodeGUID, string childGUID)
        {
            OutputGUID = outputGUID;
            ParentNodeGUID = nodeGUID;
            ChildNodeGUID = childGUID;
            DataGUID = GUID.Generate().ToString();
        }

        public EdgeData(string outputGUID, string nodeGUID)
        {
            OutputGUID = outputGUID;
            ParentNodeGUID = nodeGUID;
            DataGUID = GUID.Generate().ToString();
        }

        public void SetEdgeData(string childGUID, Edge edge)
        {
            ChildNodeGUID = childGUID;
            Edge = edge;
        }

        public void DeleteChildData()
        {
            ChildNodeGUID = "Not setted";
            Edge = null;
            Connected = false;
        }

        public void Delete()
        {
            if(Edge!= null)
                Edge.RemoveFromHierarchy();
        }
    }
}
