using System;
using System.Linq;
using UnityEngine;

public class CAD_SmartTankRBS : AITank
{
    private CAD_RulesEngine m_RulesEngine;

    public override void AITankStart()
    {
        m_RulesEngine = new(this);
    }

    public override void AITankUpdate()
    {
        // TODO: Implement Tank Update
    }


    public override void AIOnCollisionEnter(Collision collision)
    {
        // TODO: Implement Collision Response
    }

}
