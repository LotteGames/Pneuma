using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Behaviour : MonoBehaviour
{
    public TalkBehav talkBehav;

    public AI_State ai_State;

    public List<Point> pathPoints;

    public Graph graph;

    public int pointNum;

    private PathManager pathManager;

    public Transform target;

    public bool isFollow;

    public PhysicSetting physicSetting;

    private Rigidbody2D rb2D;

    public void SetTarget(Transform transform)
    {
        target = transform;
    }

    private void Awake()
    {
        physicSetting.CalculateValues();

        rb2D = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        pathManager = FindObjectOfType<PathManager>();

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public List<AI_State> decision;
    public void DecideStates()
    {
        if (IsGround)
        {
            graph = pathManager.GetGraph(transform.position);

            pathPoints = pathManager.GetPath(transform.position, target.position);

            pointNum = pathManager.GetPointNum(transform.position);


            decision.Clear();

            if (pathPoints != null)
            {
                if (pathPoints.Count != 1)
                {

                    for (int i = 0; i < pathPoints.Count - 1; i++)
                    {

                        if (isFollow)
                        {
                            if (pathPoints != null)
                            {
                                if (pointNum == pathPoints[pathPoints.Count - 1].num)
                                {

                                    //Debug.LogError(123);
                                    decision.Add(AI_State.idle);
                                }
                                else
                                {
                                    Point point = pathPoints[i];

                                    Neighbor neighbor = point.GetNeighbor(pathPoints[i + 1]);


                                    float angle;

                                    Vector3 checkPos;

                                    bool isGround;

                                    {
                                        Debug.Log(point.num + " , " + pathPoints.Count + " , " + (i + 1));
                                        angle = GetAngle(neighbor.vector);

                                        checkPos = (neighbor.point.position_World + point.position_World) / 2f;

                                        isGround = Detect.IsGround(checkPos);
                                    }

                                    if (neighbor.vector.y >= 0)
                                    {
                                        if (isGround)
                                        {
                                            if (angle <= physicSetting.maxSlopeAngle)
                                            {
                                                //Debug.LogError(456);
                                                decision.Add(AI_State.walk);
                                            }
                                            else
                                            {
                                                float verticleDistance_OtoH = neighbor.vector.y + 1f;

                                                float time_OtoH = Mathf.Sqrt(verticleDistance_OtoH / (physicSetting.gravity / 2f));

                                                float time_HtoG = Mathf.Sqrt(1f / (physicSetting.gravity / 2f));

                                                float duration = time_OtoH + time_HtoG;


                                                if (neighbor.vector.y <= physicSetting.maxJumpHeight &&
                                                    Math.Abs(neighbor.vector.x) <= physicSetting.speed * duration)
                                                {
                                                    decision.Add(AI_State.jump);
                                                }
                                                else
                                                {
                                                    decision.Add(AI_State.teleporation);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            float verticleDistance_OtoH = neighbor.vector.y + 1f;

                                            float time_OtoH = Mathf.Sqrt(verticleDistance_OtoH / (physicSetting.gravity / 2f));

                                            float time_HtoG = Mathf.Sqrt(1f / (physicSetting.gravity / 2f));

                                            float duration = time_OtoH + time_HtoG;


                                            if (verticleDistance_OtoH < physicSetting.maxJumpHeight &&
                                                Math.Abs(neighbor.vector.x) < physicSetting.speed * duration)
                                            {
                                                decision.Add(AI_State.jump);
                                            }
                                            else
                                            {
                                                decision.Add(AI_State.teleporation);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (isGround)
                                        {
                                            if (angle <= physicSetting.maxSlopeAngle)
                                            {
                                                //Debug.LogError(789);
                                                decision.Add(AI_State.walk);
                                            }
                                        }
                                        else
                                        {

                                            float time_OtoG_y = Mathf.Sqrt(Mathf.Abs(neighbor.vector.y) / (physicSetting.gravity / 2f));

                                            if (Mathf.Abs(neighbor.vector.x) <= time_OtoG_y * physicSetting.speed)
                                            {
                                                //Debug.LogError(101112);
                                                decision.Add(AI_State.walk);
                                            }
                                            else
                                            {
                                                decision.Add(AI_State.teleporation);
                                            }

                                        }
                                    }
                                }
                            }
                            else
                            {
                                decision.Add(AI_State.teleporation);
                            }
                        }
                        else
                        {
                            decision.Add(AI_State.rest);
                        }
                    }
                }

                stateIndex = 0;

                if (decision.Count != 0)
                {
                    ai_State = decision[stateIndex];
                }
            }
            else
            {
                stateIndex = 0;
                pathPoints = new List<Point>();
                pathPoints.Add(pathManager.GetPoint(target.position));
                decision.Add(AI_State.teleporation);
            }
        }
        else
        {
            StartWait();
        }
    }
    Coroutine coroutine;
    public IEnumerator wait()
    {
        while (IsGround == false)
        {
            yield return new WaitForEndOfFrame();
        }
        DecideStates();
    }
    public void StartWait()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(wait());
    }

    public int stateIndex = 0;
    public int moveWay;
    float jumpMoment;

    public ContactFilter2D contactFilter;
    public bool IsGround
    {
        get
        {
            return rb2D.IsTouching(contactFilter);
        }
    }

    public LayerMask layerMask;
    float jumpV;
    private void FixedUpdate()
    {
        if (talkBehav.IsFin)
        {

            animator.SetBool("IsGround", IsGround);

            Debug.Log(ai_State.ToString());

            if (!isFollow && IsGround)
            {
                if (decision.Count != 1)
                {
                    Debug.Log(275);
                    decision.Clear();
                    decision.Add(AI_State.rest);
                }
            }

            rb2D.AddForce(new Vector2(0, -physicSetting.gravity));


            if (decision.Count != 0 && stateIndex < decision.Count)
            {
                ai_State = decision[stateIndex];
            }
            else if (isFollow)
            {
                //Debug.LogError(159);
                ai_State = AI_State.idle;
            }


            //Debug.LogError("index : " + stateIndex + " , " + decision.Count);





            Point point = null;
            Neighbor neighbor = null;

            switch (ai_State)
            {
                case AI_State.idle:

                    break;


                case AI_State.walk:


                    animator.SetBool("IsWalk", true);

                    point = pathPoints[stateIndex];

                    if (stateIndex + 1 < pathPoints.Count)
                    {

                        neighbor = point.GetNeighbor(pathPoints[stateIndex + 1]);

                        moveWay = (neighbor.vector.x > 0) ? 1 : -1;

                        spriteRenderer.flipX = (neighbor.vector.x > 0) ? false : true;

                        rb2D.velocity = new Vector2(physicSetting.speed * moveWay, rb2D.velocity.y);

                        if (Math.Abs(neighbor.point.position_World.x - transform.position.x) < 0.2f
                              && transform.position.y + 1.5f >= neighbor.point.position_World.y)
                        {

                            Debug.LogError("abc");

                            stateIndex++;

                            rb2D.velocity = Vector2.zero;

                            animator.SetBool("IsWalk", false);
                        }
                        else if (neighbor.point.position_World.y > transform.position.y + 1.5f
                            && Math.Abs(neighbor.point.position_World.y - transform.position.y + 1.5f) > 0.2f)
                        {
                            transform.position = neighbor.point.position_World;
                            Debug.LogError("cde");
                            stateIndex++;

                            rb2D.velocity = Vector2.zero;

                            animator.SetBool("IsWalk", false);
                        }
                    }
                    break;


                case AI_State.jump:

                    point = pathPoints[stateIndex];

                    neighbor = point.GetNeighbor(pathPoints[stateIndex + 1]);

                    spriteRenderer.flipX = (neighbor.vector.x > 0) ? false : true;

                    moveWay = (neighbor.vector.x > 0) ? 1 : -1;

                    if (neighbor.vector.x > 0 && (Physics2D.Raycast(transform.position + new Vector3(0, 1f), Vector2.right, 1.2f, layerMask)
                       || Physics2D.Raycast(transform.position + new Vector3(0, 1.5f), Vector2.right, 1.2f, layerMask)
                       || Physics2D.Raycast(transform.position, Vector2.right, 1.2f, layerMask)))
                    {
                        moveWay = 0;
                    }

                    if (neighbor.vector.x < 0 && (Physics2D.Raycast(transform.position + new Vector3(0, 1f), Vector2.left, 1.2f, layerMask)
                        || Physics2D.Raycast(transform.position, Vector2.left, 1.2f, layerMask)
                        || Physics2D.Raycast(transform.position + new Vector3(0, 1.5f), Vector2.left, 1.2f, layerMask)))
                    {
                        moveWay = 0;
                    }
                    Debug.DrawLine(transform.position, transform.position + new Vector3(1, 0) * 1.2f, Color.red);
                    Debug.DrawLine(transform.position, transform.position + new Vector3(1, 0) * 1.2f, Color.red);
                    Debug.DrawLine(transform.position + new Vector3(0, 1.5f), transform.position + new Vector3(0, 1.5f) + new Vector3(1, 0) * 1.2f, Color.red);


                    float distance_y = Mathf.Abs(neighbor.vector.y) + 1f;

                    float time_OtoH = Mathf.Sqrt(distance_y / (physicSetting.gravity / 2f));

                    float time_HtoG = Mathf.Sqrt(1f / (physicSetting.gravity / 2f));

                    float duration = time_OtoH + time_HtoG;

                    jumpV = (time_OtoH * physicSetting.gravity);



                    if (Math.Abs(neighbor.point.position_World.x - transform.position.x) < 0.2f
                        && transform.position.y > neighbor.point.position_World.y)
                    {
                        if (stateIndex < decision.Count - 1)
                        {
                            AI_State nextState = decision[stateIndex + 1];

                            if (nextState == AI_State.walk)
                            {
                                Point point1 = pathPoints[stateIndex + 1];
                                Neighbor neighbor1 = point1.GetNeighbor(pathPoints[stateIndex + 2]);

                                bool currentPoint_MoveDir = neighbor.vector.x > 0 ? true : false;
                                bool nextPoint_MoveDir = neighbor1.vector.x > 0 ? true : false;

                                if (currentPoint_MoveDir != nextPoint_MoveDir)
                                {
                                    moveWay = 0;
                                }
                            }

                        }
                        else if (stateIndex == decision.Count - 1)
                        {
                            moveWay = 0;
                        }
                        stateIndex++;

                        jumpMoment = 0;


                    }



                    if (jumpMoment + duration < Time.time && IsGround)
                    {
                        animator.SetBool("IsJump", true);
                    }

                    if (jumpMoment + duration > Time.time)//���D���ɶ��̭�����������
                    {
                        rb2D.velocity = new Vector2(physicSetting.speed * moveWay, rb2D.velocity.y);
                    }

                    if (jumpMoment == 0)
                    {
                        rb2D.velocity = new Vector2(0, rb2D.velocity.y);
                    }

                    break;


                case AI_State.teleporation:

                    point = pathPoints[stateIndex];

                    if (stateIndex + 1 < pathPoints.Count)
                    {
                        Debug.LogError(1);
                        neighbor = point.GetNeighbor(pathPoints[stateIndex + 1]);

                        transform.position = neighbor.point.position_World;

                        stateIndex++;
                    }
                    else
                    {
                        Debug.LogError(2);
                        transform.position = point.position_World;
                        stateIndex++;
                    }

                    rb2D.velocity = Vector2.zero;

                    break;


                case AI_State.rest:

                    if (IsGround)
                    {
                        if (spriteRenderer.flipX)
                        {
                            Debug.DrawLine(restPoint_L.position, restPoint_L.position + new Vector3(0, -1) * 1.2f, Color.red);
                            Debug.DrawLine(restPoint_L.position, restPoint_L.position + new Vector3(-1, 0) * 1.2f, Color.red);

                            if (Physics2D.Raycast(restPoint_L.position, Vector2.down, 1.2f, layerMask))
                            {
                                if (Physics2D.Raycast(restPoint_L.position, Vector2.left, 1.2f, layerMask))
                                {
                                    spriteRenderer.flipX = false;
                                }
                            }
                            else
                            {
                                spriteRenderer.flipX = false;
                            }
                        }
                        else
                        {
                            Debug.DrawLine(restPoint_R.position, restPoint_R.position + new Vector3(0, -1) * 1.2f, Color.red);
                            Debug.DrawLine(restPoint_R.position, restPoint_R.position + new Vector3(1, 0) * 1.2f, Color.red);


                            if (Physics2D.Raycast(restPoint_R.position, Vector2.down, 1.2f, layerMask))
                            {
                                if (Physics2D.Raycast(restPoint_R.position, Vector2.right, 1.2f, layerMask))
                                {
                                    spriteRenderer.flipX = true;
                                }
                            }
                            else
                            {
                                spriteRenderer.flipX = true;
                            }
                        }

                        rb2D.velocity = new Vector2(((spriteRenderer.flipX) ? -1 : 1) * physicSetting.speed, rb2D.velocity.y);
                        animator.SetBool("IsWalk", true);
                    }
                    else
                    {
                        rb2D.velocity = new Vector2(0, rb2D.velocity.y);
                        animator.SetBool("IsWalk", false);
                    }

                    break;
            }
        }
        else
        {
            //if (target.position.x > transform.position.x)
            //{
            //    spriteRenderer.flipX = false;
            //}
            //else if (target.position.x < transform.position.x)
            //{
            //    spriteRenderer.flipX = true;
            //}
            if (catContrl.transform.position.x > transform.position.x)
            {
                spriteRenderer.flipX = false;
            }
            else if (catContrl.transform.position.x < transform.position.x)
            {
                spriteRenderer.flipX = true;
            }

            rb2D.velocity = new Vector2(0, rb2D.velocity.y);
            animator.SetBool("IsWalk", false);
        }
    }

    public CatContrl catContrl;
    public void SetJumpVelocity()
    {
        jumpMoment = Time.time;
        rb2D.velocity = new Vector2(rb2D.velocity.x, jumpV);
        animator.SetBool("IsJump", false);
    }
    public Transform restPoint_R, restPoint_L;


    public SpriteRenderer spriteRenderer;

    public Animator animator;

    public float GetAngle(Vector3 vector)
    {
        if (vector.x > 0)
        {
            return Vector3.Angle(vector, Vector2.right);
        }
        else if (vector.x < 0)
        {
            return Vector3.Angle(vector, Vector2.left);
        }
        return 0;
    }
    public void SetIsFollow(bool value)
    {
        isFollow = value;
    }
}
public enum AI_State
{
    idle,
    walk,
    jump,
    teleporation,
    rest,
}
[Serializable]
public class PhysicSetting
{

    public float
        maxJumpHeight,
        timeToHeightest,
        speed,
        characterHeight,
        maxSlopeAngle;

    public float gravity, maxJumpVelocity;
    public void CalculateValues()
    {
        float value = 2 * maxJumpHeight / (timeToHeightest * timeToHeightest);
        gravity = value;

        value = 2 * maxJumpHeight / timeToHeightest;
        maxJumpVelocity = value;
    }
}