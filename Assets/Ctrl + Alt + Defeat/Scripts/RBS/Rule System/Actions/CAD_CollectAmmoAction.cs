using UnityEngine;

[CreateAssetMenu(fileName = "Collect Ammo Action", menuName = "AI/RBS/Actions/Collect Ammo")]
public class CAD_CollectAmmoAction : CAD_Action
{
    /// <summary>
    /// tank goes towards that last known position of the ammo consumable
    /// </summary>
    /// <param name="tankAI"></param>
    /// <param name="knowledgeBase"></param>
    public override void Execute(CAD_SmartTankRBS tankAI, CAD_KnowledgeBase knowledgeBase)
    {
        tankAI.GoTo(knowledgeBase.NearestAmmoConsumable.transform.position);
    }
}
