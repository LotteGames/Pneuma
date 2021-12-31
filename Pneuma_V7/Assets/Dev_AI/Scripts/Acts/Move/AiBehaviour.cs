using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class AiBehaviour : MonoBehaviour
{
    public NodeManager nodeManager;

    public eState_AI State = eState_AI.Idle;

    public void SetState(eState_AI eState)
    {
        State = eState;
    }

    public Ai_PhysicSetting physicSetting;


    public List<PathNode> pathNodes;
    public List<DicisionAndTime> dicisionAndTimes = new List<DicisionAndTime>();


    public Rigidbody2D rb2D;

    public Ai_MoveWays ai_MoveWays;

    public Ai_Decision ai_Decision;

    public Transform playerPos;

    public Animator animator;

    public SpriteRenderer spriteRenderer;

    public void AddGravity()
    {
        rb2D.AddForce(new Vector2(0, -physicSetting.gravity));
    }


    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }


    public UnityEvent event_TargetNodeChanged;


    public void GetPath()
    {
        List<PathNode> pathNodes = Ai_Detect.GetPathNodes(nodeManager, transform.position, playerPos.position);
        dicisionAndTimes = ai_Decision.GetDicisions(pathNodes);
    }
    public void StartPath()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(Ieum_Behaviour());
    }
    Coroutine coroutine;
    IEnumerator Ieum_Behaviour()
    {
        int i = 0;
        while (dicisionAndTimes.Count != i)
        {
            DicisionAndTime dicisionAndTime = dicisionAndTimes[i];

            float time = 0, duration = dicisionAndTime.duration;

            ChangeImgDir(dicisionAndTime.vectorToNext);


            if (dicisionAndTime.dicision == E_Dicision.teleportation)
            {
                ai_MoveWays.Teleporation(transform.position + dicisionAndTime.vectorToNext);
                animator.SetBool("IsWalk", true);

                while (time < duration)
                {
                    yield return new WaitForEndOfFrame();
                    time += Time.deltaTime;
                }
            }


            else if (dicisionAndTime.dicision == E_Dicision.walk)
            {
                animator.SetBool("IsWalk", true);

                while (time < duration)
                {
                    ai_MoveWays.Walk(dicisionAndTime.speed);


                    yield return new WaitForEndOfFrame();
                    time += Time.deltaTime;
                }
                ai_MoveWays.ResetVelocity();
            }


            else if (dicisionAndTime.dicision == E_Dicision.jump)
            {
                ai_MoveWays.Jump(dicisionAndTime.speed, dicisionAndTime.jumpVelocity, transform.position + dicisionAndTime.vectorToNext);

                animator.SetBool("IsWalk", true);

                while (time < duration)
                {
                    yield return new WaitForEndOfFrame();
                    time += Time.deltaTime;
                }
                ai_MoveWays.ResetVelocity();
            }


            i++;
        }
        animator.SetBool("IsWalk", false);
    }

    public void InvokeTargetNode()
    {
        if (State == eState_AI.Follow)
        {
            event_TargetNodeChanged.Invoke();
        }
    }


    private void FixedUpdate()
    {
        AddGravity();
    }

    [HideInInspector]
    public float jumpVelocity = 0f;

    public void ChangeImgDir(Vector3 vector)
    {
        if (vector.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (vector.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }
}
public enum eState_AI
{
    Follow,
    //HasTarget,
    Idle,
}

[Serializable]
public class Ai_PhysicSetting
{

    public float
        maxJumpHeight,
        timeToHeightest,
        speed,
        characterHeight,
        maxSlopeAngle;

    public float gravity, maxJumpVelocity;

    public float Gravity
    {
        get
        {
            return gravity;
        }
    }
    public float MaxJumpVelocity
    {
        get
        {
            return maxJumpVelocity;
        }
    }


    /// <summary>
    /// ���O���ܡA���Ǹ��D�O�D���ܡA�Ӹ��줣�P����
    /// </summary>
    public void CalculateValues()
    {
        float value = 2 * maxJumpHeight / (timeToHeightest * timeToHeightest);
        gravity = value;

        value = 2 * maxJumpHeight / timeToHeightest;
        maxJumpVelocity = value;
    }
}