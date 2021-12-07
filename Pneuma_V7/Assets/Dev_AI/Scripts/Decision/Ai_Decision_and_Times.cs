using UnityEngine;
using System;

[Serializable]
public struct DicisionAndTime
{
    public E_Dicision dicision;

    public float duration;

    public Vector3 vectorToNext, targetPos;

    public float jumpVelocity;

    public float speed;

    public void SetWalk(float duration, Vector3 vectorToNext, float speed, Vector3 targetPos)
    {
        dicision = E_Dicision.walk;

        this.duration = duration;

        this.vectorToNext = vectorToNext;

        this.targetPos = targetPos;

        this.speed = speed;
    }
    public void SetJump(float duration, Vector3 vectorToNext, float jumpVelocity, float speed, Vector3 targetPos)
    {
        dicision = E_Dicision.jump;

        this.duration = duration;

        this.vectorToNext = vectorToNext;

        this.targetPos = targetPos;

        this.jumpVelocity = jumpVelocity;

        this.speed = speed;
    }
    public void SetTeleporation(float duration, Vector3 vectorToNext, Vector3 targetPos)
    {
        dicision = E_Dicision.teleportation;

        this.duration = duration;

        this.vectorToNext = vectorToNext;

        this.targetPos = targetPos;
    }

}

public enum E_Dicision
{
    walk,
    jump,
    teleportation,
}