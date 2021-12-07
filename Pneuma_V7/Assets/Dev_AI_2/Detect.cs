using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Detect
{ 
    public static bool IsGround(Point from, Point to, float length = 0.5f)
    {
        LayerMask layerGround = 1 << LayerMask.NameToLayer("Ground");
        LayerMask layerWall = 1 << LayerMask.NameToLayer("Wall");

        Vector2 checkPos = (from.position_World + to.position_World) / 2f;

        return Physics2D.Raycast(checkPos, Vector2.down, length, layerGround.value) || Physics2D.Raycast(checkPos, Vector2.down, length, layerWall.value);
    }

    public static bool IsGround(Vector3 from, Point to, float length = 0.5f)
    {
        LayerMask layerGround = 1 << LayerMask.NameToLayer("Ground");
        LayerMask layerWall = 1 << LayerMask.NameToLayer("Wall");

        Vector2 checkPos = (from + to.position_World) / 2f;

        return Physics2D.Raycast(checkPos, Vector2.down, length, layerGround.value) || Physics2D.Raycast(checkPos, Vector2.down, length, layerWall.value);

    }

    public static bool IsGround(Vector3 pos, float length = 0.5f)
    {
        LayerMask layerGround = 1 << LayerMask.NameToLayer("Ground");
        LayerMask layerWall = 1 << LayerMask.NameToLayer("Wall");

        Vector2 checkPos = pos;

        return Physics2D.Raycast(checkPos, Vector2.down, length, layerGround.value) || Physics2D.Raycast(checkPos, Vector2.down, length, layerWall.value);
    }
}
