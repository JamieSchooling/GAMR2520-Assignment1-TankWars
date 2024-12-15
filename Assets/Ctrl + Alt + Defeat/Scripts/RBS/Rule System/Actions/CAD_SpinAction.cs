using UnityEngine;

[CreateAssetMenu(fileName = "Spin Action", menuName = "AI/RBS/Actions/Spin")]
public class CAD_SpinAction : CAD_Action
{
    /// <summary>
    /// Rotates the tank's turret to save fuel
    /// </summary>
    /// <param name="tankAI">The SmartTank instance that will execute this action.</param>
    /// <param name="knowledgeBase">The knowledge base that this SmartTank is using.</param>
    public override void Execute(CAD_SmartTankRBS tankAI, CAD_KnowledgeBase knowledgeBase)
    {
        tankAI.SpinTurret();
    }
}
