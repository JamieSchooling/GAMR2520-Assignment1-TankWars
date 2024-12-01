using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAD
{
    [DisallowMultipleNodes, NodeTint("#ff0000")]
    public class ExitNode : XNode.Node
    {
        [Input] private int m_InputNode;
    }
}