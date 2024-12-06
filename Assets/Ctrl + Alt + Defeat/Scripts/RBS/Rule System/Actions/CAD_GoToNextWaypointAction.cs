using UnityEngine;

[CreateAssetMenu(fileName = "Go To Next Waypoint Action", menuName = "AI/RBS/Actions/Go To Next Waypoint")]
public class CAD_GoToNextWaypointAction : CAD_Action
{
    public override void Execute(CAD_SmartTankRBS tankAI, CAD_KnowledgeBase knowledgeBase)
    {
        tankAI.GoTo(knowledgeBase.CurrentSearchWaypoint);
    }
}
