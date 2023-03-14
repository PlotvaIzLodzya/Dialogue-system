using UnityEditor.Experimental.GraphView;

namespace DialogueSystem
{
    public class EdgeData
    {
        public string PortGUID;
        public string GUIDInput;
        public string GUIDOutput;

        public EdgeData(string input, string output)
        {
            GUIDInput = input;
            GUIDOutput = output;
        }

        public EdgeData(string output)
        {
            GUIDOutput = output;
        }
    }
}
