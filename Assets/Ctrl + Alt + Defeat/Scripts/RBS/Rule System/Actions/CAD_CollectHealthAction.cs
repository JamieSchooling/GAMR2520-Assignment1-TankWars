using UnityEngine;

[CreateAssetMenu(fileName = "Collect Health Action", menuName = "AI/RBS/Actions/Collect Health")]
public class CAD_CollectHealthAction : CAD_Action
{
    public override void Execute(CAD_SmartTankRBS tankAI, CAD_KnowledgeBase knowledgeBase)
    {
        tankAI.GoTo(knowledgeBase.NearestHealthConsumable.transform.position);
    }
}
