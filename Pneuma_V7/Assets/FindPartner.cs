using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPartner : MonoBehaviour
{
    [Header("CatContrl")]
    public CatContrl catContrl;

    [Header("面向的夥伴")]
    public GameObject Partner;


    [Header("面向的角度")]
    public float TurnRun;


    [Header("另一個方向的臉")]
    public Sprite Pictrue_1;
    public Sprite Pictrue_2;
    //public Sprite Pictrue_Idle1;
    //public Sprite Pictrue_Idle2;
    [Header("回去的BUG")]
    public bool DebugBack;

    // Start is called before the first frame update
    void Start()
    {
        catContrl = GameObject.FindObjectOfType<CatContrl>();
        Partner = GameObject.FindObjectOfType<CatContrl>().gameObject;
        StartCoroutine(DebugLong(1.1f));
    }

    // Update is called once per frame
    void Update()
    {
        Turn();
        PartnerPos();

        if (catContrl.NowCatAct == CatContrl.CatAct.LongLongCat)
        {
            LongLongCat();
        }
        else
        {
            FallowHand();
        }
        //else if (catContrl.NowCatAct == CatContrl.CatAct.Back || catContrl.NowCatAct == CatContrl.CatAct.CatDie)
        //{
        //    FallowHand();
        //}
        if (catContrl.NowCatAct == CatContrl.CatAct.CatDie)
        {
            catContrl.GetComponent<Rigidbody2D>().gravityScale = 6.5f;//貓咪屁股的重力恢復
            catContrl.GetComponent<Animator>().SetBool("Long", false);
            catContrl.NowCatAct = CatContrl.CatAct.Jump;
            if (catContrl.TurnRight == true)
            {
                Partner.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                Partner.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            Destroy(transform.parent.gameObject);
        }

    }

    void Turn()
    {
        Vector3 worldPos = Partner.transform.position;
        Vector3 direction = worldPos - transform.position;


        direction.z = 0f;
        direction.Normalize();
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, targetAngle), TurnSpeed * Time.deltaTime);

        //GetComponent<Rigidbody2D>().MoveRotation(Quaternion.Euler(0, 0, targetAngle - TurnRun));
        transform.rotation = Quaternion.Euler(0, 0, targetAngle - TurnRun);
     
    }

    //void PartnerPos()
    //{
    //    Vector3 worldPos = Partner.transform.position;
    //    if (catContrl.ButtRemove <= catContrl.ButtRemoveMin && catContrl.NowCatAct != OldCatContrl.CatAct.Jump)
    //    {
    //        if (worldPos.x <= transform.position.x)
    //        {
    //            if (Pictrue_Idle1 != null)
    //            {
    //                GetComponent<SpriteRenderer>().sprite = Pictrue_Idle1;
    //            }
    //            else
    //            {
    //                GetComponent<SpriteRenderer>().sprite = null;
    //            }
    //        }
    //        else
    //        {
    //            if (Pictrue_Idle1 != null)
    //            {
    //                GetComponent<SpriteRenderer>().sprite = Pictrue_Idle2;
    //            }
    //            else
    //            {
    //                GetComponent<SpriteRenderer>().sprite = null;
    //            }
    //        }
    //    }
    //    else if (catContrl.NowCatAct != OldCatContrl.CatAct.Idle)
    //    {
    //        if (worldPos.x <= transform.position.x)
    //        {
    //            GetComponent<SpriteRenderer>().sprite = Pictrue_1;
    //        }
    //        else
    //        {
    //            GetComponent<SpriteRenderer>().sprite = Pictrue_2;
    //        }
    //    }

    //}

    void PartnerPos()
    {
        Vector3 worldPos = Partner.transform.position;

        //if (worldPos.x <= transform.position.x)
        //{
        //    if (Pictrue_Idle1 != null)
        //    {
        //        GetComponent<SpriteRenderer>().sprite = Pictrue_Idle1;
        //    }
        //    else
        //    {
        //        GetComponent<SpriteRenderer>().sprite = null;
        //    }
        //}
        //else
        //{
        //    if (Pictrue_Idle1 != null)
        //    {
        //        GetComponent<SpriteRenderer>().sprite = Pictrue_Idle2;
        //    }
        //    else
        //    {
        //        GetComponent<SpriteRenderer>().sprite = null;
        //    }
        //}

        if (worldPos.x <= transform.position.x)
        {
            GetComponent<SpriteRenderer>().sprite = Pictrue_1;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = Pictrue_2;
        }


    }

    public void LongLongCat()//很長很長的貓
    {
        Vector3 MousePos;


        MousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

        //
        Vector3 direction = MousePos - Partner.transform.position;
        direction.z = 0f;
        direction.Normalize();
        //float targetAngle = Mathf.Atan2(direction.y, direction.x);

        Vector3 RemoveMax = Partner.transform.position + direction * catContrl.LongRemoveMax;

        float ButtRemove = Vector2.Distance(Partner.transform.position, MousePos);



        if (ButtRemove <= catContrl.LongRemoveMax)
        {
            //GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(transform.position, MousePos, HandSpeed));
            GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(transform.position, MousePos, 0.2f));
            //transform.position = Vector2.Lerp(transform.position, MousePos, 0.2f);
        }
        else
        {
            //GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(transform.position, RemoveMax, HandSpeed));
            GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(transform.position, RemoveMax, 0.2f));
            //transform.position = Vector2.Lerp(transform.position, RemoveMax, 0.2f);
        }
        if (catContrl.TurnRight == true)
        {
            GetComponent<Animator>().SetFloat("TurnRight", 1);
        }
        else
        {
            GetComponent<Animator>().SetFloat("TurnRight", 0);
        }


    }

    public void FallowHand()//頭部縮回去
    {
        float ButtRemove = Vector2.Distance(transform.position, Partner.transform.position);
        GetComponent<Animator>().SetBool("Back", true);
        if (DebugBack == true)
        {
            GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(transform.position, Partner.transform.position, 0.2f));
        }

        if (catContrl.TurnRight == true)
        {
            GetComponent<Animator>().SetFloat("TurnRight", 1);
        }
        else
        {
            GetComponent<Animator>().SetFloat("TurnRight", 0);
        }

        if (ButtRemove <= catContrl.LongRemoveMin)
        {
            catContrl.GetComponent<Rigidbody2D>().gravityScale = 6.5f;//貓咪屁股的重力恢復
            catContrl.GetComponent<Animator>().SetBool("Long", false);
            catContrl.NowCatAct = CatContrl.CatAct.Jump;
            if(catContrl.TurnRight == true)
            {
                Partner.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                Partner.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            Destroy(transform.parent.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground" || collision.gameObject.tag == "DoorGround" || collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Pike")
        {
            catContrl.CanJump = true;
            catContrl.NowCatAct = CatContrl.CatAct.Back;
            catContrl.GetComponent<Collider2D>().isTrigger = false;
            StartCoroutine(DebugLong(0.2f));
        }
    }

    public IEnumerator DebugLong(float DelayTime)
    {
        yield return new WaitForSeconds(DelayTime);
        GetComponent<Collider2D>().isTrigger = true;
        DebugBack = true;
    }
}
