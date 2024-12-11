using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAD_SetNextWaypointActionBT : CAD_ActionBT
{
    public override CAD_NodeStateBT Execute(CAD_SmartTankBT tankAI)
    {
        tankAI.CurrentWaypointIndex++;

        if (tankAI.CurrentWaypointIndex >= tankAI.Waypoints.Length)
        {
            tankAI.CurrentWaypointIndex = 0;
        }

        return CAD_NodeStateBT.Success;
    }
}
