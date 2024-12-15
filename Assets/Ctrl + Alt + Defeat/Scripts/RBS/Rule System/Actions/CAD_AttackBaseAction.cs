using UnityEngine;

[CreateAssetMenu(fileName = "Attack Base Action", menuName = "AI/RBS/Actions/Attack Base")]
public class CAD_AttackBaseAction : CAD_Action
{
    /// <summary>
    /// Fires a projectile at the enemy's base.
    /// </summary>
    /// <param name="tankAI">The SmartTank instance that will execute this action.</param>
    /// <param name="knowledgeBase">The knowledge base that this SmartTank is using.</param>
    public override void Execute(CAD_SmartTankRBS tankAI, CAD_KnowledgeBase knowledgeBase)
    {
        tankAI.Attack(knowledgeBase.EnemyBasePosition);
    }
}
