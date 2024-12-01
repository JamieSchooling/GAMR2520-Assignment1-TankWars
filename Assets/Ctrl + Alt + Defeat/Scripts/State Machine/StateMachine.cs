using System.Linq;
using UnityEngine;
using XNode;

namespace CAD
{
    public class StateMachine
    {
        private StateMachineGraph m_StateMachineGraph;
        private StateMachineGraph m_OriginalStateMachineGraph;
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

            m_OriginalStateMachineGraph = stateMachineGraph;
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

            foreach (XNode.Node node in m_OriginalStateMachineGraph.nodes)
            {
                StateNode stateNode = node as StateNode;
                if (stateNode != null) stateNode.IsActive = false;
            }

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

        public void End()
        {
            m_IsRunning = false;

            foreach (XNode.Node node in m_OriginalStateMachineGraph.nodes)
            {
                StateNode stateNode = node as StateNode;
                if (stateNode != null) stateNode.IsActive = false;
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

            StateNode stateNode = m_OriginalStateMachineGraph.nodes.Find(node =>
            {
                State thisNodeState = node.GetValue(port) as State;
                return thisNodeState == m_CurrentState;
            }) as StateNode;

            if (m_IsRunning)
            {
                m_CurrentState.OnStateExit(m_TankAI);
                if (stateNode != null) stateNode.IsActive = false;
            }

            if (port.Connection.node is ExitNode)
            {
                End();
                return;
            }

            m_StateMachineGraph.CurrentNode = port.Connection.node;
            m_CurrentState = m_StateMachineGraph.CurrentNode.GetValue(port) as State;

            stateNode = m_OriginalStateMachineGraph.nodes.Find(node =>
            {
                State thisNodeState = node.GetValue(port) as State;
                return thisNodeState == m_CurrentState;
            }) as StateNode;

            if (stateNode != null) stateNode.IsActive = true;

            m_CurrentState.OnStateEnter(m_TankAI);
        }
    }
}
