using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a state where the AI-controlled tank searches for consumables until at high enough stats.
/// </summary>
[CreateAssetMenu(menuName = "AI/States/Resource Gathering State")]

public class CAD_Resource_Gathering_State : CAD_State
{
    /// <summary>
    /// Holds all the positions for the resource spawn points.
    /// </summary>
    [SerializeField] private Vector3[] m_ResourceWaypoints;

    /// <summary>
    /// Holds the current waypoint game object
    /// </summary>
    private GameObject m_CurrentWaypoint;

    /// <summary>
    /// Holds the waypoint index (where in the list of waypoints)
    /// </summary>
    private int m_WaypointIndex = 0;

    /// <summary>
    /// Holds the current move speed
    /// </summary>
    private float m_CurrentMoveSpeed = 1.0f;

    /// <summary>
    /// Ensures the tank first goes to the closest waypoint, ans starts the update waypoint coroutine.
    /// </summary>
    /// <param name="tankAI"></param>
    public override void OnStateEnter(CAD_SmartTankFSM tankAI)
    {
        //Makes sure it starts at the closest resource waypoint
        float closestDistance = float.PositiveInfinity;
        for (int i = 0; i < m_ResourceWaypoints.Length; i++)
        {
            float currentDistance = Vector3.Distance(tankAI.transform.position, m_ResourceWaypoints[i]);
            if (currentDistance < closestDistance)
            {
                m_WaypointIndex = i;
                closestDistance = currentDistance;
            }
        }
        //Creates a waypoint gameobject at the intended patrol point
        m_CurrentWaypoint = tankAI.CreateWaypoint(m_ResourceWaypoints[m_WaypointIndex]);
        //Calls the Coroutine to update the waypoints asynchronously 
        tankAI.StartCoroutine(UpdateWaypoint(tankAI));
    }

    /// <summary>
    /// Determines what consumables to find based on current resource levels.
    /// Prioritises Health as long as Fuel can last, the Fuel, then Ammo.
    /// Ignores Health if it is max
    /// Ignores Ammo when above 5 shots
    /// </summary>
    /// <param name="tankAI">The SmartTank instance running the StateMachine.</param>
    public override void OnStateUpdate(CAD_SmartTankFSM tankAI)
    {
        List<string> consumablesToFind = new();

        if (tankAI.Health != 125.0f)
        {
            if (tankAI.Fuel >= 40.0f)
            {
                consumablesToFind.Add("Health");
            }
            else
            {
                consumablesToFind.Add("Fuel");
            }
            if (!consumablesToFind.Contains("Health"))
            {
                consumablesToFind.Add("Health");
            }
            if (!consumablesToFind.Contains("Fuel"))
            {
                consumablesToFind.Add("Fuel");
            }
        }
        else
        {
            consumablesToFind.Add("Fuel");
        }
        if (tankAI.Ammo <= 5.0f)
        {
            consumablesToFind.Add("Ammo");
        }

        //If fuel is too low, the tank slows down to use less
        m_CurrentMoveSpeed = (tankAI.Fuel >= 40) ? 1.0f : 0.5f;
        //If fuel is extremely low, the tank stops moving
        m_CurrentMoveSpeed = (tankAI.Fuel >= 10) ? m_CurrentMoveSpeed : 0.0f;
        //calls the find cosumables function
        FindConsumables(tankAI, consumablesToFind);
    }


    /// <summary>
    /// Finds the closest consumable out of the required types, and if none are found, follows a path to a random world point.
    /// </summary>
    /// <param name="tankAI">The SmartTank instance running the StateMachine.</param>
    /// <param name="consumableTypes">List of tags corresponding to the types of consumables needing found.</param>
    private void FindConsumables(CAD_SmartTankFSM tankAI, List<string> consumableTypes)
    {
        //Checks if there are any visible consumables
        if (tankAI.VisibleConsumables.Count > 0)
        {
            // Filter consumables matching the required types
            var potentialConsumables = tankAI.VisibleConsumables
                .Where(c => consumableTypes.Any(type => c.Key.CompareTag(type)))
                .OrderBy(c => c.Value) // Order by distance
                .ToList();
            //Checks if there are any potential cosumables based on what is needed
            if (potentialConsumables.Count > 0)
            {
                // Get the closest consumable
                GameObject consumable = potentialConsumables.First().Key;
                //Move towards the closest cosumable
                tankAI.FollowPathToWorldPoint(consumable, m_CurrentMoveSpeed);
            }
            else
            {
                //Move towards the next waypoint
                tankAI.FollowPathToWorldPoint(m_CurrentWaypoint, m_CurrentMoveSpeed);
            }
        }
        else
        {
            //Move towards the next waypoint
            tankAI.FollowPathToWorldPoint(m_CurrentWaypoint, m_CurrentMoveSpeed);
        }
    }

    public override void OnStateExit(CAD_SmartTankFSM tankAI)
    {
        // TODO: Implement OnStateExit
    }

    /// <summary>
    /// Updates the waypoint index asynchronously
    /// </summary>
    /// <param name="tankAI">The SmartTank instance entering the state.</param>
    /// <returns></returns>
    private IEnumerator UpdateWaypoint(CAD_SmartTankFSM tankAI)
    {
        while (true)
        {
            //Checks if the tank is within a certain area of the designated waypoint
            if (Vector3.Distance(tankAI.transform.position, m_CurrentWaypoint.transform.position) < 25.0f)
            {
                //Increases the current index in the list
                m_WaypointIndex++;
                //Checks if the current index is outside the bounds of the list, resetting it should it be true
                if (m_WaypointIndex >= m_ResourceWaypoints.Count())
                {
                    m_WaypointIndex = 0;
                }
                //Destroys the current waypoint so the game objects dont stack up
                Destroy(m_CurrentWaypoint);
                //Makes a new waypoint for the tank to follow, essentially updating it
                m_CurrentWaypoint = tankAI.CreateWaypoint(m_ResourceWaypoints[m_WaypointIndex]);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// Creates list of transitions for this state. Called when the ScriptableObject becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        Transitions = new()
        {
            new CAD_Transition("Enough Resources", tankAI => tankAI.Health >= 60.0f && tankAI.Ammo >= 5.0f && tankAI.Fuel >= 70.0f),
            new CAD_Transition("Enough to Attack Enemy", tankAI => ((tankAI.Health >= 60.0f && tankAI.Ammo > 0.0f 
            && tankAI.Fuel > 50.0f) || tankAI.Fuel <= 10.0f) && tankAI.EnemyTank),
            new CAD_Transition("Enough to Attack Base", tankAI => tankAI.Health > 20.0f && tankAI.Ammo > 0.0f
            && tankAI.Fuel > 50.0f && tankAI.VisibleEnemyBases.Count > 0.0f),
            new CAD_Transition("Retreat for Safety", tankAI => tankAI.EnemyTank && tankAI.Fuel >= 70.0f)
        };
    }
}
