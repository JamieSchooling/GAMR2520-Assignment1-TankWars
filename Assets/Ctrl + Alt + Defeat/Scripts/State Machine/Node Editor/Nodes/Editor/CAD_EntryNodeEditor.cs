using UnityEngine;
using XNodeEditor;


[CustomNodeEditor(typeof(CAD_EntryNode))]
public class CAD_EntryNodeEditor : NodeEditor
{
    public override void OnBodyGUI()
    {
        var node = target as CAD_EntryNode;

        NodeEditorGUILayout.PortField(new GUIContent(" "), node.GetOutputPort("m_OutputNode"), GUILayout.MinWidth(0));

        base.OnBodyGUI();
    }
}

