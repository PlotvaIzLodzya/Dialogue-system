using System;
using UnityEngine;

namespace DialogueSystem
{
    [Serializable]
    public struct NodeID
    {
        [field: SerializeField] public string Key { get; private set; }

        public NodeID(string key) => Key = key;

        public void AddIndex(int index)
        {
            Key = $"{Key}{index}";
        }
    }
}

