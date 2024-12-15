using UnityEngine;

[CreateAssetMenu(fileName = "Approach Enemy Action", menuName = "AI/RBS/Actions/Approach Enemy")]
public class CAD_ApproachEnemyAction : CAD_Action
{
    /// <summary>
    /// Moves the tank towards the enemy tank.
    /// </summary>
    /// <param name="tankAI">The SmartTank instance that will execute this action.</param>
    /// <param name="knowledgeBase">The knowledge base that this SmartTank is using.</param>
    public override void Execute(CAD_SmartTankRBS tankAI, CAD_KnowledgeBase knowledgeBase)
    {
        knowledgeBase.CurrentSearchWaypoint = knowledgeBase.EnemyPosition;
        knowledgeBase.TimeLastSeenEnemy = Time.time;
        tankAI.GoTo(knowledgeBase.EnemyPosition);
    }
}
