using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
public class AI_Move : MonoBehaviour
{
    #region--�P��--

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

    #region--�M��--

    [SerializeField]
    private float maxSlopeAngle = 60;

    [SerializeField]
    public float maxJumpHeight = 3, timeToHeightest = 0.5f, speed = 3.85f,
        characterHeight = 1;

    [HideInInspector]
    public float gravity = 0, maxJumpVelocity = 12f, jumpVelocity = 0f;


    /// <summary>
    /// �����@�өT�w�`�ơA���O���ܡA���D�O�D���ܡA�Ӹ��줣�P����
    /// </summary>
    public void CalculateMaxValues()
    {
        float value = 2 * maxJumpHeight / (timeToHeightest * timeToHeightest);
        gravity = value;

        value = 2 * maxJumpHeight / timeToHeightest;
        maxJumpVelocity = value;
    }

    /// <summary>
    /// ���D���̰���m�A�]�����i���ؼЦ�m�]���̰���m�A�|���쥭�x�����W�h
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
    /// �ثe���覡�i��A�X����ۤv������m���ʡA��ۤv��m�C���٨S��
    /// �i���٭n���������ʭn��h�֮ɶ��A�ήɶ��һݸ������Ӱ��o�Ӱʧ@���ɶ�
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
        #region--�]�w�O�_���]--

        if (temp.Count != 0)
        {
            isReset = true;
        }

        #endregion


        dicisionAndTimes.Clear();

        AddTeleporation(pathNodes[0].node.Position - transform.position, pathNodes[0].node.Position);
        //������Ĥ@��Node����m

        //�@�}�l�]�\��������
        //�ӬO�o�X��|��
        //�η�e��m���|���ĤG���I�h���p��
        //�o�ˤ~��q���Ӧ�m�}�l����


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
                        //�n�ݩY�׬O�_�D0��
                        //�p�G�O���ܡA�n���p��Ө��פU�A��e�t�ת�������q�򫫪����q

                        AddWalk(Mathf.Abs(vectorToNext.x) / speed, vectorToNext, ((vectorToNext.x > 0) ? 1 : (vectorToNext.x < 0) ? -1 : 0) * speed, pathNodes[i + 1].node.Position);
                    }
                    else//�Y�פj��i���Y��
                    {
                        //O=origin���I,H=highest�̰��I,G=Goal���I
                        float verticleDistance_OtoH = vectorToNext.y + 1;//1�u�O�H�N�q���Ψ���Ϥ�����

                        float time_OtoH = Mathf.Sqrt(verticleDistance_OtoH / (gravity / 2));

                        float time_HtoG = Mathf.Sqrt(1 / (gravity / 2));

                        float duration = time_OtoH + time_HtoG;

                        float jumpVelocity = (time_OtoH * gravity);


                        if (vectorToNext.y - 1 <= maxJumpHeight &&//-1�O�]���W���]�w�̰��I�|�OGoal+1�A�T�OGoal�|�O�̰��I
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
                    //������ʸ��U�һݮɶ�

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
                        //���Ǹ��U���ɶ����I�ǩǪ�
                        //�ڷQ�O�S����쨫�쥭�x��t���ɶ���
                    }
                    //���D�������٭n�Q�Q
                    //else if (Mathf.Abs(vectorToNext.x) > time_OtoG_y * speed
                    //    && Mathf.Abs(vectorToNext.x) <= duration * speed)
                    //{

                    //    float jumpVelocity = (Mathf.Abs(vectorToNext.y) / time_OtoG_x) + gravity * time_OtoG_x;
                    //�]�٨S������̤j���D�t��
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

    #region--�B��--

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
