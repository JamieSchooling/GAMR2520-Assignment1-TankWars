using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The state machine graph's entry node. Required for graph to be processed.
/// </summary>
[CreateNodeMenu("State Machine/Entry")]
[DisallowMultipleNodes, NodeTint("#00ff00")]
public class CAD_EntryNode : XNode.Node
{
    [Output] private int m_OutputNode;
}