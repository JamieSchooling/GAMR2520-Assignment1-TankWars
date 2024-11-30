using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CAD
{
    public class SmartTank : AITank
    {
        private IState m_CurrentState;

        private SearchState m_SearchState = new();
        private ChaseState m_ChaseState = new();
        private AttackState m_AttackState = new();
        private RetreatState m_RetreatState = new();

        public Dictionary<GameObject, float> VisibleConsumables => a_ConsumablesFound;

        public void GenerateNewRandomWorldPoint()
        {
            a_GenerateRandomPoint();
        }

        public void FollowPathToWorldPoint(GameObject pointInWorld, float normalizedSpeed)
        {
            a_FollowPathToPoint(pointInWorld, normalizedSpeed);
        }

        public void FollowPathToRandomWorldPoint(float normalizedSpeed)
        {
            a_FollowPathToRandomPoint(normalizedSpeed);
        }

        public override void AITankStart()
        {
            m_CurrentState = m_SearchState;
        }

        public override void AITankUpdate()
        {
            m_CurrentState.OnStateUpdate(this);

            //if (a_TanksFound.Count == 0 || !a_TanksFound.First().Key)
            //{
            //    SwitchState(m_SearchState);
            //}
            //if (a_TanksFound.Count > 0 && a_TanksFound.First().Key)
            //{
            //    SwitchState(m_ChaseState);
            //}
            //if (m_CurrentState == m_ChaseState && Vector3.Distance(transform.position, a_TanksFound.First().Key.transform.position) < 25.0f)
            //{
            //    SwitchState(m_AttackState);
            //}
            //if (a_GetHealthLevel < 30.0f || a_GetAmmoLevel < 4.0f || a_GetFuelLevel < 50.0f)
            //{
            //    SwitchState(m_RetreatState);
            //}
        }

        public override void AIOnCollisionEnter(Collision collision)
        {
            // TODO: Implement Collision Response
        }

        private void SwitchState(IState state)
        {
            if (m_CurrentState == state) return;

            m_CurrentState.OnStateExit(this);
            m_CurrentState = state;
            m_CurrentState.OnStateEnter(this);
        }
    }
}
