using UnityEngine;
using XNodeEditor;

/// <summary>
/// Custom editor for a state machine's entry node.
/// </summary>
[CustomNodeEditor(typeof(CAD_EntryNode))]
public class CAD_EntryNodeEditor : NodeEditor
{
    /// <summary>
    /// Removes the text label for the output port.
    /// </summary>
    public override void OnBodyGUI()
    {
        var node = target as CAD_EntryNode;

        NodeEditorGUILayout.PortField(new GUIContent(" "), node.GetOutputPort("m_OutputNode"), GUILayout.MinWidth(0));

        base.OnBodyGUI();
    }
}

