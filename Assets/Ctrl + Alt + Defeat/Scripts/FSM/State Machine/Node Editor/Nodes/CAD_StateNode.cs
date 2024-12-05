using System.Linq;
using UnityEditor;
using UnityEngine;
using XNode;

public class CAD_StateNode : XNode.Node
{
    [Input] private int m_InputNode;

    [SerializeField] private CAD_State m_State = null;

    public bool IsActive
    {
        get => m_IsActive;
        set
        {
            bool oldValue = m_IsActive;
            m_IsActive = value;
            if (oldValue != m_IsActive)
            {
                var windows = Resources.FindObjectsOfTypeAll<XNodeEditor.NodeEditorWindow>();
                if (windows.Length > 0) windows.First(window => window.graph == graph).Repaint();
            }
        } 
    }

    private bool m_IsActive = false;

    [SerializeField, HideInInspector] private CAD_State m_PreviousState = null;

    public void OnValidate()
    {
        if (m_State == m_PreviousState) return;

        m_PreviousState = m_State;

        foreach (var port in Ports.ToList())
        {
            if (!port.IsStatic) RemoveDynamicPort(port.fieldName);
        }

        if (m_State == null || m_State.Transitions == null) return;

        name = m_State.name;

        foreach (var transition in m_State.Transitions)
        {
            AddDynamicOutput(typeof(CAD_Transition), ConnectionType.Override, TypeConstraint.None, transition.Name);
        }
    }

    public override object GetValue(NodePort port)
    {
        return m_State;
    }
}
