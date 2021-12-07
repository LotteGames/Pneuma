using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
public class Node : MonoBehaviour
{
    public float cost_G;
    public float Cost_G
    {
        get
        {
            return cost_G;
        }
        set
        {
            cost_G = value;
        }
    }

    [Space(20)]
    [SerializeField]
    private int nodeNum = -1;
    public int NodeNum
    {
        get
        {
            return nodeNum;
        }
        set
        {
            Debug.Log(5);
            nodeNum = value;
        }
    }

    public int parentNum = -1;
    public Vector3 Position
    {
        get
        {
            return transform.position;
        }
    }

    [Space(20)]
    public List<NeighborNode> neighborNodes = new List<NeighborNode>();

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Handles.Label(Position + Vector3.up * 1.5f, nodeNum.ToString());
#endif
        if (neighborNodes.Count != 0)
        {
            for (int i = 0; i < neighborNodes.Count; i++)
            {
                Node neighbor = neighborNodes[i].neightbor;

                if (neighbor != null)
                {
                    Gizmos.color = Color.white;

                    Gizmos.DrawLine(Position, neighbor.Position);
#if UNITY_EDITOR
                    Handles.color = Color.white;
                    //Handles.Label((Position + neighbor.Position) / 2, neighborNodes[i].distance.ToString("#0.00"));
#endif
                    //DrawArrow(Position, neighbor.Position);
                    DrawArrow(this, neighbor);
                }
            }
        }
    }
    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(Position, 0.75f);



        Gizmos.color = Color.red;

        if (neighborNodes.Count != 0)
        {
            for (int i = 0; i < neighborNodes.Count; i++)
            {
                Node neighbor = neighborNodes[i].neightbor;

                if (neighbor != null)
                {
                    Gizmos.DrawWireCube(neighbor.Position, Vector3.one);
                }
            }
        }
    }

    public void GetNeighborsDistance()
    {
        for (int i = 0; i < neighborNodes.Count; i++)
        {
            neighborNodes[i].CalculateDistance(this);
        }
    }
    public void SetName()
    {
        if (!GetComponent<AI_Move>())
            gameObject.name = "Node_" + nodeNum;
        //Debug.Log("Node Set Name");
    }

    public void DrawArrow(Vector3 pos, Vector3 targetPos)
    {
        Gizmos.color = Color.red;

        //Gizmos.DrawLine(pos, targetPos);

        Vector3 middle = (pos + targetPos) / 2;

        Vector3 vector = pos - targetPos;

        Vector3 right = Quaternion.AngleAxis(30, new Vector3(0, 0, 1)) * vector.normalized;

        Vector3 left = Quaternion.AngleAxis(-30, new Vector3(0, 0, 1)) * vector.normalized;


        //Gizmos.DrawLine(middle, middle + right);
        //Gizmos.DrawLine(middle, middle + left);
        Gizmos.DrawLine(pos + -vector / 3, pos + -vector / 3 + right);
        Gizmos.DrawLine(pos + -vector / 3, pos + -vector / 3 + left);
    }
    /// <summary>
    /// will draw in only one way path
    /// </summary>
    /// <param name="current"></param>
    /// <param name="neighbor"></param>
    public void DrawArrow(Node current, Node neighbor)
    {
        if ((current.IsContainNeighbor(neighbor) && neighbor.IsContainNeighbor(current))==false)
        {
            Gizmos.color = Color.red;

            //Gizmos.DrawLine(pos, targetPos);

            Vector3 middle = (current.Position + neighbor.Position) / 2;

            Vector3 vector = current.Position - neighbor.Position;

            Vector3 right = Quaternion.AngleAxis(30, new Vector3(0, 0, 1)) * vector.normalized;

            Vector3 left = Quaternion.AngleAxis(-30, new Vector3(0, 0, 1)) * vector.normalized;


            //Gizmos.DrawLine(middle, middle + right);
            //Gizmos.DrawLine(middle, middle + left);
            Gizmos.DrawLine(middle, middle + right);
            Gizmos.DrawLine(middle, middle + left);
        }
    }

    public bool IsContainNeighbor(Node testNode)
    {
        for (int i = 0; i < neighborNodes.Count; i++) 
        {
            if (neighborNodes[i].neightbor == testNode) 
            {
                return true;
            }
        }
        return false;
    }
}
