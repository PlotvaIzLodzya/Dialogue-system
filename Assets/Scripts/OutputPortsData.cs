using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

namespace DialogueSystem
{
    [Serializable]
    public class OutputPortsData
    {
        public Port Output;
        public Port Input;
        public string GUID;

        public OutputPortsData(Port output, string guid)
        {
            Output = output;
            GUID = guid;
        }
    }
}
