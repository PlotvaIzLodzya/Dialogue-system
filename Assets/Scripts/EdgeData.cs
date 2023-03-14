using UnityEditor.Experimental.GraphView;

namespace DialogueSystem
{
    [System.Serializable]
    public class EdgeData
    {
        public string ParentNodeGUID { get; private set; } = "Not setted";
        public string ChildNodeGUID { get; private set; } = "Not setted";
        public string InputGUID { get; private set; } = "Not setted";
        public string OutputGUID { get; private set; } = "Not setted";

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
