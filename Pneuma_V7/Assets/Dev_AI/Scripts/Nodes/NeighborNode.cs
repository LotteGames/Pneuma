using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class NeighborNode
{
    public Node neightbor;

    public float distance;

    public void CalculateDistance(Node node)
    {
        Vector3 vector = (neightbor.transform.position - node.transform.position);
        distance = Mathf.Pow((vector.x * vector.x) + (vector.y * vector.y), 0.5f);
    }

}