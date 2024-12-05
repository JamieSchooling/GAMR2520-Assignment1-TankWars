using UnityEngine;
using XNodeEditor;

[CustomNodeEditor(typeof(CAD_ExitNode))]
public class CAD_ExitNodeEditor : NodeEditor
{
    public override void OnBodyGUI()
    {
        var node = target as CAD_ExitNode;

        NodeEditorGUILayout.PortField(new GUIContent(" "), node.GetInputPort("m_InputNode"), GUILayout.MinWidth(0));

        base.OnBodyGUI();
    }
}
