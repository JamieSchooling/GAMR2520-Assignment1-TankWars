using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateNodeMenu("State Machine/Entry")]
[DisallowMultipleNodes, NodeTint("#00ff00")]
public class CAD_EntryNode : XNode.Node
{
    [Output] private int m_OutputNode;
}