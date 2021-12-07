using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public List<Graph> graphs = new List<Graph>();

    [SerializeField]
    public Point[] points;

    private List<Point>
        list_UnVisit = new List<Point>(),
        list_Visited = new List<Point>();


    public bool IsSameGraph(Vector3 pos1, Vector3 pos2)
    {
        Point point1 = GetPoint(pos1), point2 = GetPoint(pos2);

        int graphIndex1 = GetGraphIndex(point1), graphIndex2 = GetGraphIndex(point2);

        return (graphIndex1 == graphIndex2) ? true : false;
    }

    public Point GetPoint(Vector3 pos)
    {
        int targetNum = -1;
        float distance = 0;

        for (int i = 0; i < points.Length; i++)
        {
            float temp = (points[i].position_World - pos).magnitude;

            if (temp < distance || targetNum == -1)
            {
                distance = temp;
                targetNum = i;
            }
        }

        return points[targetNum];
    }
    public int GetPointNum(Vector3 pos)
    {
        int targetNum = -1;
        float distance = 0;

        for (int i = 0; i < points.Length; i++)
        {
            float temp = (points[i].position_World - pos).magnitude;

            if (temp < distance || targetNum == -1)
            {
                distance = temp;
                targetNum = i;
            }
        }

        return points[targetNum].num;
    }
    public int GetGraphIndex(Point point)
    {
        for (int i = 0; i < graphs.Count; i++)
        {
            if (graphs[i].isContain(point))
            {
                return i;
            }
        }
        return -1;
    }

    public Graph GetGraph(Vector3 pos)
    {
        Point point = GetPoint(pos);

        int graphIndex = GetGraphIndex(point);

        return graphs[graphIndex];
    }

    public List<Point> GetPath(Vector3 pos1, Vector3 pos2)
    {
        if (IsSameGraph(pos1, pos2))
        {
            Point start = GetPoint(pos1);
            Graph graph = graphs[GetGraphIndex(start)];

            Debug.Log(start.name + " , " + start.num);
            Debug.Log("GraphIndex " + GetGraphIndex(start));

            list_UnVisit.Clear();
            list_Visited.Clear();


            for (int i = 0; i < graph.points.Count; i++)
            {
                list_UnVisit.Add(graph.points[i]);

                graph.points[i].cost = float.MaxValue;

                graph.points[i].parent = -1;
            }

            start.cost = 0;


            while (list_UnVisit.Count != 0)
            {
                int index = -1;
                float value = -1;


                for (int i = 0; i < list_UnVisit.Count; i++)
                {
                    if (value == -1)
                    {
                        index = i;
                        value = list_UnVisit[i].cost;
                    }
                    else
                    {
                        if (list_UnVisit[i].cost < value)
                        {
                            index = i;
                            value = list_UnVisit[i].cost;
                        }
                    }
                }

                Point point = list_UnVisit[index];

                list_Visited.Add(point);
                list_UnVisit.Remove(point);


                for (int i = 0; i < point.neighbors.Count; i++)
                {
                    Neighbor neighbor = point.neighbors[i];

                    Debug.Log(point.name + " , " + neighbor.point.name);

                    if (!list_Visited.Contains(neighbor.point))
                    {
                        float tempCost = point.cost + neighbor.distance;
                        //calculate cost

                        if ((neighbor.point.cost == float.MaxValue) || tempCost < neighbor.point.cost)
                        {
                            //set cost G , parent
                            neighbor.point.cost = tempCost;
                            neighbor.point.parent = point.num;


                            Debug.Log(point.name + " , " + neighbor.point.name + " , " + neighbor.point.parent);
                        }
                    }
                }
            }


            Point lastPoint = GetPoint(pos2);

            Debug.Log(lastPoint.name + " , " + lastPoint.num);

            List<Point> pathPoints = new List<Point>();

            int secure = 0;

            while (lastPoint.parent != -1)
            {
                pathPoints.Add(lastPoint);

                Debug.Log(lastPoint.num + " , parent " + lastPoint.parent + " graph.point " + graph.GetPoint(lastPoint.parent).num);

                lastPoint = graph.GetPoint(lastPoint.parent);


                secure++;
                if (secure >= 100)
                {
                    Debug.LogError("Ĳ�o�קK�L���j�骺�w������");
                    break;
                }
            }

            pathPoints.Add(start);

            pathPoints.Reverse();

            return pathPoints;
        }
        else
        {
            return null;
        }
    }
}
