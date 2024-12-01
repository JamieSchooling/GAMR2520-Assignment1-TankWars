using System.Linq;
using UnityEngine;
using XNode;

namespace CAD
{
    public class StateMachine
    {
        private StateMachineGraph m_StateMachineGraph;
        private State m_CurrentState;
        private SmartTank m_TankAI;

        private bool m_IsRunning = false;

        public StateMachine(SmartTank tankAI)
        {
            m_TankAI = tankAI;
        }

        public void Start(StateMachineGraph stateMachineGraph)
        {
            if (m_StateMachineGraph != null) return;

            m_StateMachineGraph = stateMachineGraph.Copy() as StateMachineGraph;

            foreach (XNode.Node node in m_StateMachineGraph.nodes)
            {
                if (node is EntryNode)
                {
                    m_StateMachineGraph.CurrentNode = node;
                    break;
                }
            }

            if (m_StateMachineGraph.CurrentNode == null) Debug.LogError("Failed to start State Machine, no Entry Node found in graph.");

            SwitchState("m_OutputNode");
            m_IsRunning = true;
        }

        public void Update()
        {
            if (!m_IsRunning) return;

            m_CurrentState.OnStateUpdate(m_TankAI);

            foreach (Transition transition in m_CurrentState.Transitions)
            {
                if (transition.Condition(m_TankAI))
                {
                    SwitchState(transition.Name);
                    return;
                }
            }
        }

        private void SwitchState(string portName)
        {
            XNode.Node currentNode = m_StateMachineGraph.CurrentNode;

            NodePort port = currentNode.GetOutputPort(portName);

            if (port == null)
            {
                Debug.LogError($"Couldn't fetch port [{portName}].");
                return;
            }

            if (!port.IsConnected)
            {
                Debug.LogError($"Port [{portName}] is not connected.");
                return;
            }

            if (m_IsRunning) m_CurrentState.OnStateExit(m_TankAI);

            if (port.Connection.node is ExitNode)
            {
                m_IsRunning = false;
                return;
            }

            m_StateMachineGraph.CurrentNode = port.Connection.node;
            m_CurrentState = m_StateMachineGraph.CurrentNode.GetValue(port) as State;

            m_CurrentState.OnStateEnter(m_TankAI);
        }
    }
}
