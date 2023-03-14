using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using DialogueSystem;
using UnityEditor.Callbacks;

public class DialogueEditor : EditorWindow
{
    private InspectorView _inspectorView;
    private DialogueEditorView _dialogueEditorView;

    [MenuItem("DialogueSystem/Editor ...")]
    public static void OpenWindow()
    {
        DialogueEditor wnd = GetWindow<DialogueEditor>();
        wnd.titleContent = new GUIContent("DialogueEditor");
    }

    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceId, int line)
    {
        if(Selection.activeObject is DialogueData)
        {
            OpenWindow();
            return true;
        }

        return false;
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/DialogueMaker/DialogueEditor.uxml");
        visualTree.CloneTree(root);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/DialogueMaker/DialogueEditor.uss");
        root.styleSheets.Add(styleSheet);

        _inspectorView = root.Q<InspectorView>();
        _dialogueEditorView = root.Q<DialogueEditorView>();
        _dialogueEditorView.OnNodeSelected += OnNodeSelectionChanged;
        _dialogueEditorView.OnNodeUnselected += OnNodeUnselected;

        OnSelectionChange();
    }

    private void OnSelectionChange()
    {
        DialogueData dialogueData = Selection.activeObject as DialogueData;
        if (dialogueData && AssetDatabase.CanOpenAssetInEditor(dialogueData.GetInstanceID()))
        {
            _dialogueEditorView.PoppulateView(dialogueData);
        }
    }

    private void OnNodeSelectionChanged(EditorDialogueNodeView editorDialogueNodeView)
    {
        _inspectorView.UpdateSelection(editorDialogueNodeView);
    }

    private void OnNodeUnselected()
    {
        _inspectorView.DestroyInspector();
    }
}
