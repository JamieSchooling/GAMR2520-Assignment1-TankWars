using UnityEngine;
using XNode;

namespace CAD
{
    [CreateAssetMenu(menuName = "AI/State Machine")]
    public class StateMachine : NodeGraph
    {
        public XNode.Node CurrentNode { get; set; }
    }
}