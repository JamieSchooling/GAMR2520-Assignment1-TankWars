using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAD
{
    [DisallowMultipleNodes, NodeTint("#00ff00")]
    public class EntryNode : XNode.Node
    {
        [Output] private int m_OutputNode;
    }
}