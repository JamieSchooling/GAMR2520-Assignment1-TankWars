using UnityEngine;

[CreateAssetMenu(fileName = "Make New Waypoint Action", menuName = "AI/RBS/Actions/Make New Waypoint")]
public class CAD_MakeNewWaypointAction : CAD_Action
{
    /// <summary>
    /// Updates the tank's current waypoint.
    /// </summary>
    /// <param name="tankAI">The SmartTank instance that will execute this action.</param>
    /// <param name="knowledgeBase">The knowledge base that this SmartTank is using.</param>
    public override void Execute(CAD_SmartTankRBS tankAI, CAD_KnowledgeBase knowledgeBase)
    {
        tankAI.NextSearchWaypoint();
    }
}
