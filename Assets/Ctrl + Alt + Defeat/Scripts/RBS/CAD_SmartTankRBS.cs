using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CAD_SmartTankRBS : AITank
{
    [SerializeField] private CAD_Rules m_Rules;
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
        m_RulesEngine = new(this, m_Rules);
        m_KnowledgeBase = new(this);
        m_KnowledgeBase.CurrentSearchWaypoint = GetClosestSearchWayPoint();
        m_CurrentWaypointIndex = m_SearchWaypoints.ToList().IndexOf(m_KnowledgeBase.CurrentSearchWaypoint);
    }

    public override void AITankUpdate()
    {
        m_RulesEngine.Update(m_KnowledgeBase);
    }

    public override void AIOnCollisionEnter(Collision collision)
    {
        // TODO: Implement Collision Response
    }

    public void Attack(Vector3 position)
    {
        GameObject target = CreateWaypoint(position, "Target");
        a_FaceTurretToPoint(target);
        a_FireAtPoint(target);
    }

    public void GoTo(Vector3 position, float speed = 1.0f)
    {
        GameObject waypoint = CreateWaypoint(position, "Waypoint");
        a_FollowPathToPoint(waypoint, speed);
    }

    public void NextSearchWaypoint()
    {
        m_CurrentWaypointIndex++;
        if (m_CurrentWaypointIndex >= m_SearchWaypoints.Count())
        {
            m_CurrentWaypointIndex = 0;
        }
        m_KnowledgeBase.CurrentSearchWaypoint = m_SearchWaypoints[m_CurrentWaypointIndex];
    }

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

    public void SpinTurret()
    {
        Transform turret = transform.Find("Model/Turret");
        Vector3 target = turret.forward + turret.right;
        a_FaceTurretToPoint(CreateWaypoint(transform.TransformPoint(target), "Spin Target"));
    }

    public GameObject CreateWaypoint(Vector3 position) => CreateWaypoint(position, "Waypoint", Time.deltaTime);
    public GameObject CreateWaypoint(Vector3 position, string name) => CreateWaypoint(position, name, Time.deltaTime);
    public GameObject CreateWaypoint(Vector3 position, float duration) => CreateWaypoint(position, "Waypoint", duration);
    public GameObject CreateWaypoint(Vector3 position, string name, float duration)
    {
        GameObject waypoint = new GameObject(name);
        waypoint.transform.position = position;
        Destroy(waypoint, duration);
        return waypoint;
    }
}
