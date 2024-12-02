using UnityEngine;
using XNodeEditor;

namespace CAD
{
    [CustomNodeEditor(typeof(ExitNode))]
    public class ExitNodeEditor : NodeEditor
    {
        public override void OnBodyGUI()
        {
            var node = target as ExitNode;

            NodeEditorGUILayout.PortField(new GUIContent(" "), node.GetInputPort("m_InputNode"), GUILayout.MinWidth(0));

            base.OnBodyGUI();
        }
    }
}
