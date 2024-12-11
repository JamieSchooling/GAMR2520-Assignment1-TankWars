using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CAD_SmartTankBT : AITank
{
    [SerializeField] private CAD_BehaviourTree m_BehaviourTree;
    [SerializeField] private Vector3[] m_SearchWaypoints;

    public float FuelLevel => a_GetFuelLevel;
    public float HealthLevel => a_GetHealthLevel;
    public float AmmoLevel => a_GetAmmoLevel;
    public Dictionary<GameObject, float> TanksFound => a_TanksFound;
    public Dictionary<GameObject, float> BasesFound => a_BasesFound;
    public List<GameObject> FriendlyBases => a_GetMyBases;
    public Dictionary<GameObject, float> ConsumablesFound => a_ConsumablesFound;
    public Vector3[] Waypoints => m_SearchWaypoints;
    public Vector3 CurrentWaypoint => LastKnownEnemyPos == Vector3.zero ? Waypoints[CurrentWaypointIndex] : LastKnownEnemyPos;
    public int CurrentWaypointIndex { get; set; } = 0;
    public Vector3 LastKnownEnemyPos { get; set; } = Vector3.zero;

    public override void AITankStart()
    {
        m_BehaviourTree.Start();
    }

    public override void AITankUpdate()
    {
        m_BehaviourTree.Root.Execute(this);
    }

    public override void AIOnCollisionEnter(Collision collision)
    {
        throw new NotImplementedException();
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

    private CAD_NodeStateBT Attack(CAD_SmartTankBT tankAI)
    {
        Vector3 position = a_TanksFound.First().Key.transform.position;
        GameObject target = CreateWaypoint(position, "Target");
        a_FaceTurretToPoint(target);
        a_FireAtPoint(target);

        return CAD_NodeStateBT.Success;
    }
}
