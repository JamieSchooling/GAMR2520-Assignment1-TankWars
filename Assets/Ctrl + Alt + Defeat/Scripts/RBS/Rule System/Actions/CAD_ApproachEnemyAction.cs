using UnityEngine;

[CreateAssetMenu(fileName = "Approach Enemy Action", menuName = "AI/RBS/Actions/Approach Enemy")]
public class CAD_ApproachEnemyAction : CAD_Action
{
    public override void Execute(CAD_SmartTankRBS tankAI, CAD_KnowledgeBase knowledgeBase)
    {
        knowledgeBase.CurrentSearchWaypoint = knowledgeBase.EnemyPosition;
        knowledgeBase.TimeLastSeenEnemy = Time.time;
        tankAI.GoTo(knowledgeBase.EnemyPosition);
    }
}
