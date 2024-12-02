using System;

/// <summary>
/// Represents an exit condition for a state.
/// </summary>
public class CAD_Transition
{
    /// <summary>
    /// The name of this transition. Used by the node editor to create and name an output port.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// A function that defines a condition to evaluate for a transition to occur.
    /// </summary>
    /// <value>
    /// A <see cref="Func{SmartTank, bool}"/> representing the condition. It takes a <see cref="CAD_SmartTank"/> 
    /// as an input and returns a bool indicating whether the condition is met.
    /// </value>
    public Func<CAD_SmartTank, bool> Condition { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CAD_Transition"/> class with the specified name and condition.
    /// </summary>
    /// <param name="name">
    /// The name of this transition. Used by the node editor to create and name an output port.
    /// </param>
    /// <param name="condition">
    /// A function that defines the condition to evaluate for the transition to occur. 
    /// It takes a <see cref="CAD_SmartTank"/> as input and returns a <see cref="bool"/> indicating whether the transition condition is met.
    /// </param>
    public CAD_Transition(string name, Func<CAD_SmartTank, bool> condition)
    {
        Name = name;
        Condition = condition;
    }
}
