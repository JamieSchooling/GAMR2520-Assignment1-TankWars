using System.Linq;
using UnityEngine;
using XNode;

namespace CAD
{
    /// <summary>
    /// Represents a State Machine controlling AI behavior. Manages transitions and the execution of AI states.
    /// </summary>
    public class StateMachine
    {
        /// <summary>
        /// The state machine graph currently being executed.
        /// </summary>
        private StateMachineGraph m_StateMachineGraph;

        /// <summary>
        /// The original state machine graph, used for displaying the current state in the graph window.
        /// </summary>
        private StateMachineGraph m_OriginalStateMachineGraph;

        /// <summary>
        /// The currently active state in the state machine.
        /// </summary>
        private State m_CurrentState;

        /// <summary>
        /// The AI tank controlled by this state machine.
        /// </summary>
        private SmartTank m_TankAI;

        /// <summary>
        /// Indicates whether the state machine is running.
        /// </summary>
        private bool m_IsRunning = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateMachine"/> class.
        /// </summary>
        /// <param name="tankAI">The AI tank to control.</param>
        public StateMachine(SmartTank tankAI)
        {
            m_TankAI = tankAI;
        }

        /// <summary>
        /// Starts the state machine with the specified state machine graph.
        /// </summary>
        /// <param name="stateMachineGraph">The graph defining the state machine's behavior.</param>
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

            if (m_StateMachineGraph.CurrentNode == null)
                Debug.LogError("Failed to start State Machine, no Entry Node found in graph.");

            foreach (XNode.Node node in m_OriginalStateMachineGraph.nodes)
            {
                StateNode stateNode = node as StateNode;
                if (stateNode != null) stateNode.IsActive = false;
            }

            SwitchState("m_OutputNode");
            m_IsRunning = true;
        }

        /// <summary>
        /// Updates the state machine logic. Transitions to new states based on conditions.
        /// </summary>
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

        /// <summary>
        /// Ends the state machine and resets all state nodes to inactive.
        /// </summary>
        public void End()
        {
            m_IsRunning = false;

            foreach (XNode.Node node in m_OriginalStateMachineGraph.nodes)
            {
                StateNode stateNode = node as StateNode;
                if (stateNode != null) stateNode.IsActive = false;
            }
        }

        /// <summary>
        /// Switches the current state of the state machine to the one connected to the specified port.
        /// </summary>
        /// <param name="portName">The name of the port to transition from.</param>
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
