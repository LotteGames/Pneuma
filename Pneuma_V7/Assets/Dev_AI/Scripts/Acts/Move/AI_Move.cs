using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
public class AI_Move : MonoBehaviour
{
    #region--感知--

    private Node node_Start, node_End;

    private NodeManager nodeManager;

    public List<PathNode> pathNodes;


    private SpriteRenderer spriteRenderer;

    private BoxCollider2D col2D;


    public void SetVariable_Detect()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        col2D = GetComponent<BoxCollider2D>();

        nodeManager = FindObjectOfType<NodeManager>();
    }

    public bool IsMouseClick
    {
        get
        {
            return Input.GetMouseButtonDown(0);
        }
    }

    public void GetPathNodes()
    {
        if (IsMouseClick)
        {
            node_Start = nodeManager.GetNode(transform.position);


            Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            node_End = nodeManager.GetNode(clickPos);


            pathNodes.Clear();
            pathNodes = nodeManager.GetPath(node_Start, node_End);

            //
            SetDicisions();
        }
    }


    [SerializeField]
    private float lineLength = 1;

    [SerializeField]
    private LayerMask groundLayer;

    public bool IsGroundAhead
    {
        get
        {
            Vector2 checkPos = transform.position + new Vector3((spriteRenderer.flipX) ? -col2D.size.x / 2 : col2D.size.x / 2, -col2D.size.y / 2) * 0.9f;

            Debug.DrawLine(checkPos, checkPos + Vector2.down * lineLength, Color.red);

            return Physics2D.Raycast(checkPos, Vector2.down, lineLength, groundLayer.value);
        }
    }

    public bool GetIsGround(Node from, Node to, float length = 1f)
    {
        Vector2 checkPos = (from.Position + to.Position) / 2f;

        Debug.DrawLine(checkPos, checkPos + Vector2.down * length, Color.red, 1f);

        return Physics2D.Raycast(checkPos, Vector2.down, length, groundLayer.value);
    }

    public bool GetIsGround(Vector3 pos, Color lineColor, float length = 1f)
    {
        Vector2 checkPos = pos;

        Debug.DrawLine(checkPos, checkPos + Vector2.down * length, lineColor, 3f);

        return Physics2D.Raycast(checkPos, Vector2.down, length, groundLayer.value);
    }


    #endregion

    #region--決策--

    [SerializeField]
    private float maxSlopeAngle = 60;

    [SerializeField]
    public float maxJumpHeight = 3, timeToHeightest = 0.5f, speed = 3.85f,
        characterHeight = 1;

    [HideInInspector]
    public float gravity = 0, maxJumpVelocity = 12f, jumpVelocity = 0f;


    /// <summary>
    /// 應為一個固定常數，重力不變，跳躍力道改變，來跳到不同高度
    /// </summary>
    public void CalculateMaxValues()
    {
        float value = 2 * maxJumpHeight / (timeToHeightest * timeToHeightest);
        gravity = value;

        value = 2 * maxJumpHeight / timeToHeightest;
        maxJumpVelocity = value;
    }

    /// <summary>
    /// 跳躍的最高位置，因為不可能把目標位置設為最高位置，會撞到平台跳不上去
    /// </summary>
    [HideInInspector]
    public Vector3 jumpHeightestPos;
    public Vector3 SetJumpHeightestPos(Vector3 vectorToNext, float characterHeight = 1, float nodeToPlatformEdge = 1)
    {
        Vector3 vector = Vector3.zero;

        if (vectorToNext.x < 0)
        {
            vector = vectorToNext + new Vector3(-nodeToPlatformEdge, characterHeight);
        }
        else
        {
            vector = vectorToNext + new Vector3(nodeToPlatformEdge, characterHeight);
        }

        return vector;
    }
    [HideInInspector]
    public float timeToH = 0;
    /// <summary>
    /// 目前的方式可能適合往比自己高的位置移動，比自己位置低的還沒有
    /// 可能還要比較水平移動要花多少時間，用時間所需較長的來做這個動作的時間
    /// </summary>
    /// <param name="jumpHeightestPos"></param>
    public void Calculate_TimeOfJump(Vector3 jumpHeightestPos)
    {
        float distance = jumpHeightestPos.y;

        float tSqr = distance * 2 / gravity;

        timeToH = Mathf.Sqrt(tSqr);
    }
    public void Calculate_JumpVelocity(float timeToH)
    {
        jumpVelocity = gravity * timeToH;
    }


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

    public List<DicisionAndTime> dicisionAndTimes = new List<DicisionAndTime>();

    private bool isReset = false;

    public float GetAngle(Vector3 vectorToNext)
    {
        float value = 90 - Vector2.Angle(vectorToNext, Vector2.up);

        return value;
    }

    public void AddTeleporation(Vector3 vectorToNext, Vector3 targetPos)
    {
        DicisionAndTime dicisionAndTime = new DicisionAndTime();

        dicisionAndTime.SetTeleporation(0.05f, vectorToNext, targetPos);

        dicisionAndTimes.Add(dicisionAndTime);
    }
    public void AddWalk(float duration, Vector3 vectorToNext, float speed, Vector3 targetPos)
    {
        DicisionAndTime dicisionAndTime = new DicisionAndTime();

        dicisionAndTime.SetWalk(duration, vectorToNext, speed, targetPos);

        dicisionAndTimes.Add(dicisionAndTime);
    }

    public void AddJump(float duration, Vector3 vectorToNext, float jumpVelocity, float speed, Vector3 targetPos)
    {
        DicisionAndTime dicisionAndTime = new DicisionAndTime();

        dicisionAndTime.SetJump(duration, vectorToNext, jumpVelocity, speed, targetPos);

        dicisionAndTimes.Add(dicisionAndTime);
    }

    public void SetDicisions()
    {
        #region--設定是否重設--

        if (temp.Count != 0)
        {
            isReset = true;
        }

        #endregion


        dicisionAndTimes.Clear();

        AddTeleporation(pathNodes[0].node.Position - transform.position, pathNodes[0].node.Position);
        //瞬移到第一個Node的位置

        //一開始也許不能瞬移
        //而是得出路徑後
        //用當前位置跟路徑的第二個點去做計算
        //這樣才能從那個位置開始移動


        for (int i = 0; i < pathNodes.Count - 1; i++)
        {
            float angle = GetAngle(pathNodes[i].vectorToNext);

            bool isGround = GetIsGround(pathNodes[i].node, pathNodes[i + 1].node);

            Debug.LogWarning("Path Node " + pathNodes[i].node.NodeNum + " to Path Node " + pathNodes[i + 1].node.NodeNum + " , angle : " + angle + " , IsGround : " + isGround);

            Vector3 vectorToNext = pathNodes[i].vectorToNext;


            if (vectorToNext.y >= 0)
            {
                if (isGround)
                {
                    if (angle <= maxSlopeAngle)
                    {
                        //要看坡度是否非0度
                        //如果是的話，要怎麼計算該角度下，當前速度的水平分量跟垂直分量

                        AddWalk(Mathf.Abs(vectorToNext.x) / speed, vectorToNext, ((vectorToNext.x > 0) ? 1 : (vectorToNext.x < 0) ? -1 : 0) * speed, pathNodes[i + 1].node.Position);
                    }
                    else//坡度大於可走坡度
                    {
                        //O=origin原點,H=highest最高點,G=Goal終點
                        float verticleDistance_OtoH = vectorToNext.y + 1;//1只是隨意訂的或角色圖片身高

                        float time_OtoH = Mathf.Sqrt(verticleDistance_OtoH / (gravity / 2));

                        float time_HtoG = Mathf.Sqrt(1 / (gravity / 2));

                        float duration = time_OtoH + time_HtoG;

                        float jumpVelocity = (time_OtoH * gravity);


                        if (vectorToNext.y - 1 <= maxJumpHeight &&//-1是因為上面設定最高點會是Goal+1，確保Goal會是最高點
                            Math.Abs(vectorToNext.x) <= speed * duration)
                        {
                            AddJump(duration, vectorToNext, jumpVelocity, Math.Abs(vectorToNext.x) / duration, pathNodes[i + 1].node.Position);
                        }
                        else
                        {
                            AddTeleporation(pathNodes[i].vectorToNext, pathNodes[i + 1].node.Position);
                        }
                    }
                }
                else
                {

                    float verticleDistance_HtoG = Mathf.Abs(vectorToNext.y) + 1f;

                    float time_GtoH = Mathf.Sqrt(verticleDistance_HtoG / (gravity / 2f));

                    float time_HtoO = Mathf.Sqrt(1f / (gravity / 2f));

                    float duration = time_GtoH + time_HtoO;

                    float jumpV = (time_GtoH * gravity);


                    if (verticleDistance_HtoG < maxJumpHeight &&
                        Math.Abs(vectorToNext.x) < speed * duration)
                    {
                        AddJump(duration, vectorToNext, jumpV, Math.Abs(vectorToNext.x) / duration, pathNodes[i + 1].node.Position);

                    }
                    else
                    {
                        AddTeleporation(pathNodes[i].vectorToNext, pathNodes[i + 1].node.Position);
                    }
                }
            }
            else if (vectorToNext.y < 0)
            {
                if (isGround)
                {
                    if (angle <= maxSlopeAngle)
                    {
                        Debug.Log(346 + " , " + Mathf.Abs(vectorToNext.x) / speed);
                        AddWalk(Mathf.Abs(vectorToNext.x) / speed, vectorToNext, ((vectorToNext.x > 0) ? 1 : (vectorToNext.x < 0) ? -1 : 0) * speed, pathNodes[i + 1].node.Position);
                    }
                    else
                    {
                        AddTeleporation(pathNodes[i].vectorToNext, pathNodes[i + 1].node.Position);
                    }
                }
                else
                {
                    float time_WalktoEdge = 1f / speed;

                    float time_OtoG_y = Mathf.Sqrt(Mathf.Abs(vectorToNext.y) / (gravity / 2));
                    //水平移動落下所需時間

                    Vector3 fallPos = pathNodes[i].node.Position + new Vector3(time_OtoG_y * ((vectorToNext.x > 0) ? 1 : (vectorToNext.x < 0) ? -1 : 0) * speed, vectorToNext.y);

                    bool isPosGround = GetIsGround(fallPos, Color.white, 3);

                    bool isGroundConnect = GetIsGround((fallPos + pathNodes[i + 1].node.Position) / 2, Color.yellow, 3);


                    Debug.Log("IsGround : " + isPosGround + " , " + isGroundConnect);


                    //float time_OtoH_y = Mathf.Sqrt(maxJumpHeight / (gravity / 2f));

                    //float time_GtoH_y = Mathf.Sqrt(maxJumpHeight + vectorToNext.y / (gravity / 2f));

                    //float duration = time_OtoH_y + time_GtoH_y;


                    float time_OtoG_x = (Mathf.Abs(vectorToNext.x) / speed);


                    Debug.Log("time_OtoG_y : " + time_OtoG_y + " , time_OtoG_x : " + time_OtoG_x);

                    if (Mathf.Abs(vectorToNext.x) <= time_OtoG_y * speed ||
                        (isPosGround && isGroundConnect))
                    {
                        //AddWalk(time_OtoG_x, vectorToNext,);
                        float speed_OtoG = vectorToNext.x / (time_OtoG_y + time_WalktoEdge);
                        float speed_withDir = ((vectorToNext.x > 0) ? 1 : (vectorToNext.x < 0) ? -1 : 0) * speed;

                        //AddWalk(time_OtoG_y + time_WalktoEdge, vectorToNext, (Math.Abs( speed_OtoG) >= Math.Abs(speed_withDir)) ? speed_withDir : speed_OtoG);
                        AddWalk(time_OtoG_y + time_WalktoEdge, vectorToNext, speed_OtoG, pathNodes[i + 1].node.Position);
                        //AddWalk(time_OtoG_x, vectorToNext, speed_withDir);
                        //有些落下的時間有點怪怪的
                        //我想是沒有算到走到平台邊緣的時間嗎
                    }
                    //跳躍的條件還要想想
                    //else if (Mathf.Abs(vectorToNext.x) > time_OtoG_y * speed
                    //    && Mathf.Abs(vectorToNext.x) <= duration * speed)
                    //{

                    //    float jumpVelocity = (Mathf.Abs(vectorToNext.y) / time_OtoG_x) + gravity * time_OtoG_x;
                    //也還沒有限制最大跳躍速度
                    //    Debug.Log("jumpVelocity : " + jumpVelocity+" maxJumpVelocity : "+maxJumpVelocity);

                    //    AddJump(time_OtoG_x, vectorToNext, jumpVelocity, speed);
                    //}
                    else
                    {
                        AddTeleporation(pathNodes[i].vectorToNext, pathNodes[i + 1].node.Position);
                    }

                }
            }
        }

        StartCoroutine(enumerator_Move());
    }

    #endregion

    #region--運動--

    Rigidbody2D rb2D;

    public void SetVariable_Act()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
    public void Move(Vector3 targetPos)
    {
        transform.position = targetPos;
    }
    public void Move(float speed, Vector3 targetPos)
    {
        Vector2 originVelocity = rb2D.velocity;

        rb2D.velocity = new Vector2(speed, originVelocity.y);
        animator.SetBool("IsWalk", true);
    }
    public void Move(float speed, float jumpVelocity, Vector3 targetPos)
    {
        animator.SetBool("IsWalk", true);
        rb2D.velocity = new Vector2(((targetPos.x > transform.position.x) ? 1 : -1) * speed, jumpVelocity);
    }
    public void ResetVelocity()
    {
        rb2D.velocity = Vector2.zero;

        animator.SetBool("IsWalk", false);
    }

    public void AddGravity()
    {
        rb2D.AddForce(new Vector2(0, -gravity));
    }

    Animator animator;

    #endregion


    private void Start()
    {
        SetVariable_Detect();

        SetVariable_Act();

        CalculateMaxValues();

        animator = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        GetPathNodes();
    }

    private List<DicisionAndTime> temp = new List<DicisionAndTime>();
    IEnumerator enumerator_Move()
    {
        temp.Clear();

        for (int i = 0; i < dicisionAndTimes.Count; i++)
        {
            temp.Add(dicisionAndTimes[i]);
        }

        temp.Reverse();


        while (temp.Count != 0)
        {
            DicisionAndTime dicisionAndTime = temp[temp.Count - 1];

            float time = 0, duration = dicisionAndTime.duration;

            if (dicisionAndTime.dicision == E_Dicision.teleportation)
            {
                Move(transform.position + dicisionAndTime.vectorToNext);

                ChangeImgDir((dicisionAndTime.vectorToNext.x > 0) ? false : (dicisionAndTime.vectorToNext.x < 0) ? true : spriteRenderer.flipX);

                while (time < duration)
                {
                    yield return new WaitForEndOfFrame();
                    time += Time.deltaTime;
                }
                Debug.DrawLine(transform.position, transform.position + Vector3.right, Color.magenta, 5);

                ResetVelocity();
            }
            else if (dicisionAndTime.dicision == E_Dicision.walk)
            {
                while (time < duration)
                {
                    Move(dicisionAndTime.speed, transform.position + dicisionAndTime.vectorToNext);
                    ChangeImgDir();


                    yield return new WaitForEndOfFrame();
                    time += Time.deltaTime;

                    if (isReset)
                    {
                        Debug.LogError("Reset_1");
                        isReset = false;
                        yield break;
                    }
                }
                Move(dicisionAndTime.targetPos);

                Debug.DrawLine(transform.position, transform.position + Vector3.right, Color.magenta, 5);

                ResetVelocity();
            }
            else if (dicisionAndTime.dicision == E_Dicision.jump)
            {

                Move(dicisionAndTime.speed, dicisionAndTime.jumpVelocity, transform.position + dicisionAndTime.vectorToNext);
                ChangeImgDir();

                while (time < duration)
                {
                    yield return new WaitForEndOfFrame();
                    time += Time.deltaTime;

                    if (isReset)
                    {
                        Debug.LogError("Reset_2");
                        isReset = false;
                        yield break;
                    }
                }
                Debug.DrawLine(transform.position, transform.position + Vector3.right, Color.magenta, 5);
                Move(dicisionAndTime.targetPos);
                ResetVelocity();
            }

            temp.RemoveAt(temp.Count - 1);
        }
    }

    private void FixedUpdate()
    {
        AddGravity();
    }


    public void ChangeImgDir()
    {
        if (rb2D.velocity.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (rb2D.velocity.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }
    public void ChangeImgDir(bool value)
    {
        spriteRenderer.flipX = value;
    }

}
