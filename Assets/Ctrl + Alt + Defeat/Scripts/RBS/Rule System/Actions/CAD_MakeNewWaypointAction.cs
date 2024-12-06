using UnityEngine;

[CreateAssetMenu(fileName = "Make New Waypoint Action", menuName = "AI/RBS/Actions/Make New Waypoint")]
public class CAD_MakeNewWaypointAction : CAD_Action
{
    public override void Execute(CAD_SmartTankRBS tankAI, CAD_KnowledgeBase knowledgeBase)
    {
        tankAI.NextSearchWaypoint();
    }
}
