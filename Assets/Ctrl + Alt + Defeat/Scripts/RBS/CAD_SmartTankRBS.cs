using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CAD_SmartTankRBS : AITank
{
    [SerializeField] private Vector3[] m_SearchWaypoints;

    public float FuelLevel => a_GetFuelLevel;
    public float HealthLevel => a_GetHealthLevel;
    public float AmmoLevel => a_GetAmmoLevel;
    public Dictionary<GameObject, float> TanksFound => a_TanksFound;
    public Dictionary<GameObject, float> BasesFound => a_BasesFound;
    public List<GameObject> FriendlyBases => a_GetMyBases;
    public Dictionary<GameObject, float> ConsumablesFound => a_ConsumablesFound;

    private CAD_RulesEngine m_RulesEngine;
    private CAD_KnowledgeBase m_KnowledgeBase;

    private Vector3 m_EnemyPosition = Vector3.zero;
    private int m_CurrentWaypointIndex = 0;

    public override void AITankStart()
    {
        m_RulesEngine = new(this);
        m_KnowledgeBase = new(this);
        InitializeRules(m_RulesEngine);
        m_KnowledgeBase.CurrentSearchWaypoint = GetClosestSearchWayPoint();
        m_CurrentWaypointIndex = m_SearchWaypoints.ToList().IndexOf(m_KnowledgeBase.CurrentSearchWaypoint);
    }

    public override void AITankUpdate()
    {
        m_RulesEngine.Update();
    }

    public override void AIOnCollisionEnter(Collision collision)
    {
        // TODO: Implement Collision Response
    }

    private void InitializeRules(CAD_RulesEngine rulesEngine)
    {
        rulesEngine.AddRule(new CAD_Rule(
            priority: 1,
            conditionName: "Enemy too close",
            condition: (tank) => m_KnowledgeBase.IsEnemySpotted && m_KnowledgeBase.IsEnemyInRange && m_KnowledgeBase.IsEnemyTooClose,
            action: (tank) => {
                m_KnowledgeBase.CurrentSearchWaypoint = m_KnowledgeBase.EnemyPosition;
                tank.GoTo((m_KnowledgeBase.NearestEnemyTank.transform.forward * 20.0f) + (m_KnowledgeBase.NearestEnemyTank.transform.right * 20.0f));
            }));

        // Attack if enemy in range and well-equipped
        rulesEngine.AddRule(new CAD_Rule(
            priority: 2,
            conditionName: "Enemy seen and in range",
            condition: (tank) => m_KnowledgeBase.IsEnemySpotted && m_KnowledgeBase.IsEnemyInRange && !m_KnowledgeBase.IsLowHealth && !m_KnowledgeBase.IsOutOfAmmo,
            action: (tank) => {
                m_KnowledgeBase.CurrentSearchWaypoint = m_KnowledgeBase.EnemyPosition;
                tank.Attack(m_KnowledgeBase.EnemyPosition);
            }));

        // Attack if enemy spotted and out of range
        rulesEngine.AddRule(new CAD_Rule(
            priority: 5,
            conditionName: "Enemy seen and out of range",
            condition: (tank) => m_KnowledgeBase.IsEnemySpotted && !m_KnowledgeBase.IsEnemyInRange 
                                && !m_KnowledgeBase.IsLowHealth && !m_KnowledgeBase.IsLowFuel && !m_KnowledgeBase.IsLowAmmo,
            action: (tank) => {
                m_KnowledgeBase.CurrentSearchWaypoint = m_KnowledgeBase.EnemyPosition;
                tank.GoTo(m_KnowledgeBase.EnemyPosition);
            }));

        // Attack if base spotted and in range
        rulesEngine.AddRule(new CAD_Rule(
            priority: 10,
            conditionName: "Base spotted and in range",
            condition: (tank) => m_KnowledgeBase.IsBaseSpotted && m_KnowledgeBase.IsBaseInRange && !m_KnowledgeBase.IsLowHealth && !m_KnowledgeBase.IsLowAmmo,
            action: (tank) => tank.Attack(m_KnowledgeBase.EnemyBasePosition)));

        // Attack if base spotted and out of range
        rulesEngine.AddRule(new CAD_Rule(
            priority: 10,
            conditionName: "Base seen and out of range",
            condition: (tank) => m_KnowledgeBase.IsBaseSpotted && !m_KnowledgeBase.IsBaseInRange && !m_KnowledgeBase.IsLowHealth && !m_KnowledgeBase.IsLowAmmo && !m_KnowledgeBase.IsLowFuel,
            action: (tank) => tank.GoTo(m_KnowledgeBase.EnemyBasePosition)));

        rulesEngine.AddRule(new CAD_Rule(
            priority: 20,
            conditionName: "Fuel Seen",
            condition: (tank) => m_KnowledgeBase.IsFuelSpotted && !m_KnowledgeBase.IsFuelFull,
            action: (tank) => tank.GoTo(m_KnowledgeBase.NearestFuelConsumable.transform.position)));

        rulesEngine.AddRule(new CAD_Rule(
            priority: 30,
            conditionName: "Health Seen",
            condition: (tank) => m_KnowledgeBase.IsHealthSpotted && !m_KnowledgeBase.IsHealthFull && !m_KnowledgeBase.IsLowFuel,
            action: (tank) => tank.GoTo(m_KnowledgeBase.NearestHealthConsumable.transform.position)));

        rulesEngine.AddRule(new CAD_Rule(
            priority: 40,
            conditionName: "Ammo Seen",
            condition: (tank) => m_KnowledgeBase.IsAmmoSpotted && !m_KnowledgeBase.IsAmmoFull && !m_KnowledgeBase.IsLowFuel,
            action: (tank) => tank.GoTo(m_KnowledgeBase.NearestAmmoConsumable.transform.position)));

        rulesEngine.AddRule(new CAD_Rule(
            priority: 50,
            conditionName: "Reached Search Waypoint",
            condition: (tank) => m_KnowledgeBase.HasReachedSearchWaypoint,
            action: (tank) => tank.NextSearchWaypoint()));

        rulesEngine.AddRule(new CAD_Rule(
            priority: 60,
            conditionName: "Searching",
            condition: (tank) => m_KnowledgeBase.CurrentSearchWaypoint != Vector3.zero && !m_KnowledgeBase.IsLowFuel,
            action: (tank) => tank.GoTo(m_KnowledgeBase.CurrentSearchWaypoint)));

        rulesEngine.AddRule(new CAD_Rule(
            priority: 100,
            conditionName: "Camping",
            condition: (tank) => true, // Always true if this point is reached
            action: (tank) => tank.SpinTurret()));
    }

    private void Attack(Vector3 position)
    {
        if (m_KnowledgeBase.IsEnemySpotted) Debug.Log(m_KnowledgeBase.DistanceToEnemy);
        GameObject target = CreateWaypoint(position, "Target");
        a_FaceTurretToPoint(target);
        a_FireAtPoint(target);
    }

    private void GoTo(Vector3 position, float speed = 1.0f)
    {
        GameObject waypoint = CreateWaypoint(position, "Waypoint");
        a_FollowPathToPoint(waypoint, speed);
    }

    private void NextSearchWaypoint()
    {
        m_CurrentWaypointIndex++;
        if (m_CurrentWaypointIndex >= m_SearchWaypoints.Count())
        {
            m_CurrentWaypointIndex = 0;
        }
        m_KnowledgeBase.CurrentSearchWaypoint = m_SearchWaypoints[m_CurrentWaypointIndex];
    }

    private Vector3 GetClosestSearchWayPoint()
    {
        float closestDistance = float.PositiveInfinity;
        int index = 0;
        for (int i = 0; i < m_SearchWaypoints.Length; i++)
        {
            float currentDistance = Vector3.Distance(transform.position, m_SearchWaypoints[i]);
            if (currentDistance < closestDistance)
            {
                index = i;
                closestDistance = currentDistance;
            }
        }
        return m_SearchWaypoints[index];
    }

    private void SpinTurret()
    {
        Transform turret = transform.Find("Model/Turret");
        Vector3 target = turret.forward + turret.right;
        a_FaceTurretToPoint(CreateWaypoint(transform.TransformPoint(target), "Spin Target"));
    }

    private GameObject CreateWaypoint(Vector3 position) => CreateWaypoint(position, "Waypoint", Time.deltaTime);
    private GameObject CreateWaypoint(Vector3 position, string name) => CreateWaypoint(position, name, Time.deltaTime);
    private GameObject CreateWaypoint(Vector3 position, float duration) => CreateWaypoint(position, "Waypoint", duration);
    private GameObject CreateWaypoint(Vector3 position, string name, float duration)
    {
        GameObject waypoint = new GameObject(name);
        waypoint.transform.position = position;
        Destroy(waypoint, duration);
        return waypoint;
    }
}
