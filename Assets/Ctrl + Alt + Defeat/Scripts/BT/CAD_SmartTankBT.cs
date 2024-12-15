using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Represents an AI-controlled tank that uses a Behavior Tree for decision making.
/// </summary>
public class CAD_SmartTankBT : AITank
{
    /// <summary>
    /// The Behavior Tree governing this tank's AI.
    /// </summary>
    [SerializeField] private CAD_BehaviourTree m_BehaviourTree;

    /// <summary>
    /// An array of waypoints used for patrolling or searching.
    /// </summary>
    [SerializeField] private Vector3[] m_SearchWaypoints;

    /// <summary>
    /// The tank's current fuel level.
    /// </summary>
    public float FuelLevel => a_GetFuelLevel;

    /// <summary>
    /// The tank's current health level.
    /// </summary>
    public float HealthLevel => a_GetHealthLevel;

    /// <summary>
    /// The tank's current ammo level.
    /// </summary>
    public float AmmoLevel => a_GetAmmoLevel;

    /// <summary>
    /// A dictionary of all visible enemy tanks and their distances.
    /// </summary>
    public Dictionary<GameObject, float> TanksFound => a_TanksFound;

    /// <summary>
    /// A dictionary of all visible enemy bases and their distances.
    /// </summary>
    public Dictionary<GameObject, float> BasesFound => a_BasesFound;

    /// <summary>
    /// A list of all friendly bases in the game.
    /// </summary>
    public List<GameObject> FriendlyBases => a_GetMyBases;

    /// <summary>
    /// A dictionary of all visible consumables and their distances.
    /// </summary>
    public Dictionary<GameObject, float> ConsumablesFound => a_ConsumablesFound;

    /// <summary>
    /// The array of waypoints this tank is using for navigation.
    /// </summary>
    public Vector3[] Waypoints => m_SearchWaypoints;

    /// <summary>
    /// The current target waypoint or the last known enemy position.
    /// </summary>
    public Vector3 CurrentWaypoint => LastKnownEnemyPos == Vector3.zero ? Waypoints[CurrentWaypointIndex] : LastKnownEnemyPos;

    /// <summary>
    /// The index of the current waypoint in the array.
    /// </summary>
    public int CurrentWaypointIndex { get; set; } = 0;

    /// <summary>
    /// The last known position of an enemy, used for navigation.
    /// </summary>
    public Vector3 LastKnownEnemyPos { get; set; } = Vector3.zero;

    /// <summary>
    /// Starts the tank's AI by initializing the behavior tree.
    /// </summary>
    public override void AITankStart()
    {
        m_BehaviourTree.Start();
    }

    /// <summary>
    /// Updates the tank's AI by executing the behavior tree's root node.
    /// </summary>
    public override void AITankUpdate()
    {
        m_BehaviourTree.Root.Execute(this);
    }

    /// <summary>
    /// Called when the tank collides with another object.
    /// </summary>
    /// <param name="collision">The collision data.</param>
    public override void AIOnCollisionEnter(Collision collision)
    {
        //throw new NotImplementedException();
    }

    /// <summary>
    /// Commands the tank to attack a specific position.
    /// </summary>
    /// <param name="position">The target position to attack.</param>
    public void Attack(Vector3 position)
    {
        float rand = UnityEngine.Random.value;
        GameObject target = CreateWaypoint(rand > 0.8 ? position : TanksFound.First().Key.transform.position, "Target");
        a_FaceTurretToPoint(target);
        a_FireAtPoint(target);
    }

    /// <summary>
    /// Commands the tank to move to a specific position at a specified speed.
    /// </summary>
    /// <param name="position">The target position to move to.</param>
    /// <param name="speed">The movement speed.</param>
    public void GoTo(Vector3 position, float speed = 1.0f)
    {
        GameObject waypoint = CreateWaypoint(position, "Waypoint");
        a_FollowPathToPoint(waypoint, speed);
    }

    /// <summary>
    /// Rotates the turret in a spinning motion.
    /// </summary>
    public void SpinTurret()
    {
        Transform turret = transform.Find("Model/Turret");
        Vector3 target = turret.forward + turret.right;
        a_FaceTurretToPoint(CreateWaypoint(transform.TransformPoint(target), "Spin Target"));
    }

    /// <summary>
    /// Creates a waypoint GameObject at a specific position.
    /// </summary>
    /// <param name="position">The position of the waypoint.</param>
    /// <returns>The created waypoint GameObject.</returns>
    public GameObject CreateWaypoint(Vector3 position) => CreateWaypoint(position, "Waypoint", Time.deltaTime);

    /// <summary>
    /// Creates a waypoint GameObject at a specific position with a custom name.
    /// </summary>
    /// <param name="position">The position of the waypoint.</param>
    /// <param name="name">The name of the waypoint.</param>
    /// <returns>The created waypoint GameObject.</returns>
    public GameObject CreateWaypoint(Vector3 position, string name) => CreateWaypoint(position, name, Time.deltaTime);

    /// <summary>
    /// Creates a waypoint GameObject at a specific position for a set duration.
    /// </summary>
    /// <param name="position">The position of the waypoint.</param>
    /// <param name="duration">The duration for which the waypoint exists.</param>
    /// <returns>The created waypoint GameObject.</returns>
    public GameObject CreateWaypoint(Vector3 position, float duration) => CreateWaypoint(position, "Waypoint", duration);

    /// <summary>
    /// Creates a waypoint GameObject at a specific position with a custom name and duration.
    /// </summary>
    /// <param name="position">The position of the waypoint.</param>
    /// <param name="name">The name of the waypoint.</param>
    /// <param name="duration">The duration for which the waypoint exists.</param>
    /// <returns>The created waypoint GameObject.</returns>
    public GameObject CreateWaypoint(Vector3 position, string name, float duration)
    {
        GameObject waypoint = new GameObject(name);
        waypoint.transform.position = position;
        Destroy(waypoint, duration);
        return waypoint;
    }
}
