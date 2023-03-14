using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class InspectorView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

    private Editor _editor;

    public InspectorView()
    {
    }

    internal void UpdateSelection(EditorDialogueNodeView editorDialogueNodeView)
    {
        DestroyInspector();
        _editor = Editor.CreateEditor(editorDialogueNodeView.DialogueNode);
        IMGUIContainer container = new IMGUIContainer(() => { _editor.OnInspectorGUI(); });
        Add(container);
    }

    internal void DestroyInspector()
    {
        Clear();

        UnityEngine.Object.DestroyImmediate(_editor);
    }
}
