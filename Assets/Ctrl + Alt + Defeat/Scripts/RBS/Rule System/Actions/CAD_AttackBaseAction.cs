using UnityEngine;

[CreateAssetMenu(fileName = "Attack Base Action", menuName = "AI/RBS/Actions/Attack Base")]
public class CAD_AttackBaseAction : CAD_Action
{
    /// <summary>
    /// commands the tank to shoot at the last known enemy base position
    /// </summary>
    /// <param name="tankAI"></param>
    /// <param name="knowledgeBase"></param>
    public override void Execute(CAD_SmartTankRBS tankAI, CAD_KnowledgeBase knowledgeBase)
    {
        tankAI.Attack(knowledgeBase.EnemyBasePosition);
    }
}
