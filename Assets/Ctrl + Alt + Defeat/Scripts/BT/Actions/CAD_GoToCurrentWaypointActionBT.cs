using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents an action node that commands the AI-controlled tank to move to its current waypoint.
/// </summary>
public class CAD_GoToCurrentWaypointActionBT : CAD_ActionBT
{
    /// <summary>
    /// Executes the action to move the AI tank towards its current waypoint.
    /// If the tank is already close to the waypoint (within 30 units), it succeeds immediately.
    /// Otherwise, it moves the tank to the waypoint and reports the action as still running.
    /// </summary>
    /// <param name="tankAI">The <see cref="CAD_SmartTankBT"/> instance executing this action.</param>
    /// <returns>
    /// - <see cref="CAD_NodeStateBT.Success"/> if the tank is within 30 units of the waypoint.
    /// - <see cref="CAD_NodeStateBT.Running"/> if the tank is still moving towards the waypoint.
    /// </returns>
    public override CAD_NodeStateBT Execute(CAD_SmartTankBT tankAI)
    {
        // Check if the tank is close enough to the current waypoint.
        if (Vector3.Distance(tankAI.transform.position, tankAI.CurrentWaypoint) <= 30.0f)
            return CAD_NodeStateBT.Success;

        // Command the tank to move towards the current waypoint.
        tankAI.GoTo(tankAI.CurrentWaypoint);
        return CAD_NodeStateBT.Running;
    }
}
