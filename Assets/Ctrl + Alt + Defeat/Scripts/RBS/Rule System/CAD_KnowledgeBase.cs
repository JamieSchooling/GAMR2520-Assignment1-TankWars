using System.Linq;
using UnityEngine;

/// <summary>
/// Holds all the information that the tank should be aware of.
/// </summary>
public class CAD_KnowledgeBase
{
    /// <summary>
    /// Returns whether the tank is on or below 30 fuel points.
    /// </summary>
    public bool IsLowFuel => m_TankAI.FuelLevel <= 30.0f;

    /// <summary>
    /// Returns whether the tank is on or below 10 fuel points.
    /// </summary>
    public bool IsOutOfFuel => m_TankAI.FuelLevel <= 10.0f;

    /// <summary>
    /// Returns whether the tank has less than 30 health points.
    /// </summary>
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

    /// <summary>
    /// Returns whether the tank has less than 4 ammo.
    /// </summary>
    public bool IsLowAmmo => m_TankAI.AmmoLevel < 4.0f;

    /// <summary>
    /// Returns whether the tank has used up all ammo.
    /// </summary>
    public bool IsOutOfAmmo => m_TankAI.AmmoLevel < 1.0f;

    /// <summary>
    /// Returns whether the tank's fuel is currently full.
    /// </summary>
    public bool IsFuelFull => m_TankAI.FuelLevel == 125.0f;

    /// <summary>
    /// Returns whether the tank's health is currently full.
    /// </summary>
    public bool IsHealthFull => m_TankAI.HealthLevel == 125.0f;

    /// <summary>
    /// Returns whether the tank's ammo is currently full.
    /// </summary>
    public bool IsAmmoFull => m_TankAI.AmmoLevel == 20.0f;

    /// <summary>
    /// Returns whether any enemy tanks are currently in view.
    /// </summary>
    public bool IsEnemySpotted => m_TankAI.TanksFound.Count > 0;

    /// <summary>
    /// Returns whether any enemy bases are currently in view.
    /// </summary>
    public bool IsBaseSpotted => m_TankAI.BasesFound.Count > 0;

    /// <summary>
    /// Returns whether any friendly bases are remaining.
    /// </summary>
    public bool HasFriendlyBases => m_TankAI.FriendlyBases.Count > 0;

    /// <summary>
    /// Returns whether the currently visible enemy is within 25 units.
    /// </summary>
    public bool IsEnemyInRange => DistanceToEnemy < 25.0f;

    /// <summary>
    /// Returns whether the currently visible base is within 25 units.
    /// </summary>
    public bool IsBaseInRange => DistanceToBase < 25.0f;

    /// <summary>
    /// Returns whether the currently visible enemy is within 10 units.
    /// </summary>
    public bool IsEnemyTooClose => DistanceToEnemy < 10.0f;

    /// <summary>
    /// Returns whether the currently visible base is within 10 units.
    /// </summary>
    public bool IsBaseTooClose => DistanceToBase < 10.0f;

    /// <summary>
    /// Returns whether any health consumables are currently visible.
    /// </summary>
    public bool IsHealthSpotted => m_TankAI.ConsumablesFound.Where(c => c.Key.CompareTag("Health")).Count() > 0;

    /// <summary>
    /// Returns whether any fuel consumables are currently visible.
    /// </summary>
    public bool IsFuelSpotted => m_TankAI.ConsumablesFound.Where(c => c.Key.CompareTag("Fuel")).Count() > 0;

    /// <summary>
    /// Returns whether any ammo consumables are currently visible.
    /// </summary>
    public bool IsAmmoSpotted => m_TankAI.ConsumablesFound.Where(c => c.Key.CompareTag("Ammo")).Count() > 0;

    /// <summary>
    /// Returns whether the current waypoint is within 25 units.
    /// </summary>
    public bool HasReachedSearchWaypoint => Vector3.Distance(m_TankAI.transform.position, CurrentSearchWaypoint) < 25.0f;

    /// <summary>
    /// Returns whether the current waypoint is valid (not at the origin).
    /// </summary>
    public bool IsWaypointValid => CurrentSearchWaypoint != Vector3.zero;

    /// <summary>
    /// Returns whether the enemy tank has been seen within the last 30 seconds.
    /// </summary>
    public bool HasNotSeenEnemyForAWhile => TimeSinceEnemySeen >= 30.0f;

    /// <summary>
    /// Always true, used for fallback rules.
    /// </summary>
    public bool Default => true;

    /// <summary>
    /// Returns the nearest enemy's position in world space.
    /// </summary>
    public Vector3 EnemyPosition => NearestEnemyTank.transform.position;

    /// <summary>
    /// Returns the nearest enemy base's position in world space.
    /// </summary>
    public Vector3 EnemyBasePosition => NearestEnemyBase ? NearestEnemyBase.transform.position : Vector3.zero;

    /// <summary>
    /// The current waypoint position.
    /// </summary>
    public Vector3 CurrentSearchWaypoint { get; set; } = Vector3.zero;

    /// <summary>
    /// Returns the nearest enemy tank GameObject
    /// </summary>
    public GameObject NearestEnemyTank => m_TankAI.TanksFound.OrderBy(t => t.Value).First().Key;

    /// <summary>
    /// Returns the nearest enemy base GameObject
    /// </summary>
    public GameObject NearestEnemyBase => m_TankAI.BasesFound.OrderBy(t => t.Value).First().Key;

    /// <summary>
    /// Returns a friendly base GameObject
    /// </summary>
    public GameObject FriendlyBase => m_TankAI.FriendlyBases.First();

    /// <summary>
    /// Returns the nearest health consumable GameObject
    /// </summary>
    public GameObject NearestHealthConsumable => m_TankAI.ConsumablesFound.Where(c => c.Key.CompareTag("Health")).First().Key;

    /// <summary>
    /// Returns the nearest fuel consumable GameObject
    /// </summary>
    public GameObject NearestFuelConsumable => m_TankAI.ConsumablesFound.Where(c => c.Key.CompareTag("Fuel")).First().Key;

    /// <summary>
    /// Returns the nearest ammo consumable GameObject
    /// </summary>
    public GameObject NearestAmmoConsumable => m_TankAI.ConsumablesFound.Where(c => c.Key.CompareTag("Ammo")).First().Key;

    /// <summary>
    /// Returns the distance to the closest visible enemy tank.
    /// </summary>
    public float DistanceToEnemy => m_TankAI.TanksFound.OrderBy(t => t.Value).First().Value;

    /// <summary>
    /// Returns the distance to the closest visible enemy base.
    /// </summary>
    public float DistanceToBase => m_TankAI.BasesFound.OrderBy(t => t.Value).First().Value;
    public float TimeLastSeenEnemy { get; set; }
    public float TimeSinceEnemySeen => Time.time - TimeLastSeenEnemy;

    /// <summary>
    /// The smart tank instance processing this information.
    /// </summary>
    private CAD_SmartTankRBS m_TankAI;

    public CAD_KnowledgeBase(CAD_SmartTankRBS tankAI)
    {
        m_TankAI = tankAI;
    }
}
