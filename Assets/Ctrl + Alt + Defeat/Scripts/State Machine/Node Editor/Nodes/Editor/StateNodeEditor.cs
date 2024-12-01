using UnityEngine;
using XNodeEditor;

namespace CAD
{
    [CustomNodeEditor(typeof(StateNode))]
    public class StateNodeEditor : NodeEditor
    {
        public override void OnBodyGUI()
        {
            var node = target as StateNode;

            NodeEditorGUILayout.PortField(new GUIContent(" "), node.GetInputPort("m_InputNode"), GUILayout.MinWidth(0));

            base.OnBodyGUI();
        }
    }
}
