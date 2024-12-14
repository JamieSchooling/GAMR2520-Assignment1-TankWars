using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CAD_SmartTankRBS : AITank
{
    /// <summary>
    /// The rules the tank should evaluate.
    /// </summary>
    [SerializeField] private CAD_Rules m_Rules;

    /// <summary>
    /// The waypoints the tank should follow while patrolling.
    /// </summary>
    [SerializeField] private Vector3[] m_SearchWaypoints;

    /// <summary>
    /// The tank's current fuel level
    /// </summary>
    public float FuelLevel => a_GetFuelLevel;

    /// <summary>
    /// The tank's current health level
    /// </summary>
    public float HealthLevel => a_GetHealthLevel;

    /// <summary>
    /// The tank's current ammo level
    /// </summary>
    public float AmmoLevel => a_GetAmmoLevel;

    /// <summary>
    /// A dictionary of all visiable enemy tanks. 
    /// Key: The visible tank GameObject, Value: Distance to the tank.
    /// </summary>
    public Dictionary<GameObject, float> TanksFound => a_TanksFound;

    /// <summary>
    /// A dictionary of all visiable enemy bases. 
    /// Key: The visible base GameObject, Value: Distance to the base.
    /// </summary>
    public Dictionary<GameObject, float> BasesFound => a_BasesFound;

    /// <summary>
    /// List of all remaining friendly bases.
    /// </summary>
    public List<GameObject> FriendlyBases => a_GetMyBases;

    /// <summary>
    /// A dictionary of all visiable consumables. 
    /// Key: The visible consumable GameObject, Value: Distance to the consumable.
    /// </summary>
    public Dictionary<GameObject, float> ConsumablesFound => a_ConsumablesFound;

    /// <summary>
    /// The rules engine instance that evaluates this AI's rules.
    /// </summary>
    private CAD_RulesEngine m_RulesEngine;

    /// <summary>
    /// Knowledge base instance that holds all facts about this tank AI.
    /// </summary>
    private CAD_KnowledgeBase m_KnowledgeBase;

    /// <summary>
    /// Last known enemy position.
    /// </summary>
    private Vector3 m_EnemyPosition = Vector3.zero;

    /// <summary>
    /// The index of the current waypoint the tank will navigate to.
    /// </summary>
    private int m_CurrentWaypointIndex = 0;

    /// <summary>
    /// Initialises the rules engine and knowledge base,
    /// sets the first index of knowledge first search waypoint,
    /// sets the current waypoint index to the first item in the list
    /// </summary>
    public override void AITankStart()
    {
        m_RulesEngine = new(this, m_Rules);
        m_KnowledgeBase = new(this);
        m_KnowledgeBase.CurrentSearchWaypoint = GetClosestSearchWayPoint();
        m_CurrentWaypointIndex = m_SearchWaypoints.ToList().IndexOf(m_KnowledgeBase.CurrentSearchWaypoint);
    }

    /// <summary>
    /// Updates the rules engine with the current knowledge base.
    /// </summary>
    public override void AITankUpdate()
    {
        m_RulesEngine.Update(m_KnowledgeBase);
    }

    public override void AIOnCollisionEnter(Collision collision)
    {
        // TODO: Implement Collision Response
    }

    /// <summary>
    /// Creates a GameObject to look at and then fires at the GameObject's position.
    /// </summary>
    /// <param name="position"></param>
    public void Attack(Vector3 position)
    {
        GameObject target = CreateWaypoint(position, "Target");
        a_FaceTurretToPoint(target);
        a_FireAtPoint(target);
    }

    /// <summary>
    /// Creates a GameObject at waypoint, then the tank goes towards the point using a pathfinding algorithm
    /// </summary>
    /// <param name="position"></param>
    /// <param name="speed"></param>
    public void GoTo(Vector3 position, float speed = 1.0f)
    {
        GameObject waypoint = CreateWaypoint(position, "Waypoint");
        a_FollowPathToPoint(waypoint, speed);
    }

    /// <summary>
    /// Increments the current waypoint index, and updates the current waypoint position in the knowledge base.
    /// </summary>
    public void NextSearchWaypoint()
    {
        m_CurrentWaypointIndex++;
        if (m_CurrentWaypointIndex >= m_SearchWaypoints.Count())
        {
            m_CurrentWaypointIndex = 0;
        }
        m_KnowledgeBase.CurrentSearchWaypoint = m_SearchWaypoints[m_CurrentWaypointIndex];
    }

    /// <summary>
    /// Finds and returns the waypoint position closest to the tanks current position.
    /// </summary>
    /// <returns>The closet waypoint position</returns>
    public Vector3 GetClosestSearchWayPoint()
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


    /// <summary>
    /// Rotates the turret to the right
    /// </summary>
    public void SpinTurret()
    {
        Transform turret = transform.Find("Model/Turret");
        Vector3 target = turret.forward + turret.right;
        a_FaceTurretToPoint(CreateWaypoint(transform.TransformPoint(target), "Spin Target"));
    }

    public GameObject CreateWaypoint(Vector3 position) => CreateWaypoint(position, "Waypoint", Time.deltaTime);
    public GameObject CreateWaypoint(Vector3 position, string name) => CreateWaypoint(position, name, Time.deltaTime);
    public GameObject CreateWaypoint(Vector3 position, float duration) => CreateWaypoint(position, "Waypoint", duration);
    
    /// <summary>
    /// Creates a wapoint GameObject at a position in world space. Destroys it after a duration.
    /// </summary>
    /// <param name="position">The position to spawn the GameObject.</param>
    /// <param name="name">The name of the GameObject.</param>
    /// <param name="duration">The duration the GameObject should exist for.</param>
    /// <returns>The waypoint GameObject.</returns>
    public GameObject CreateWaypoint(Vector3 position, string name, float duration)
    {
        GameObject waypoint = new GameObject(name);
        waypoint.transform.position = position;
        Destroy(waypoint, duration);
        return waypoint;
    }
}
