using System;

namespace CAD
{
    public class Transition
    {
        public string Name { get; private set; }
        public Func<SmartTank, bool> Condition { get; private set; }

        public Transition(string name, Func<SmartTank, bool> condition)
        {
            Name = name;
            Condition = condition;
        }
    }
}
