using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateNodeMenu("State Machine/Exit")]
[DisallowMultipleNodes, NodeTint("#ff0000")]
public class CAD_ExitNode : XNode.Node
{
    [Input] private int m_InputNode;
}