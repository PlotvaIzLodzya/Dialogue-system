using System;
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
        public Func<string, EditorDialogueNodeView> FindEdtorDialogueNodeView;

        public DialogueNode GetDialogueNode(NodeID nodeID)
        {
            return DialogueNodes.FirstOrDefault(node => node.NodeID.Key == nodeID.Key);
        }

        public void AddChild(DialogueNode parent, DialogueNode child, string outputGUID, Edge edge)
        {
            //parent.EditorData.Childrens.Add(child);
            //child.EditorData.Parents.Add(parent);
            Undo.RecordObject(parent, "DialogueEditor (AddChild)");
            EdgeData edgeData;

            if (parent.TryGetEdgeData(outputGUID, out edgeData))
            {
                edgeData.SetEdgeData(child.GUID, edge);
            }
            else
            {
                edgeData = new EdgeData(outputGUID, parent.GUID, child.GUID);
                parent.AddEdge(edgeData);
            }
            edgeData.Connected = true;

            EditorUtility.SetDirty(parent);
        }

        public void RemoveChild(DialogueNode parent, DialogueNode child, string outputName)
        {
            Undo.RecordObject(parent, "DialogueEditor (RemoveChild)");
            //parent.EditorData.Childrens.Remove(child);
            //child.EditorData.Parents.Remove(parent);
            parent.DeleteEdge(outputName, child.GUID);
            EditorUtility.SetDirty(parent);
        }

        public List<DialogueNode> GetChildren(DialogueNode parent)
        {
            return parent.Childrens; 
        }

        public DialogueNode CreateDialogueNode(int index, Vector2 position)
        {
            DialogueNode nodeInstance = ScriptableObject.CreateInstance(nameof(DialogueNode)) as DialogueNode;
            nodeInstance.name = "DialogueNode";
            nodeInstance.NodeID.AddIndex(index);
            DialogueNodes.Add(nodeInstance);
            Undo.RecordObject(this, "DialogueEditor (Create Dialogue Node)");
            AssetDatabase.AddObjectToAsset(nodeInstance, this);
            nodeInstance.GUID = GUID.Generate().ToString();
            Undo.RegisterCreatedObjectUndo(nodeInstance, "DialogueEditor (Create Dialogue Node)");
            AssetDatabase.SaveAssets();

            //EditorUtility.SetDirty(this);
            return nodeInstance;
        }

        public void DeleteDialogueNode(DialogueNode dialogueNode)
        {
            Undo.RecordObject(this, "DialogueEditor (Delete Dialogue Node)");
            DialogueNodes.Remove(dialogueNode);
            Undo.DestroyObjectImmediate(dialogueNode);
            AssetDatabase.SaveAssets();
        }
    }
}
