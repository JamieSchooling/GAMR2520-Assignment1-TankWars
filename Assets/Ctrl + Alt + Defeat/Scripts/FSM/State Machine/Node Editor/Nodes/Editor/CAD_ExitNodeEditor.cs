using UnityEngine;
using XNodeEditor;

/// <summary>
/// Custom editor for a state machine's exit node.
/// </summary>
[CustomNodeEditor(typeof(CAD_ExitNode))]
public class CAD_ExitNodeEditor : NodeEditor
{
    /// <summary>
    /// Removes the text label for the input port.
    /// </summary>
    public override void OnBodyGUI()
    {
        var node = target as CAD_ExitNode;

        NodeEditorGUILayout.PortField(new GUIContent(" "), node.GetInputPort("m_InputNode"), GUILayout.MinWidth(0));

        base.OnBodyGUI();
    }
}
