using UnityEngine;

[CreateAssetMenu(fileName = "Spin Action", menuName = "AI/RBS/Actions/Spin")]
public class CAD_SpinAction : CAD_Action
{
    /// <summary>
    /// rotates the tanks turret, saves fuel
    /// </summary>
    /// <param name="tankAI"></param>
    /// <param name="knowledgeBase"></param>
    public override void Execute(CAD_SmartTankRBS tankAI, CAD_KnowledgeBase knowledgeBase)
    {
        tankAI.SpinTurret();
    }
}
