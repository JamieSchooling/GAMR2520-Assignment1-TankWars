using UnityEngine;

[CreateAssetMenu(fileName = "Spin Action", menuName = "AI/RBS/Actions/Spin")]
public class CAD_SpinAction : CAD_Action
{
    public override void Execute(CAD_SmartTankRBS tankAI, CAD_KnowledgeBase knowledgeBase)
    {
        tankAI.SpinTurret();
    }
}
