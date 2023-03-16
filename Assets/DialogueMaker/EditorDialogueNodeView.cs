using System;
using System.Collections.Generic;
using System.Linq;
using DialogueSystem;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class EditorDialogueNodeView: Node
{
    public new class UxmlFactory : UxmlFactory<Node, GraphView.UxmlTraits> { }

    public DialogueNode DialogueNode;
    public Port Input;
    public List<Port> Outputs = new List<Port>();
    public List<TextField> TextFields = new List<TextField>();

    public Action<EditorDialogueNodeView> OnNodeSelected;
    public Action OnNodeUnselected;
    public Action OnEdgeDeleted;

    public EditorDialogueNodeView(DialogueNode dialogueNode, Rect rect)/*: base("Assets/DialogueMaker/DialogueNodeView.uxml")*/
    {
        DialogueNode = dialogueNode;
        title = "DialogueNode";
        viewDataKey = DialogueNode.EditorData.GUID;
        SetPosition(rect);
        CreateInputPorts();
        CreateContet();

        dialogueNode.EditorData.Edges.ForEach(edge =>
        {
            if(Outputs.Any(output => output.name == edge.OutputGUID) == false)
                CreateOutputPorts(edge.OutputGUID);
        });
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        Undo.RecordObject(DialogueNode.EditorData, "DialogueEditor (Set Position)");
        DialogueNode.EditorData.EditorPosition.x = newPos.xMin;
        DialogueNode.EditorData.EditorPosition.y = newPos.yMin;
        EditorUtility.SetDirty(DialogueNode.EditorData);
    }

    public override void OnUnselected()
    {
        base.OnUnselected();
        OnNodeUnselected?.Invoke();
    }

    public override void OnSelected()
    {
        base.OnSelected();
        OnNodeSelected?.Invoke(this);
    }

    public Port GetOutputPort(string guid)
    {
        return Outputs.First(port => port.name == guid);
    }

    private void CreateInputPorts()
    {
        Input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));

        if (Input!=null)
        {
            Input.portName = "";
            inputContainer.Add(Input);
        }
    }

    private Port CreateOutputPorts(string guid)
    {
        var textField = new TextField("Text: ");
        textField.multiline = true;
        textField.labelElement.style.minWidth = 20;
        textField.labelElement.style.alignSelf = Align.Center;
        textField.style.minWidth = 200;
        var button = new Button(() => OnDeleteButtonClick(guid));
        button.text = "X";
        textField.hierarchy.Insert(0, button);
        textField.name = guid;
        TextFields.Add(textField);
        outputContainer.Add(textField);
        Port output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
        output.name = guid;

        if (Input != null)
        {
            output.portName = "";
            textField.contentContainer.Add(output);
        }

        Outputs.Add(output);
        return output;
    }


    private void CreateContet()
    {
        var button = new Button(()=> OnCreateButtonClick());
        button.text = "New stuff";
        titleContainer.Add(button);
    }

    private void OnCreateButtonClick()
    {
        string guid = GUID.Generate().ToString();
        Port output = CreateOutputPorts(guid);
        EdgeData edgeData = new EdgeData(guid, DialogueNode.EditorData.GUID);
        DialogueNode.EditorData.AddEdge(edgeData);
    }

    public void OnDeleteButtonClick(string guid)
    {
        var output =  Outputs.First(output => output.name == guid);
        var textfield =  TextFields.First(text => text.name == guid);

        DialogueNode.EditorData.Edges.ForEach(edgeData =>
        {
            if(edgeData.OutputGUID == guid)
            {
                edgeData.Delete();
            }
        });

        DialogueNode.EditorData.Edges.RemoveAll(edgeData => edgeData.OutputGUID == guid);

        outputContainer.Remove(textfield);
        Outputs.Remove(output);
        OnEdgeDeleted?.Invoke();
    }
}
