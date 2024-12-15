using UnityEngine;

[CreateAssetMenu(fileName = "Go To Next Waypoint Action", menuName = "AI/RBS/Actions/Go To Next Waypoint")]
public class CAD_GoToNextWaypointAction : CAD_Action
{
    /// <summary>
    /// Moves the tank towards the current search waypoint.
    /// </summary>
    /// <param name="tankAI">The SmartTank instance that will execute this action.</param>
    /// <param name="knowledgeBase">The knowledge base that this SmartTank is using.</param>
    public override void Execute(CAD_SmartTankRBS tankAI, CAD_KnowledgeBase knowledgeBase)
    {
        tankAI.GoTo(knowledgeBase.CurrentSearchWaypoint);
    }
}
