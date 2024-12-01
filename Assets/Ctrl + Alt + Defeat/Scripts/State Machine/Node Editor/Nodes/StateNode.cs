using System.Linq;
using UnityEngine;
using XNode;

namespace CAD
{
    public class StateNode : XNode.Node
    {
        [Input] private int m_InputNode;

        [SerializeField] private State m_State = null;

        public void OnValidate()
        {
            foreach (var port in Ports.ToList())
            {
                if (!port.IsStatic) RemoveDynamicPort(port.fieldName);
            }

            if (m_State == null || m_State.Transitions == null) return;

            foreach (var transition in m_State.Transitions)
            {
                AddDynamicOutput(typeof(Transition), ConnectionType.Override, TypeConstraint.None, transition.Name);
            }
        }

        public override object GetValue(NodePort port)
        {
            if (!port.IsOutput) return null;

            return m_State.Transitions.FirstOrDefault(t => t.Name == port.fieldName).Condition;
        }
    }
}
