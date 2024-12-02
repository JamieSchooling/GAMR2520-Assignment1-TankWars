using UnityEngine;
using XNode;

[CreateAssetMenu(menuName = "AI/State Machine")]
public class CAD_StateMachine : NodeGraph
{
    public XNode.Node CurrentNode { get; set; }
}