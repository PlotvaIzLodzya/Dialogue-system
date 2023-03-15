using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem
{
    [System.Serializable]
    public class EdgeData
    {
        [field:SerializeField] public string ParentNodeGUID { get; private set; } = "Not setted";
        [field:SerializeField] public string ChildNodeGUID { get; private set; } = "Not setted";
        [field:SerializeField] public string InputGUID { get; private set; } = "Not setted";
        [field:SerializeField] public string OutputGUID { get; private set; } = "Not setted";

        public bool Connected;

        public EdgeData(string outputGUID, string nodeGUID, string childGUID)
        {
            OutputGUID = outputGUID;
            ParentNodeGUID = nodeGUID;
            ChildNodeGUID = childGUID;
        }

        public EdgeData(string outputGUID, string nodeGUID)
        {
            OutputGUID = outputGUID;
            ParentNodeGUID = nodeGUID;
        }

        public void SetChildData(string childGUID)
        {
            ChildNodeGUID = childGUID;
        }
    }
}
