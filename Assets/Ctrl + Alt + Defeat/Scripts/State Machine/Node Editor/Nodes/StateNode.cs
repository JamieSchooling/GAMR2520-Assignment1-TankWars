using System.Linq;
using UnityEngine;
using XNode;

namespace CAD
{
    public class StateNode : XNode.Node
    {
        [Input] private int m_InputNode;

        [SerializeField] private State m_State = null;

        private State m_PreviousState = null;

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
                AddDynamicOutput(typeof(Transition), ConnectionType.Override, TypeConstraint.None, transition.Name);
            }
        }

        public override object GetValue(NodePort port)
        {
            return m_State;
        }
    }
}
