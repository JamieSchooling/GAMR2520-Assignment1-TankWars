using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAD
{
    [DisallowMultipleNodes]
    public class EntryNode : XNode.Node
    {
        [Output] private int m_OutputNode;
    }
}