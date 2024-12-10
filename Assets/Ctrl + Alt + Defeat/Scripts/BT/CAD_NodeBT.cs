using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class CAD_NodeBT : XNode.Node
{
    /// <summary>
    /// Gets all child nodes connected to this node. 
    /// Sorts the child nodes by their vertical position in the graph.
    /// </summary>
    /// <returns>An ordered list of all child nodes.</returns>
    protected List<CAD_NodeBT> GetConnectedChildren()
    {
        if (!HasPort("m_Children"))
        {
            Debug.LogError("Can't retrieve connected nodes. Node is a leaf node.");
            return null;
        }

        List<CAD_NodeBT> nodes = new();
        foreach (var port in GetOutputPort("m_Children").GetConnections())
        {
            nodes.Add(port.node as CAD_NodeBT);
        }

        return nodes.OrderBy(n => n.position.y).ToList();
    }

    public abstract CAD_NodeStateBT Execute(CAD_SmartTankBT tankAI);
}

public enum CAD_NodeStateBT
{
    Success,
    Failure,
    Running
}