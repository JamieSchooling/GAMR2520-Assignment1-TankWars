using UnityEngine;

[CreateAssetMenu(fileName = "Attack Enemy Action", menuName = "AI/RBS/Actions/Attack Enemy")]
public class CAD_AttackEnemyAction : CAD_Action
{
    public override void Execute(CAD_SmartTankRBS tankAI, CAD_KnowledgeBase knowledgeBase)
    {
        tankAI.Attack(knowledgeBase.EnemyPosition);
    }
}
