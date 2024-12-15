using UnityEditor;
using UnityEngine;
using XNodeEditor;

/// <summary>
/// Custom editor script for a state node.
/// </summary>
[CustomNodeEditor(typeof(CAD_StateNode))]
public class CAD_StateNodeEditor : NodeEditor
{
    /// <summary>
    /// Removes the text label for the output port. Changes the text colour if the node is active.
    /// </summary>
    private static GUIStyle editorLabelStyle;

    public override void OnBodyGUI()
    {
        var node = target as CAD_StateNode;

        NodeEditorGUILayout.PortField(new GUIContent(" "), node.GetInputPort("m_InputNode"), GUILayout.MinWidth(0));

        editorLabelStyle ??= new GUIStyle(EditorStyles.label);
        if (node.IsActive) EditorStyles.label.normal.textColor = Color.black;
        base.OnBodyGUI();
        EditorStyles.label.normal = editorLabelStyle.normal;
    }

    /// <summary>
    /// Changes the node colour if the node is active.
    /// </summary>
    /// <returns>The colour of the node.</returns>
    public override Color GetTint()
    {
        var node = target as CAD_StateNode;
        if (node.IsActive)
        {
            return Color.cyan;
        }
        else
        {
            return base.GetTint();
        }
    }
}
