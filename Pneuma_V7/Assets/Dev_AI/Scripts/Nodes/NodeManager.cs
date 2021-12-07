using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
  

    [SerializeField]
    public Node[] nodes;
    public Node[] Nodes
    {
        get
        {
            return nodes;
        }
        set
        {
            nodes = value;
        }
    }

    [Header("For Testing")]
    public Node node_Start;
    public Node node_End;//�Ȯɪ��A���|�Q�n�b���}���̦��o���ܼơC


    private List<Node> list_UnVisit = new List<Node>(), list_Visited = new List<Node>();


    public Node GetNode(Vector3 pos)
    {
        int targetNum = -1;
        float distance = 0;

        for (int i = 0; i < nodes.Length; i++)
        {
            float temp = (nodes[i].Position - pos).magnitude;

            if (temp < distance || targetNum == -1)
            {
                distance = temp;
                targetNum = i;
            }
        }

        return nodes[targetNum];
    }
    public Node GetNode(Node except, Vector3 pos)
    {
        int targetNum = -1;
        float distance = 0;

        for (int i = 0; i < nodes.Length; i++)
        {
            if (nodes[i] != except)
            {
                float temp = (nodes[i].Position - pos).magnitude;

                if (temp < distance || distance == 0)
                {
                    distance = temp;
                    targetNum = i;
                }
            }
        }

        return nodes[targetNum];
    }

    //public List<int> GetPath(Node start, Node end)
    //{
    //    #region--��l�]�w--

    //    list_UnVisit.Clear();
    //    list_Visited.Clear();

    //    for (int i = 0; i < nodes.Length; i++)
    //    {
    //        list_UnVisit.Add(nodes[i]);
    //        nodes[i].Cost_G = float.MaxValue;
    //    }
    //    start.Cost_G = 0;

    //    #endregion


    //    while (list_UnVisit.Count != 0)
    //    {
    //        #region--��쥼���X���I���ȳ̤p���I--
    //        int index = -1;
    //        float value = -1;
    //        for (int i = 0; i < list_UnVisit.Count; i++)
    //        {
    //            if (value == -1)
    //            {
    //                index = i;
    //                value = list_UnVisit[i].Cost_G;
    //            }
    //            else
    //            {
    //                if (list_UnVisit[i].Cost_G < value)
    //                {
    //                    index = i;
    //                    value = list_UnVisit[i].Cost_G;
    //                }
    //            }
    //        }

    //        Node current = list_UnVisit[index];

    //        list_Visited.Add(current);
    //        list_UnVisit.Remove(current);

    //        #endregion


    //        for (int i = 0; i < current.neighborNodes.Count; i++)
    //        {
    //            NeighborNode neighborNode = current.neighborNodes[i];

    //            if (!list_Visited.Contains(neighborNode.neightbor))
    //            {
    //                float tempCost = current.Cost_G + neighborNode.distance;
    //                //calculate cost

    //                if ((neighborNode.neightbor.Cost_G == float.MaxValue) || tempCost < neighborNode.neightbor.cost_G)
    //                {
    //                    //set cost G , parent
    //                    neighborNode.neightbor.Cost_G = tempCost;
    //                    neighborNode.neightbor.parentNum = current.NodeNum;
    //                }
    //            }
    //        }
    //    }

    //    #region--�qEnd�̴`ParentNum�˱����_�I�A�ÿ�X--

    //    List<int> nodeNums = new List<int>();

    //    Node node = end;

    //    int secure = 0;
    //    while (node.parentNum != -1)
    //    {
    //        nodeNums.Add(node.NodeNum);
    //        node = nodes[node.parentNum];

    //        secure++;
    //        if (secure >= 100)
    //        {
    //            break;
    //        }
    //    }
    //    nodeNums.Add(start.NodeNum);

    //    nodeNums.Reverse();

    //    string pathNodes = "";

    //    for (int i = 0; i < nodeNums.Count; i++)
    //    {
    //        if (i == nodeNums.Count - 1)
    //        {
    //            pathNodes += nodeNums[i];
    //        }
    //        else
    //        {
    //            pathNodes += nodeNums[i] + ",";
    //        }
    //    }

    //    Debug.Log("Path Nodes = " + pathNodes);

    //    #endregion


    //    #region--���]parentNum--

    //    for (int i = 0; i < nodes.Length; i++)
    //    {
    //        nodes[i].parentNum = -1;
    //    }

    //    #endregion

    //    return nodeNums;
    //}

    public List<PathNode> GetPath(Node start, Node end)
    {
        #region--��l�]�w--

        list_UnVisit.Clear();
        list_Visited.Clear();

        for (int i = 0; i < nodes.Length; i++)
        {
            list_UnVisit.Add(nodes[i]);
            nodes[i].Cost_G = float.MaxValue;
        }
        start.Cost_G = 0;

        #endregion


        while (list_UnVisit.Count != 0)
        {
            #region--��쥼���X���I���ȳ̤p���I--
            int index = -1;
            float value = -1;
            for (int i = 0; i < list_UnVisit.Count; i++)
            {
                if (value == -1)
                {
                    index = i;
                    value = list_UnVisit[i].Cost_G;
                }
                else
                {
                    if (list_UnVisit[i].Cost_G < value)
                    {
                        index = i;
                        value = list_UnVisit[i].Cost_G;
                    }
                }
            }

            Node current = list_UnVisit[index];

            list_Visited.Add(current);
            list_UnVisit.Remove(current);

            #endregion


            for (int i = 0; i < current.neighborNodes.Count; i++)
            {
                NeighborNode neighborNode = current.neighborNodes[i];

                if (!list_Visited.Contains(neighborNode.neightbor))
                {
                    float tempCost = current.Cost_G + neighborNode.distance;
                    //calculate cost

                    if ((neighborNode.neightbor.Cost_G == float.MaxValue) || tempCost < neighborNode.neightbor.cost_G)
                    {
                        //set cost G , parent
                        neighborNode.neightbor.Cost_G = tempCost;
                        neighborNode.neightbor.parentNum = current.NodeNum;
                    }
                }
            }
        }

        #region--�qEnd�̴`ParentNum�˱����_�I�A�ÿ�X--


        List<PathNode> pathNodes = new List<PathNode>();

        Node node = end;
        PathNode pathNode;
        //�|���ONode4�O�]������ɦV������Y�A����ɦV����A������������Y�ܼƮɨä��|�ƻs�@�ӷs������
        //�٬O���ϥΦP�@�Ӫ���

        int secure = 0;
        while (node.parentNum != -1)
        {
            pathNode = new PathNode();
            pathNode.node = node;


            pathNodes.Add(pathNode);

            node = nodes[node.parentNum];

            secure++;
            if (secure >= 100)
            {
                Debug.LogError("Ĳ�o�w������");
                break;
            }
        }
        pathNode = new PathNode();
        pathNode.node = start;
        pathNodes.Add(pathNode);

        pathNodes.Reverse();



        #endregion


        #region--���]parentNum--

        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i].parentNum = -1;
        }

        #endregion

        #region--�]�w��U�@��Node��Vector--

        string path = "";

        for (int i = 0; i < pathNodes.Count; i++)
        {
            path += pathNodes[i].node.gameObject.name + " , ";
            //Debug.Log(pathNodes[i].node.gameObject.name);

            if (i == pathNodes.Count - 1)
            {
                pathNodes[i].vectorToNext = Vector3.zero;
                break;
                //�̫�@�Ӹ`�I�S���U�@�ӥi�H��
            }
            pathNodes[i].vectorToNext = pathNodes[i + 1].node.Position - pathNodes[i].node.Position;
        }
        //Debug.Log("path : " + path);

        #endregion

        return pathNodes;
    }
    public Node GetNode(int index)
    {
        return nodes[index];
    }
}

[System.Serializable]
public class PathNode
{
    public Node node;

    public Vector3 vectorToNext;
}