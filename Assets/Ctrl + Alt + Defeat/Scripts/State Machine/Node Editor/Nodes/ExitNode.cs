using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAD
{
    [DisallowMultipleNodes]
    public class ExitNode : XNode.Node
    {
        [Input] private int m_InputNode;
    }
}