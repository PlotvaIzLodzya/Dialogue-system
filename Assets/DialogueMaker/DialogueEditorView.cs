using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using DialogueSystem;
using System;
using System.Linq;
using UnityEngine;
using Edge = UnityEditor.Experimental.GraphView.Edge;

public partial class DialogueEditorView : GraphView
{
    public new class UxmlFactory : UxmlFactory<DialogueEditorView, GraphView.UxmlTraits> { }
    private DialogueData _dialogueData;

    public event Action<EditorDialogueNodeView> OnNodeSelected;
    public event Action OnNodeUnselected;
    public event Action OnEdgeDeleted;

    public DialogueEditorView()
    {
        Insert(0, new GridBackground());
        //_dialogueData.FindEdtorDialogueNodeView = FindEditorDialogueNodeView;
        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/DialogueMaker/DialogueEditor.uss");
        styleSheets.Add(styleSheet);

        Undo.undoRedoPerformed += OnUndoRedo;
    }

    private void OnUndoRedo()
    {
        PoppulateView(_dialogueData);   
        AssetDatabase.SaveAssets();
    }

    internal void PoppulateView(DialogueData dialogueData)
    {
        _dialogueData = dialogueData;
        graphViewChanged -= OnGraphViewChange;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChange;

        dialogueData.DialogueNodes.ForEach(node => CreateDialogueNodeView(node, node.EditorData.EditorPosition));

        dialogueData.DialogueNodes.ForEach(node =>
        {
            node.EditorData.Edges.ForEach(edge =>
            {
                EditorDialogueNodeView parentDialogueNodeView = FindEditorDialogueNodeView(edge.ParentNodeGUID);
                EditorDialogueNodeView childDialogueNodeView = FindEditorDialogueNodeView(edge.ChildNodeGUID);

                if (childDialogueNodeView != null)
                {
                    parentDialogueNodeView.Outputs.ForEach(output =>
                    {
                        if (output.name == edge.OutputGUID)
                        {
                            Edge edge = output.ConnectTo(childDialogueNodeView.Input);
                            AddElement(edge);
                        }
                    });
                }

            });
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
                    _dialogueData.RemoveChild(parentView.DialogueNode, childView.DialogueNode, edge.output.name);
                }
            });
        }

        if(graphViewChange.edgesToCreate != null)
        {
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                EditorDialogueNodeView parentView = edge.output.node as EditorDialogueNodeView;
                EditorDialogueNodeView childView = edge.input.node as EditorDialogueNodeView;
                
                _dialogueData.AddChild(parentView.DialogueNode, childView.DialogueNode, edge.output.name, edge);
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
        editorDialogueNodeView.OnEdgeDeleted = OnEdgeDeleted;
        AddElement(editorDialogueNodeView);
    }
}
