using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Graph : MonoBehaviour
{
    public GameObject graph;
    private GameObject lastGraph;

    private void OnValidate()
    {
        if (graph != null && lastGraph != graph)
        {
            lastGraph = graph;

            Point[] points = graph.GetComponentsInChildren<Point>();

            this.points.Clear();

            for (int i = 0; i < points.Length; i++)
            {
                this.points.Add(points[i]);
            }
        }
    }

    [SerializeField, HideInInspector]
    public List<Point> points = new List<Point>();

    public bool isContain(Point point)
    {
        return points.Contains(point);
    }
    public Point GetPoint(int num) 
    {
        for (int i = 0; i < points.Count; i++) 
        {
            if (points[i].num == num) 
            {
                return points[i];
            }
        }
        return null;
    }
}
