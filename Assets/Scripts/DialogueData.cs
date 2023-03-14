using System.Collections.Generic;
using System.Linq;
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

        public void AddChild(DialogueNode parent, DialogueNode child)
        {
            parent.DialogueNodeEditorData.Childrens.Add(child);
            child.DialogueNodeEditorData.Parents.Add(parent);
            parent.DialogueNodeEditorData.AddEdge(new EdgeData(child.DialogueNodeEditorData.GUID, parent.DialogueNodeEditorData.GUID));
        }

        public void RemoveChild(DialogueNode parent, DialogueNode child)
        {
            parent.DialogueNodeEditorData.Childrens.Remove(child);
            child.DialogueNodeEditorData.Parents.Remove(parent);
        }

        public List<DialogueNode> GetChildren(DialogueNode parent)
        {
            return parent.DialogueNodeEditorData.Childrens; 
        }

        public DialogueNode CreateDialogueNode(int index, Vector2 position)
        {
            DialogueNode nodeInstance = ScriptableObject.CreateInstance(nameof(DialogueNode)) as DialogueNode;
            nodeInstance.name = "DialogueNode";
            nodeInstance.NodeID.AddIndex(index);
            nodeInstance.DialogueNodeEditorData.GUID = GUID.Generate().ToString();
            DialogueNodes.Add(nodeInstance);
            AssetDatabase.AddObjectToAsset(nodeInstance, this);
            AssetDatabase.SaveAssets();
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
