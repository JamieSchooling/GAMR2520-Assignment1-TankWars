using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The state machine graph's exit node. Optional, and tells the state machine processor when to stop processing.
/// </summary>
[CreateNodeMenu("State Machine/Exit")]
[DisallowMultipleNodes, NodeTint("#ff0000")]
public class CAD_ExitNode : XNode.Node
{
    [Input] private int m_InputNode;
}