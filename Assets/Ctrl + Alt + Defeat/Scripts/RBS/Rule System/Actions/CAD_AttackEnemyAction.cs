using UnityEngine;

[CreateAssetMenu(fileName = "Attack Enemy Action", menuName = "AI/RBS/Actions/Attack Enemy")]
public class CAD_AttackEnemyAction : CAD_Action
{
    /// <summary>
    /// commands the tank to shoot at the last known enemy position
    /// </summary>
    /// <param name="tankAI"></param>
    /// <param name="knowledgeBase"></param>
    public override void Execute(CAD_SmartTankRBS tankAI, CAD_KnowledgeBase knowledgeBase)
    {
        knowledgeBase.CurrentSearchWaypoint = knowledgeBase.EnemyPosition;
        knowledgeBase.TimeLastSeenEnemy = Time.time;
        tankAI.Attack(knowledgeBase.EnemyPosition);
    }
}
