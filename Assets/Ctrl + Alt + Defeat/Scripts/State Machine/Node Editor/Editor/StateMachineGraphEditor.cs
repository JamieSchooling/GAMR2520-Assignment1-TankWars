using UnityEngine;
using XNode;
using XNodeEditor;

namespace CAD
{
    [CustomNodeGraphEditor(typeof(StateMachine))]
    public class StateMachineGraphEditor : NodeGraphEditor
    {
        public override Gradient GetNoodleGradient(NodePort output, NodePort input)
        {
            Gradient grad = new Gradient();

            Color start = Color.red;
            Color end = Color.blue;

            if (window.hoveredPort == output || window.hoveredPort == input)
            {
                start = Color.Lerp(start, Color.white, 0.8f);
                end = Color.Lerp(end, Color.white, 0.8f);
            }
            grad.SetKeys(
                new GradientColorKey[] { new GradientColorKey(start, 0f), new GradientColorKey(end, 1f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) }
            );

            return grad;
        }

        /// <summary> Returns a color for the port based on the noodle's gradient </summary>
        public override Color GetPortColor(XNode.NodePort port)
        {
            Color defaultColor = Color.gray;

            if (!port.IsConnected) return defaultColor;

            NodePort connectedPort = port.Connection;

            Gradient noodleGradient = GetNoodleGradient(
                port.IsOutput ? port : connectedPort,
                port.IsOutput ? connectedPort : port
            );

            return port.IsOutput ? noodleGradient.Evaluate(0f) : noodleGradient.Evaluate(1f);
        }


        public override NoodlePath GetNoodlePath(NodePort output, NodePort input)
        {
            return NoodlePath.Straight;
        }
    }
}