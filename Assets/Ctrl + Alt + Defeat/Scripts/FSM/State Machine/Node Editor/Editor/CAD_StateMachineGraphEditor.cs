using UnityEngine;
using XNode;
using XNodeEditor;

/// <summary>
/// Custom editor for the CAD_StateMachine graph, providing a customized appearance and behavior for node connections ("noodles") in the editor.
/// </summary>
[CustomNodeGraphEditor(typeof(CAD_StateMachine))]
public class CAD_StateMachineGraphEditor : NodeGraphEditor
{
    /// <summary>
    /// Customizes the gradient for the noodles connecting nodes in the graph.
    /// The gradient changes when a port is hovered, blending the colors with white.
    /// </summary>
    /// <param name="output">The output port of the noodle.</param>
    /// <param name="input">The input port of the noodle.</param>
    /// <returns>A <see cref="Gradient"/> object defining the color transition of the noodle.</returns>
    public override Gradient GetNoodleGradient(NodePort output, NodePort input)
    {
        Gradient grad = new Gradient();

        // Default start and end colors for the noodle.
        Color start = Color.red;
        Color end = Color.blue;

        // Blend colors with white if the port is hovered.
        if (window.hoveredPort == output || window.hoveredPort == input)
        {
            start = Color.Lerp(start, Color.white, 0.8f);
            end = Color.Lerp(end, Color.white, 0.8f);
        }

        // Set gradient keys for the noodle.
        grad.SetKeys(
            new GradientColorKey[] { new GradientColorKey(start, 0f), new GradientColorKey(end, 1f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) }
        );

        return grad;
    }

    /// <summary>
    /// Determines the color of a port based on the noodle's gradient and its connection state.
    /// </summary>
    /// <param name="port">The port for which the color is determined.</param>
    /// <returns>
    /// The color of the port:
    /// - Gray if the port is not connected.
    /// - The gradient's starting color if the port is an output port.
    /// - The gradient's ending color if the port is an input port.
    /// </returns>
    public override Color GetPortColor(XNode.NodePort port)
    {
        Color defaultColor = Color.gray;

        // Return gray if the port is not connected.
        if (!port.IsConnected) return defaultColor;

        NodePort connectedPort = port.Connection;

        // Determine the noodle gradient based on the port's direction.
        Gradient noodleGradient = GetNoodleGradient(
            port.IsOutput ? port : connectedPort,
            port.IsOutput ? connectedPort : port
        );

        // Return the appropriate color based on the port's direction.
        return port.IsOutput ? noodleGradient.Evaluate(0f) : noodleGradient.Evaluate(1f);
    }

    /// <summary>
    /// Determines the path style for the noodles connecting nodes in the graph.
    /// </summary>
    /// <param name="output">The output port of the noodle.</param>
    /// <param name="input">The input port of the noodle.</param>
    /// <returns>A <see cref="NoodlePath"/> representing the path style. Always returns a straight path.</returns>
    public override NoodlePath GetNoodlePath(NodePort output, NodePort input)
    {
        return NoodlePath.Straight;
    }
}
