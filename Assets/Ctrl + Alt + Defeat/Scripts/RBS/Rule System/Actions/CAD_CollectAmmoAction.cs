using UnityEngine;

[CreateAssetMenu(fileName = "Collect Ammo Action", menuName = "AI/RBS/Actions/Collect Ammo")]
public class CAD_CollectAmmoAction : CAD_Action
{
    public override void Execute(CAD_SmartTankRBS tankAI, CAD_KnowledgeBase knowledgeBase)
    {
        tankAI.GoTo(knowledgeBase.NearestAmmoConsumable.transform.position);
    }
}
