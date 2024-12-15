using UnityEngine;

[CreateAssetMenu(fileName = "Attack Enemy Action", menuName = "AI/RBS/Actions/Attack Enemy")]
public class CAD_AttackEnemyAction : CAD_Action
{
    /// <summary>
    /// Fires a projectile at the enemy tank.
    /// </summary>
    /// <param name="tankAI">The SmartTank instance that will execute this action.</param>
    /// <param name="knowledgeBase">The knowledge base that this SmartTank is using.</param>
    public override void Execute(CAD_SmartTankRBS tankAI, CAD_KnowledgeBase knowledgeBase)
    {
        knowledgeBase.CurrentSearchWaypoint = knowledgeBase.EnemyPosition;
        knowledgeBase.TimeLastSeenEnemy = Time.time;
        tankAI.Attack(knowledgeBase.EnemyPosition);
    }
}
