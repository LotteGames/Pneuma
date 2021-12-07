using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Ai_Detect
{
    //public static Node GetNode(NodeManager nodeManager, Vector3 pos)
    //{
    //    return nodeManager.GetNode(pos);
    //}

    public static List<PathNode> GetPathNodes(NodeManager nodeManager, Vector3 pos)
    {
        Node node_Start = nodeManager.GetNode(pos);

        Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Node node_End = nodeManager.GetNode(clickPos);

        return nodeManager.GetPath(node_Start, node_End);
    }
    public static List<PathNode> GetPathNodes(NodeManager nodeManager, Vector3 startPos, Vector3 endPos)
    {
        Node node_Start = nodeManager.GetNode(startPos);

        Node node_End = nodeManager.GetNode(endPos);

        return nodeManager.GetPath(node_Start, node_End);
    }

    public static List<PathNode> GetPathNodes(NodeManager nodeManager, Node node_Start, Node node_End)
    {
        return nodeManager.GetPath(node_Start, node_End);
    }

    /// <summary>
    /// Detect is there ground connect two nodes.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static bool GetIsGround(Node from, Node to, float length = 0.3f)
    {
        LayerMask groundLayer = 1 << LayerMask.NameToLayer("Ground");

        Vector2 checkPos = (from.Position + to.Position) / 2f;

        Debug.DrawLine(checkPos, checkPos + Vector2.down * length, Color.red, length);

        return Physics2D.Raycast(checkPos, Vector2.down, length, groundLayer.value);
    }
    public static bool GetIsGround(Vector3 from, Node to, float length = 0.3f)
    {
        LayerMask groundLayer = 1 << LayerMask.NameToLayer("Ground");

        Vector2 checkPos = (from + to.Position) / 2f;

        Debug.DrawLine(checkPos, checkPos + Vector2.down * length, Color.red, length);

        return Physics2D.Raycast(checkPos, Vector2.down, length, groundLayer.value);
    }

    public static bool GetIsGround(Vector3 pos, Color lineColor, float length = 0.3f)
    {
        LayerMask groundLayer = 1 << LayerMask.NameToLayer("Ground");

        Vector2 checkPos = pos;

        Debug.DrawLine(checkPos, checkPos + Vector2.down * length, lineColor, length);

        return Physics2D.Raycast(checkPos, Vector2.down, length, groundLayer.value);
    }
}
