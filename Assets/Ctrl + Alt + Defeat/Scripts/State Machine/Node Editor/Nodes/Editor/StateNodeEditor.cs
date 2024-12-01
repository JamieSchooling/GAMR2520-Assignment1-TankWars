using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace CAD
{
    [CustomNodeEditor(typeof(StateNode))]
    public class StateNodeEditor : NodeEditor
    {
        private static GUIStyle editorLabelStyle;

        public override void OnBodyGUI()
        {
            var node = target as StateNode;

            NodeEditorGUILayout.PortField(new GUIContent(" "), node.GetInputPort("m_InputNode"), GUILayout.MinWidth(0));

            editorLabelStyle ??= new GUIStyle(EditorStyles.label);
            if (node.IsActive) EditorStyles.label.normal.textColor = Color.black;
            base.OnBodyGUI();
            EditorStyles.label.normal = editorLabelStyle.normal;
        }

        public override Color GetTint()
        {
            var node = target as StateNode;
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
}
