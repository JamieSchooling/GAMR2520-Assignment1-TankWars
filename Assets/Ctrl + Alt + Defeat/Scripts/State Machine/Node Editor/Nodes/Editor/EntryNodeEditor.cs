using UnityEngine;
using XNodeEditor;

namespace CAD
{
    [CustomNodeEditor(typeof(EntryNode))]
    public class EntryNodeEditor : NodeEditor
    {
        public override void OnBodyGUI()
        {
            var node = target as EntryNode;

            NodeEditorGUILayout.PortField(new GUIContent(" "), node.GetOutputPort("m_OutputNode"), GUILayout.MinWidth(0));

            base.OnBodyGUI();
        }
    }
}
