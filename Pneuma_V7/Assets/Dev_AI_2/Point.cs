using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Point : MonoBehaviour
{
    public int num;

    [HideInInspector]
    public int parent;

    [HideInInspector]
    public float cost;

    public List<Neighbor> neighbors = new List<Neighbor>();

    public Vector3 position_World
    {
        get
        {
            return transform.position;
        }
    }

    public void SetName()
    {
        gameObject.name = "Point_" + num;
    }
    private void OnDrawGizmos()
    {

#if UNITY_EDITOR

        Handles.Label(position_World + Vector3.up * 1.5f, num.ToString());

#endif

        if (neighbors.Count != 0)
        {
            for (int i = 0; i < neighbors.Count; i++)
            {
                Point neighbor = neighbors[i].point;

                if (neighbor != null)
                {
                    Gizmos.color = Color.white;

                    Gizmos.DrawLine(position_World, neighbor.position_World);
#if UNITY_EDITOR
                    Handles.color = Color.white;
#endif
                    DrawArrow(neighbor);
                }
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(position_World, 0.75f);


        Gizmos.color = Color.red;

        if (neighbors.Count != 0)
        {
            for (int i = 0; i < neighbors.Count; i++)
            {
                Point neighbor = neighbors[i].point;

                if (neighbor != null)
                {
                    Gizmos.DrawWireCube(neighbor.position_World, Vector3.one);
                }
            }
        }
    }
    public void DrawArrow(Point neighbor)
    {
        if ((this.IsContain(neighbor) && neighbor.IsContain(this)) == false)
        {
            Gizmos.color = Color.red;


            Vector3 middle = (this.position_World + neighbor.position_World) / 2f;

            Vector3 vector = this.position_World - neighbor.position_World;


            Vector3 right = Quaternion.AngleAxis(30, new Vector3(0, 0, 1)) * vector.normalized;

            Vector3 left = Quaternion.AngleAxis(-30, new Vector3(0, 0, 1)) * vector.normalized;


            Gizmos.DrawLine(middle, middle + right);

            Gizmos.DrawLine(middle, middle + left);
        }
    }
    public bool IsContain(Point neighbor)
    {
        for (int i = 0; i < neighbors.Count; i++)
        {
            if (neighbors[i].point == neighbor)
            {
                return true;
            }
        }
        return false;
    }

    public void GetInfo(Neighbor neighbor)
    {
        neighbor.vector = (neighbor.point.position_World - position_World);

        neighbor.distance = neighbor.vector.magnitude;
    }

    public Neighbor GetNeighbor(Point point)
    {
        for (int i = 0; i < neighbors.Count; i++)
        {
            if (neighbors[i].point.num == point.num)
            {
                return neighbors[i];
            }
        }
        return null;
    }
}

[System.Serializable]
public class Neighbor
{
    public Point point;

    public Vector3 vector;

    public float distance;
}