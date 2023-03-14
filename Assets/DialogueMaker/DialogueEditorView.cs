using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using DialogueSystem;
using System;
using System.Linq;
using UnityEngine;
using Unity.VisualScripting;
using Edge = UnityEditor.Experimental.GraphView.Edge;

public partial class DialogueEditorView : GraphView
{
    public new class UxmlFactory : UxmlFactory<DialogueEditorView, GraphView.UxmlTraits> { }
    private DialogueData _dialogueData;

    public event Action<EditorDialogueNodeView> OnNodeSelected;
    public event Action OnNodeUnselected;

    public DialogueEditorView()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/DialogueMaker/DialogueEditor.uss");
        styleSheets.Add(styleSheet);
    }


    internal void PoppulateView(DialogueData dialogueData)
    {
        _dialogueData = dialogueData;
        graphViewChanged -= OnGraphViewChange;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChange;

        dialogueData.DialogueNodes.ForEach(node => CreateDialogueNodeView(node, node.DialogueNodeEditorData.EditorPosition));

        dialogueData.DialogueNodes.ForEach(node =>
        {
            node.DialogueNodeEditorData.Edges.ForEach(edgeData =>
            {
                EditorDialogueNodeView parentDialogueNodeView = FindEditorDialogueNodeView(edgeData.GUIDOutput);
                EditorDialogueNodeView childDialogueNodeView = FindEditorDialogueNodeView(edgeData.GUIDInput);

                Edge edge = new Edge();
                //edge.output = parentDialogueNodeView.DialogueNode.ou
                AddElement(edge);
            });
            //node.DialogueNodeEditorData.Childrens.ForEach(child =>
            //{
            //    EditorDialogueNodeView parentDialogueNodeView = FindEditorDialogueNodeView(node);
            //    EditorDialogueNodeView childDialogueNodeView = FindEditorDialogueNodeView(child);

            //    parentDialogueNodeView.Outputs.ForEach(output =>
            //    {
            //        Edge edge = output.ConnectTo(childDialogueNodeView.Input);
            //        AddElement(edge);
            //    });
            //});
        });
    }

    private EditorDialogueNodeView FindEditorDialogueNodeView(string dialogueNode)
    {
        return GetNodeByGuid(dialogueNode) as EditorDialogueNodeView;
    }

    private GraphViewChange OnGraphViewChange(GraphViewChange graphViewChange)
    {
        if(graphViewChange.elementsToRemove != null)
        {
            graphViewChange.elementsToRemove.ForEach(element =>
            {
                EditorDialogueNodeView dialogueNodeview = element as EditorDialogueNodeView;
                if (dialogueNodeview != null)
                {
                    _dialogueData.DeleteDialogueNode(dialogueNodeview.DialogueNode);
                }

                Edge edge = element as Edge;
                if(edge != null)
                {
                    EditorDialogueNodeView parentView = edge.output.node as EditorDialogueNodeView;
                    EditorDialogueNodeView childView = edge.input.node as EditorDialogueNodeView;
                    _dialogueData.RemoveChild(parentView.DialogueNode, childView.DialogueNode);
                }
            });
        }

        if(graphViewChange.edgesToCreate != null)
        {
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                EditorDialogueNodeView parentView = edge.output.node as EditorDialogueNodeView;
                EditorDialogueNodeView childView = edge.input.node as EditorDialogueNodeView;
                _dialogueData.AddChild(parentView.DialogueNode, childView.DialogueNode);
            });
        }

        return graphViewChange;
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node && startPort.connections.Any(edge => edge.input == endPort) == false).ToList();
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        //base.BuildContextualMenu(evt);
        Vector2 worldMousePosition = evt.mousePosition;
        Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);
        evt.menu.AppendAction($"[{nameof(DialogueNode)}]", (action) => CreateDialogueNode(localMousePosition));
    }

    private void CreateDialogueNode(Vector2 position)
    {
       DialogueNode dialogueNode = _dialogueData.CreateDialogueNode(0, position);

       CreateDialogueNodeView(dialogueNode, position);

    }

    private void CreateDialogueNodeView(DialogueNode dialogueNode, Vector2 position)
    {
        Rect rect = new Rect();
        rect.position = position;
        EditorDialogueNodeView editorDialogueNodeView = new EditorDialogueNodeView(dialogueNode, rect);
        editorDialogueNodeView.OnNodeSelected = OnNodeSelected;
        editorDialogueNodeView.OnNodeUnselected = OnNodeUnselected; 
        AddElement(editorDialogueNodeView);
    }
}
