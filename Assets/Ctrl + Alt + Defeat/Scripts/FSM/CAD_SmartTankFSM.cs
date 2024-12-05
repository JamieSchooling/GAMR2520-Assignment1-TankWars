using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Main Tank AI class that controls tank AI behaviour. Contains wrapper properties for accessing tank info, and wrapper methods for using certain AI functionality.
/// </summary>
public class CAD_SmartTankFSM : AITank
{
    /// <summary>
    /// State machine graph that contains the intended behaviour of the AI.
    /// </summary>
    [SerializeField] private CAD_StateMachine m_StateMachine;

    /// <returns>
    /// Dictionary(GameObject consumable, float distance) of visible consumables (consumables in Consumable LayerMask).
    /// </returns>
    public Dictionary<GameObject, float> VisibleConsumables => a_ConsumablesFound;

    /// <returns>
    /// The enemy tank GameObject when found, otherwise returns null.
    /// </returns>
    public GameObject EnemyTank
    {
        get
        {
            if (a_TanksFound.Count > 0) return a_TanksFound.First().Key;
            return null;
        }
    }

    /// <summary>
    /// Returns Dictionary(GameObject base, float distance) of visible enemy bases (bases in Base LayerMask).
    /// </summary>
    /// <returns>All enemy bases currently visible.</returns>
    public Dictionary<GameObject, float> VisibleEnemyBases
    {
        get
        {
            return a_BasesFound;
        }
    }

    /// <summary>
    /// Returns Dictionary(GameObject base, float distance) of friendly bases (bases in Base LayerMask).
    /// </summary>
    /// <returns>All enemy bases currently visible.</returns>
    public Dictionary<GameObject, float> FriendlyBases
    {
        get
        {
            Dictionary<GameObject, float> DistanceFriendlyBases = new Dictionary<GameObject, float>();
            foreach (GameObject b in a_GetMyBases) 
            {
                if (!b) continue;
                float distance = Vector3.Distance(transform.position, b.transform.position);
                DistanceFriendlyBases.Add(b, distance);
            }
            return DistanceFriendlyBases;
        }
    }
    /// <summary>
    /// Property for retrieving tank's last known enemy position. Replaces the current GameObject when set.
    /// </summary>
    /// <returns> GameObject at last known enemy tank position.</returns>
    public GameObject LastKnownEnemyPos
    {
        get => m_LastKnownEnemyPos;
        set
        {
            Destroy(m_LastKnownEnemyPos);
            m_LastKnownEnemyPos = value;
        }
    }

    /// <summary>
    /// Property for retrieving tank's last known safest position. Replaces the current GameObject when set.
    /// </summary>
    /// <returns> GameObject at last known safest position.</returns>
    public GameObject LastKnownSafestPos
    {
        get => m_LastKnownSafestPos;
        set
        {
            Destroy(m_LastKnownSafestPos);
            m_LastKnownSafestPos = value;
        }
    }

    /// <returns>
    /// Current tank health.
    /// </returns>
    public float Health => a_GetHealthLevel;

    /// <returns>
    /// Current tank ammo count.
    /// </returns>
    public float Ammo => a_GetAmmoLevel;

    /// <returns>
    /// Current tank fuel level.
    /// </returns>
    public float Fuel => a_GetFuelLevel;

    /// <summary>
    /// GameObject at last known enemy tank position.
    /// </summary>
    private GameObject m_LastKnownEnemyPos = null;

    /// <summary>
    /// GameObject at last known safest position.
    /// </summary>
    private GameObject m_LastKnownSafestPos = null;

    /// <summary>
    /// Instance of StateMachine that reads a StateMachineGraph.
    /// </summary>
    private CAD_StateMachineProcessor m_StateMachineProcessor;

    /// <summary>
    /// Wrapper method for generating a random world point for pathfinding.
    /// </summary>
    public void GenerateNewRandomWorldPoint()
    {
        a_GenerateRandomPoint();
    }

    public GameObject CreateWaypoint(Vector3 position)
    {
        GameObject Waypoint = new GameObject("Waypoint");
        Waypoint.transform.position = position;
        return Waypoint;
    }

    /// <summary>
    /// Wrapper method for following a path to a world point.
    /// </summary>
    public void FollowPathToWorldPoint(GameObject pointInWorld, float normalizedSpeed)
    {
        a_FollowPathToPoint(pointInWorld, normalizedSpeed);
    }

    /// <summary>
    /// Wrapper method for following a path to a random world point.
    /// </summary>
    public void FollowPathToRandomWorldPoint(float normalizedSpeed)
    {
        a_FollowPathToRandomPoint(normalizedSpeed);
    }

    public void StopTank()
    {
        a_StopTank();
    }

    /// <summary>
    /// Wrapper method for firing at a point in the scene.
    /// </summary>
    public void TurretFireAtPoint(GameObject pointInWorld)
    {
        a_FireAtPoint(pointInWorld);
    }

    /// <summary>
    /// Called when AI Initialises. Used to Init StateMachine.
    /// </summary>
    public override void AITankStart()
    {
        InitialiseStateMachine();
    }

    /// <summary>
    /// Called every frame. Used to update StateMachine.
    /// </summary>
    public override void AITankUpdate()
    {
        m_StateMachineProcessor.Update();
    }

    /// <summary>
    /// Called when AI collides with something.
    /// </summary>
    /// <param name="collision"></param>
    public override void AIOnCollisionEnter(Collision collision)
    {
        // TODO: Implement Collision Response
    }

    /// <summary>
    /// Creates new instance of StateMachine, and starts it using the StateMachineGraph.
    /// </summary>
    private void InitialiseStateMachine()
    {
        m_StateMachineProcessor = new(this);

        m_StateMachineProcessor.Start(m_StateMachine);
    }

    private void OnDisable()
    {
        m_StateMachineProcessor?.End();
    }
}
