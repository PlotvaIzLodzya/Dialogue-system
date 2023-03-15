using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "DialogueData", menuName = "ScriptableObject/Dialogues")]
    public class DialogueData: ScriptableObject
    {
        public List<DialogueNode> DialogueNodes = new List<DialogueNode>();

        public DialogueNode GetDialogueNode(NodeID nodeID)
        {
            return DialogueNodes.FirstOrDefault(node => node.NodeID.Key == nodeID.Key);
        }

        public void AddChild(DialogueNode parent, DialogueNode child, string outputGUID)
        {
            parent.EditorData.Childrens.Add(child);
            child.EditorData.Parents.Add(parent);
            EdgeData edgeData;

            if (parent.EditorData.TryGetEdgeData(outputGUID, out edgeData))
            {
                edgeData.SetChildData(child.EditorData.GUID);
            }
            else
            {
                edgeData = new EdgeData(outputGUID, parent.EditorData.GUID, child.EditorData.GUID);
                parent.EditorData.AddEdge(edgeData);
            }

            edgeData.Connected = true;
        }

        public void RemoveChild(DialogueNode parent, DialogueNode child)
        {
            parent.EditorData.Childrens.Remove(child);
            child.EditorData.Parents.Remove(parent);
        }

        public List<DialogueNode> GetChildren(DialogueNode parent)
        {
            return parent.EditorData.Childrens; 
        }

        public DialogueNode CreateDialogueNode(int index, Vector2 position)
        {
            DialogueNode nodeInstance = ScriptableObject.CreateInstance(nameof(DialogueNode)) as DialogueNode;
            nodeInstance.name = "DialogueNode";
            nodeInstance.NodeID.AddIndex(index);
            DialogueNodes.Add(nodeInstance);
            AssetDatabase.AddObjectToAsset(nodeInstance, this);
            AssetDatabase.SaveAssets();

            nodeInstance.Init();
            nodeInstance.EditorData.GUID = GUID.Generate().ToString();
            EditorUtility.SetDirty(this);

            return nodeInstance;
        }

        public void DeleteDialogueNode(DialogueNode dialogueNode)
        {
            DialogueNodes.Remove(dialogueNode);
            AssetDatabase.RemoveObjectFromAsset(dialogueNode);
            AssetDatabase.SaveAssets();
        }
    }
}
