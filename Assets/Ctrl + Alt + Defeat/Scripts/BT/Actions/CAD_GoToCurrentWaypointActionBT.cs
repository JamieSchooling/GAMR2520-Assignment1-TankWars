using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAD_GoToCurrentWaypointActionBT : CAD_ActionBT
{
    public override CAD_NodeStateBT Execute(CAD_SmartTankBT tankAI)
    {
        if (Vector3.Distance(tankAI.transform.position, tankAI.CurrentWaypoint) <= 30.0f) return CAD_NodeStateBT.Success;

        tankAI.GoTo(tankAI.CurrentWaypoint);
        return CAD_NodeStateBT.Running;
    }
}
