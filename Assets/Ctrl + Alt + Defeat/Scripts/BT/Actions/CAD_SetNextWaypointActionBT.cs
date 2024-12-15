using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents an action node that sets the AI-controlled tank's next waypoint.
/// </summary>
public class CAD_SetNextWaypointActionBT : CAD_ActionBT
{
    /// <summary>
    /// Executes the action to update the AI tank's current waypoint index to the next waypoint in its list.
    /// If the current waypoint index exceeds the length of the waypoint array, it loops back to the first waypoint.
    /// </summary>
    /// <param name="tankAI">The <see cref="CAD_SmartTankBT"/> instance executing this action.</param>
    /// <returns>
    /// Always returns <see cref="CAD_NodeStateBT.Success"/> as the action is always successfully completed.
    /// </returns>
    public override CAD_NodeStateBT Execute(CAD_SmartTankBT tankAI)
    {
        // Increment the current waypoint index.
        tankAI.CurrentWaypointIndex++;

        // Loop back to the start if the index exceeds the array length.
        if (tankAI.CurrentWaypointIndex >= tankAI.Waypoints.Length)
        {
            tankAI.CurrentWaypointIndex = 0;
        }

        return CAD_NodeStateBT.Success;
    }
}
