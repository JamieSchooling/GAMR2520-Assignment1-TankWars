using UnityEngine;

[CreateAssetMenu(fileName = "Attack Base Action", menuName = "AI/RBS/Actions/Attack Base")]
public class CAD_AttackBaseAction : CAD_Action
{
    public override void Execute(CAD_SmartTankRBS tankAI, CAD_KnowledgeBase knowledgeBase)
    {
        tankAI.Attack(knowledgeBase.EnemyBasePosition);
    }
}
