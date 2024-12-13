using UnityEngine;

[CreateAssetMenu(fileName = "Make New Waypoint Action", menuName = "AI/RBS/Actions/Make New Waypoint")]
public class CAD_MakeNewWaypointAction : CAD_Action
{
    /// <summary>
    /// creates a new waypoint for the database
    /// </summary>
    /// <param name="tankAI"></param>
    /// <param name="knowledgeBase"></param>
    public override void Execute(CAD_SmartTankRBS tankAI, CAD_KnowledgeBase knowledgeBase)
    {
        tankAI.NextSearchWaypoint();
    }
}
