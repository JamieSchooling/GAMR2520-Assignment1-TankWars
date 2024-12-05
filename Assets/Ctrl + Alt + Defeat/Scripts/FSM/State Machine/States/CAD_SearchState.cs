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
    [SerializeField] private Vector3[] m_PatrolPoints;
    [SerializeField] private float m_WaitPeriod = 10.0f;
    /// <summary> 
    /// Holds the time in seconds since the last random path finding target was generated.
    /// </summary>
    private float m_CurrentTime;
    private GameObject m_CurrentPoint;
    private int m_CurrentIndex = 0;
    private int m_StartIndex = 0;
    private bool m_Waiting = false;

    /// <summary>
    /// Called when the state is entered. Initializes time tracking for the state.
    /// </summary>
    /// <param name="tankAI">The SmartTank instance entering the state.</param>
    public override void OnStateEnter(CAD_SmartTankFSM tankAI)
    {
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
        m_StartIndex = m_CurrentIndex;
        m_CurrentPoint = tankAI.CreateWaypoint(m_PatrolPoints[m_CurrentIndex]);
        tankAI.StartCoroutine(UpdatePatrolPoint(tankAI));
    }

    /// <summary>
    /// The tank will search for the last known enemy position,
    /// approach consumables, or move to a random world point if neither are found.
    /// </summary>
    /// <param name="tankAI">The SmartTank instance running the StateMachine.</param>
    public override void OnStateUpdate(CAD_SmartTankFSM tankAI)
    {
        if (tankAI.LastKnownEnemyPos)
        {
            if (Vector3.Distance(tankAI.transform.position, tankAI.LastKnownEnemyPos.transform.position) < 5.0f)
            {
                Destroy(tankAI.LastKnownEnemyPos);
                tankAI.LastKnownEnemyPos = null;
            }
            tankAI.FollowPathToWorldPoint(tankAI.LastKnownEnemyPos, 1f);
        }
        else if (!m_Waiting)
        {
            tankAI.FollowPathToWorldPoint(m_CurrentPoint, 1f);
        }
        else
        {
            tankAI.StopTank();
        }
    }

    public override void OnStateExit(CAD_SmartTankFSM tankAI)
    {
        // TODO: Implement OnStateExit
    }

    private IEnumerator UpdatePatrolPoint(CAD_SmartTankFSM tankAI)
    {
        while (true)
        {
            if (Vector3.Distance(tankAI.transform.position, m_CurrentPoint.transform.position) < 25.0f)
            {
                m_CurrentIndex++;
                if (m_CurrentIndex >= m_PatrolPoints.Count())
                {
                    m_CurrentIndex = 0;
                }
                if (m_CurrentIndex == m_StartIndex)
                {
                    m_Waiting = true;
                    yield return new WaitForSeconds(m_WaitPeriod);
                    m_Waiting = false;
                }

                Destroy(m_CurrentPoint);
                m_CurrentPoint = tankAI.CreateWaypoint(m_PatrolPoints[m_CurrentIndex]);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// Creates a list of transitions for this state. Called when the ScriptableObject becomes enabled and active.
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
