using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateNodeMenu("Behaviour Tree/Root")]
[DisallowMultipleNodes, NodeTint("#000000")]
public class CAD_RootNode : CAD_NodeBT
{
    [Output(ShowBackingValue.Never, ConnectionType.Override), SerializeField] private int m_Children;

    public override CAD_NodeStateBT Execute(CAD_SmartTankBT tankAI)
    {
        return GetConnectedChildren().First().Execute(tankAI);
    }
}
