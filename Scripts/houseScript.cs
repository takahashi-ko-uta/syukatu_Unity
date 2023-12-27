using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class houseScript : MonoBehaviour
{
    public Vector2Int NodeId { get; private set; }

    public void SetNodeId(Vector2Int nodeId)
    {
        NodeId = nodeId;
    }
}
