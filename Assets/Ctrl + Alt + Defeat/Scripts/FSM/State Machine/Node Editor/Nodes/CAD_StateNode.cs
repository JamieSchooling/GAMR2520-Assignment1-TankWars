using System.Linq;
using UnityEditor;
using UnityEngine;
using XNode;

/// <summary>
/// The state machine graph's state node.
/// </summary>
[CreateNodeMenu("State Machine/State")]
public class CAD_StateNode : XNode.Node
{
    [Input] private int m_InputNode;

    /// <summary>
    /// This node's state.
    /// </summary>
    [SerializeField] private CAD_State m_State = null;

    /// <summary>
    /// Sets this node as the current active node. Updates graph visual to show currently active node.
    /// </summary>
    /// <returns>If this node is active.</returns>
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

    /// <summary>
    /// Holds the previous state this node represented. 
    /// Used to ensure updates to this node's ports only happen if the state it represents changes.
    /// </summary>
    [SerializeField, HideInInspector] private CAD_State m_PreviousState = null;

    /// <summary>
    /// Updates the output ports based on the state's transitions.
    /// </summary>
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

    /// <summary>
    /// Gets the value of this node.
    /// </summary>
    /// <returns>The state this node represents</returns>
    public override object GetValue(NodePort port)
    {
        return m_State;
    }
}
