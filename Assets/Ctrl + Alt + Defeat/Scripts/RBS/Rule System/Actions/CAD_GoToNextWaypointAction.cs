using UnityEngine;

[CreateAssetMenu(fileName = "Go To Next Waypoint Action", menuName = "AI/RBS/Actions/Go To Next Waypoint")]
public class CAD_GoToNextWaypointAction : CAD_Action
{
    /// <summary>
    /// tank goes towards the current search waypoint
    /// </summary>
    /// <param name="tankAI"></param>
    /// <param name="knowledgeBase"></param>
    public override void Execute(CAD_SmartTankRBS tankAI, CAD_KnowledgeBase knowledgeBase)
    {
        tankAI.GoTo(knowledgeBase.CurrentSearchWaypoint);
    }
}
