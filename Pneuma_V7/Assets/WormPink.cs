using UnityEngine;
using DG.Tweening;
public class WormPink : MonoBehaviour
{

    public enum E_PinkState
    {
        Idle,
        Fall,
        Stay,
        Back,
        BeSteppedOn,
    }
    public enum Ani_Parameter
    {
        BeSteppedOn,
        IsFall,
        IsStay,
        IsBack,
        IsReset,
    }

    public E_PinkState pinkState;

    private CameraActivate cameraActivate;

    private CurrentAea currentArea;

    private int areaNum;

    private Vector3 startPos,silkStartPos;

    public Rigidbody2D rb2D,silkRb2D;

    public Animator animator;

    public float fallDistance = 0;
    private float colliderSizeY;

    public LayerMask LayerCanStand;


    public float fallTime = 1, idleTime = 1f, stayTime, backTime, resetTime = 1;
    private float passedTime = 0;

    public ContactFilter2D catFileter;


    public SpriteRenderer spR_Silk;
    public Vector2 silkOriginSize = new Vector2(1, 1f);

    public enum E_Use
    {
        Time,
        Duration,
    }

    private void Start()
    {
        cameraActivate = FindObjectOfType<CameraActivate>();
        currentArea = FindObjectOfType<CurrentAea>();
        startPos = transform.position;
        silkStartPos = silkRb2D.transform.position;

        areaNum = currentArea.GetCurrentArea(transform.parent.position);

        GetFallDistance();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        if (!Application.isPlaying)
        {
            Vector3 pos = transform.position;

            void DrawFallPath(Vector3 pos)
            {
                Gizmos.DrawLine(pos, pos + new Vector3(0, -fallDistance));
            }
            DrawFallPath(pos);


            Rect rect = GetComponentInChildren<SpriteRenderer>().sprite.rect;

            Vector2 spriteSize = new Vector2(rect.width / 100f, rect.height / 100f);

            Debug.LogError(spriteSize.ToString());

            void DrawFallPos(Vector3 pos)
            {
                Vector3 fallPos = pos + new Vector3(0, -fallDistance);
                Gizmos.DrawLine(fallPos + new Vector3(-spriteSize.x / 2f, +spriteSize.y / 2f), fallPos + new Vector3(spriteSize.x / 2f, +spriteSize.y / 2f));
                Gizmos.DrawLine(fallPos + new Vector3(-spriteSize.x / 2f, -spriteSize.y / 2f), fallPos + new Vector3(spriteSize.x / 2f, -spriteSize.y / 2f));
                Gizmos.DrawLine(fallPos + new Vector3(-spriteSize.x / 2f, +spriteSize.y / 2f), fallPos + new Vector3(-spriteSize.x / 2f, -spriteSize.y / 2f));
                Gizmos.DrawLine(fallPos + new Vector3(+spriteSize.x / 2f, +spriteSize.y / 2f), fallPos + new Vector3(spriteSize.x / 2f, -spriteSize.y / 2f));
            }
            DrawFallPos(pos);
        }
    }

    private void OnValidate()
    {
        if (fallDistance <= 0)
        {
            fallDistance = 0;
        }
    }

    public void GetFallDistance()
    {
        colliderSizeY = GetComponent<BoxCollider2D>().size.y;

        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, Vector2.down, float.PositiveInfinity, LayerCanStand);

        float distanceToGroud = raycastHit2D.distance - (colliderSizeY / 2f);

        fallDistance = (distanceToGroud > fallDistance) ? fallDistance : distanceToGroud;
    }

    private void Update()
    {
        Behaviour();
    }

    public void Behaviour()
    {
        switch (pinkState)
        {
            case E_PinkState.Idle:
                {
                    StateBehav(Ani_Parameter.IsFall, Vector2.zero, idleTime, E_PinkState.Fall, true);
                    break;
                }
            case E_PinkState.Fall:
                {
                    StateBehav(Ani_Parameter.IsFall, new Vector2(0, -fallDistance / fallTime), fallTime, E_PinkState.Stay);
                    SetSilkSize();
                    break;
                }
            case E_PinkState.Stay:
                {
                    StateBehav(Ani_Parameter.IsStay, Vector2.zero, stayTime, E_PinkState.Back);
                    break;
                }
            case E_PinkState.Back:
                {
                    StateBehav(Ani_Parameter.IsBack, new Vector2(0, fallDistance / backTime), backTime, E_PinkState.Idle);
                    SetSilkSize();
                    break;
                }
            case E_PinkState.BeSteppedOn:
                {
                    StateBehav_BeSteppedOn();
                    break;
                }
        }
    }

    public void SetAnimator_Bool(Ani_Parameter ani_Parameter, bool value)
    {
        animator.SetBool(ani_Parameter.ToString(), value);
    }
    public void SetAnimator_Trigger(Ani_Parameter ani_Parameter)
    {
        animator.SetTrigger(ani_Parameter.ToString());
    }

    public void SetSilkSize()
    {
        spR_Silk.size += new Vector2(0, -rb2D.velocity.y * Time.deltaTime);
    }

    public void StateBehav(Ani_Parameter currentAniState, Vector2 speed, float duration, E_PinkState nextState, bool isIdle = false)
    {
        if (cameraActivate.CurrentCam_AreaNum == areaNum)
        {
            if (!isIdle)
            {
                SetAnimator_Bool(currentAniState, true);
            }
            passedTime += Time.deltaTime;

            rb2D.velocity = speed;


            if (passedTime >= duration)
            {
                SetAnimator_Bool(currentAniState, false);

                passedTime = 0;

                SetState(nextState);
            }
        }
        else
        {
            ResetWorm();
        }
    }

    public void StateBehav_BeSteppedOn()
    //讓Silk跟著掉落
    {
        SetAnimator_Bool(Ani_Parameter.IsBack,false);
        SetAnimator_Bool(Ani_Parameter.IsStay, false);
        SetAnimator_Bool(Ani_Parameter.IsFall, false);

        if (GetComponent<Collider2D>().isTrigger == false)
        {
            rb2D.velocity = Vector2.zero;
            GetComponent<Collider2D>().isTrigger = true;
            rb2D.velocity = new Vector2(0, 15);//改在FixedUpdate裡
        }
        //silkRb2D.velocity = rb2D.velocity;
        rb2D.AddForce(new Vector2(0, -37.5f));

    
        SetAnimator_Trigger(Ani_Parameter.BeSteppedOn);

        passedTime += Time.deltaTime;

        if (passedTime >= resetTime)
        {
            ResetWorm();

            GetComponent<Collider2D>().isTrigger = false;

            SetAnimator_Trigger(Ani_Parameter.IsReset);

            passedTime = 0;

            SetState(E_PinkState.Idle);
        }
    }

    public void ResetWorm()
    {
        //silkRb2D.velocity = Vector2.zero;
        //silkRb2D.transform.position = silkStartPos;

        transform.position = startPos;
        rb2D.velocity = Vector2.zero;
        spR_Silk.size = silkOriginSize;
        passedTime = 0;
        SetState(E_PinkState.Idle);
    }

    public void SetState(E_PinkState e_PinkState)
    {
        pinkState = e_PinkState;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<CatContrl>())
        {
            if (rb2D.IsTouching(catFileter))
            {
                SetState(E_PinkState.BeSteppedOn);
            }
        }
    }


}
