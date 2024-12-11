using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Represents a state where the AI-controlled tank searches for the enemy, prioritising the last known enemy position. 
/// Follows a set path if the enemy is not found at the last known position.
/// </summary>
[CreateAssetMenu(menuName = "AI/States/Search State")]
public class CAD_SearchState : CAD_State
{
    /// <summary>
    /// Holds the dedicated Patrol Points in Unity
    /// </summary>
    [SerializeField] private Vector3[] m_PatrolPoints;
    /// <summary>
    /// Holds the size of the Waiting Period
    /// </summary>
    [SerializeField] private float m_WaitPeriod = 10.0f;
    /// <summary> 
    /// Holds the time in seconds since the last random path finding target was generated.
    /// </summary>
    private float m_CurrentTime;
    /// <summary>
    /// Holds the current selected point
    /// </summary>
    private GameObject m_CurrentPoint;
    /// <summary>
    /// Holds the current index from the Patrol Points
    /// </summary>
    private int m_CurrentIndex = 0;
    /// <summary>
    /// Holds the starting index from the Patrol Points
    /// </summary>
    private int m_StartIndex = 0;
    /// <summary>
    /// Holds the variable for when the tank should wait
    /// </summary>
    private bool m_Waiting = false;

    /// <summary>
    /// Called when the state is entered. Initializes time tracking for the state.
    /// </summary>
    /// <param name="tankAI">The SmartTank instance entering the state.</param>
    public override void OnStateEnter(CAD_SmartTankFSM tankAI)
    {
        //Resets the appropriate variables on state enter
        m_CurrentTime = 0;
        m_Waiting = false;
        // Makes sure it starts at the closest waypoint
        float beststart = float.PositiveInfinity;
        for (int i = 0; i < m_PatrolPoints.Length; i++)
        {
            float currentstart = Vector3.Distance(tankAI.transform.position, m_PatrolPoints[i]);
            if (currentstart < beststart)
            {
                m_CurrentIndex = i;
                beststart = currentstart;
            }
        }
        //sets the starting index to the current closest ones
        m_StartIndex = m_CurrentIndex;
        //Creates a waypoint gameobject at the intended patrol point
        m_CurrentPoint = tankAI.CreateWaypoint(m_PatrolPoints[m_CurrentIndex]);
        //Calls the Coroutine to update the patrol points asynchronously 
        tankAI.StartCoroutine(UpdatePatrolPoint(tankAI));
    }

    /// <summary>
    /// The tank will search for the last known enemy position,
    /// approach consumables, or move to a random world point if neither are found.
    /// </summary>
    /// <param name="tankAI">The SmartTank instance running the StateMachine.</param>
    public override void OnStateUpdate(CAD_SmartTankFSM tankAI)
    {
        //Checks if there is a last known enemy position
        if (tankAI.LastKnownEnemyPos)
        {
            //Checks if the tank is at the last known enemy position and deletes it if it is 
            if (Vector3.Distance(tankAI.transform.position, tankAI.LastKnownEnemyPos.transform.position) < 5.0f)
            {
                Destroy(tankAI.LastKnownEnemyPos);
                tankAI.LastKnownEnemyPos = null;
            }
            //If there is a last known enemy position makes the tank move towards it
            tankAI.FollowPathToWorldPoint(tankAI.LastKnownEnemyPos, 1f);
        }
        //Checks if the tank is waiting
        else if (!m_Waiting)
        {
            //If the tank is not waiting, the tank moves to the current patrol point
            tankAI.FollowPathToWorldPoint(m_CurrentPoint, 1f);
        }
        else
        {
            //Makes the tank stop
            tankAI.StopTank();
        }
    }

    public override void OnStateExit(CAD_SmartTankFSM tankAI)
    {
        // TODO: Implement OnStateExit
    }

    /// <summary>
    /// Updates the Patrol Points so the tank may go through the list
    /// </summary>
    /// <param name="tankAI"></param>
    /// <returns></returns>
    private IEnumerator UpdatePatrolPoint(CAD_SmartTankFSM tankAI)
    {
        while (true)
        {
            //Checks if the tank is within a certain area of the designated patrol point
            if (Vector3.Distance(tankAI.transform.position, m_CurrentPoint.transform.position) < 25.0f)
            {
                //Increases the current index in the list
                m_CurrentIndex++;
                //Checks if the current index is outside the bounds of the list, resetting it should it be true
                if (m_CurrentIndex >= m_PatrolPoints.Count())
                {
                    m_CurrentIndex = 0;
                }
                //Checks if the current index is equal to the start index
                if (m_CurrentIndex == m_StartIndex)
                {
                    //Makes the tank wait in place
                    m_Waiting = true;
                    yield return new WaitForSeconds(m_WaitPeriod);
                    m_Waiting = false;
                }
                //Destroys the current waypoint so the game objects dont stack up
                Destroy(m_CurrentPoint);
                //Makes a new waypoint for the tank to follow, essentially updating it
                m_CurrentPoint = tankAI.CreateWaypoint(m_PatrolPoints[m_CurrentIndex]);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// Creates a list of transitions for this state. Called when the ScriptableObject becomes enabled and active.
    /// Should the tank's health or fuel be too low we move to the retreat state
    /// Should the tank's ammo be too low we move to the resource gathering state
    /// Should the tank find the enemy we move to the chase state
    /// Should the tank find the enemy base we move to te attack base state
    /// </summary>
    private void OnEnable()
    {
        Transitions = new()
        {
            new CAD_Transition("Low Health or Fuel", tankAI => tankAI.Health <= 30.0f || tankAI.Fuel <= 50.0f),
            new CAD_Transition("Low Ammo", tankAI => tankAI.Ammo == 0.0f),
            new CAD_Transition("Tank Found", tankAI => tankAI.EnemyTank),
            new CAD_Transition("Base Found", tankAI => tankAI.VisibleEnemyBases.Count > 0)
        };
    }
}
