using System.Linq;
using UnityEngine;

public class CAD_KnowledgeBase
{
    public bool IsLowFuel => m_TankAI.FuelLevel <= 30.0f;
    public bool IsLowHealth {
        get 
        {
            if (m_TankAI.HealthLevel < 30.0f)
            {
                CurrentSearchWaypoint = HasFriendlyBases ? FriendlyBase.transform.position: -EnemyPosition;
            }
            return m_TankAI.HealthLevel < 30.0f;
        }
    }
    public bool IsLowAmmo => m_TankAI.AmmoLevel < 4.0f;
    public bool IsOutOfAmmo => m_TankAI.AmmoLevel < 1.0f;

    public bool IsFuelFull => m_TankAI.FuelLevel == 125.0f;
    public bool IsHealthFull => m_TankAI.HealthLevel == 125.0f;
    public bool IsAmmoFull => m_TankAI.AmmoLevel == 20.0f;

    public bool IsEnemySpotted => m_TankAI.TanksFound.Count > 0;
    public bool IsBaseSpotted => m_TankAI.BasesFound.Count > 0;
    public bool HasFriendlyBases => m_TankAI.FriendlyBases.Count > 0;

    public bool IsEnemyInRange => DistanceToEnemy < 25.0f;
    public bool IsBaseInRange => DistanceToBase < 25.0f;

    public bool IsEnemyTooClose => DistanceToEnemy < 10.0f;
    public bool IsBaseTooClose => DistanceToBase < 10.0f;

    public bool IsHealthSpotted => m_TankAI.ConsumablesFound.Where(c => c.Key.CompareTag("Health")).Count() > 0;
    public bool IsFuelSpotted => m_TankAI.ConsumablesFound.Where(c => c.Key.CompareTag("Fuel")).Count() > 0;
    public bool IsAmmoSpotted => m_TankAI.ConsumablesFound.Where(c => c.Key.CompareTag("Ammo")).Count() > 0;

    public bool HasReachedSearchWaypoint => Vector3.Distance(m_TankAI.transform.position, CurrentSearchWaypoint) < 25.0f;
    public bool IsWaypointValid => CurrentSearchWaypoint != Vector3.zero;

    public bool HasNotSeenEnemyForAWhile => TimeSinceEnemySeen >= 30.0f;

    public bool Default => true;

    public Vector3 EnemyPosition => NearestEnemyTank.transform.position;
    public Vector3 EnemyBasePosition => NearestEnemyBase ? NearestEnemyBase.transform.position : Vector3.zero;

    public Vector3 CurrentSearchWaypoint { get; set; } = Vector3.zero;

    public GameObject NearestEnemyTank => m_TankAI.TanksFound.OrderBy(t => t.Value).First().Key;
    public GameObject NearestEnemyBase => m_TankAI.BasesFound.OrderBy(t => t.Value).First().Key;
    public GameObject FriendlyBase => m_TankAI.FriendlyBases.First();

    public GameObject NearestHealthConsumable => m_TankAI.ConsumablesFound.Where(c => c.Key.CompareTag("Health")).First().Key;
    public GameObject NearestFuelConsumable => m_TankAI.ConsumablesFound.Where(c => c.Key.CompareTag("Fuel")).First().Key;
    public GameObject NearestAmmoConsumable => m_TankAI.ConsumablesFound.Where(c => c.Key.CompareTag("Ammo")).First().Key;

    public float DistanceToEnemy => m_TankAI.TanksFound.OrderBy(t => t.Value).First().Value;
    public float DistanceToBase => m_TankAI.BasesFound.OrderBy(t => t.Value).First().Value;
    public float TimeLastSeenEnemy { get; set; }
    public float TimeSinceEnemySeen => Time.time - TimeLastSeenEnemy;

    private CAD_SmartTankRBS m_TankAI;

    public CAD_KnowledgeBase(CAD_SmartTankRBS tankAI)
    {
        m_TankAI = tankAI;
    }
}
