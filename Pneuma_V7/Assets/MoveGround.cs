using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGround : MonoBehaviour
{
    //[Header("跟隨點")]
    //public GameObject FollowPos;
    [Header("位移點")]
    public GameObject[] MovePos;
    [Header("移動速度")]
    public float MoveSpeed;


    private GameObject OldPos;
    private GameObject NewPos;
    private int MovePosNumber = 0;//跟隨哪個座標
    private float MovePosTime = 0;//(0~1)
    private CatContrl Cat;

    [Header("貓咪是否在上面")]
    public bool CatOn;

    [Header("是否是陷阱")]
    public bool IsPike;

    // Start is called before the first frame update
    void Start()
    {
        Cat = GameObject.FindObjectOfType<CatContrl>();
        transform.position = MovePos[0].transform.position;
        OldPos = MovePos[0];
        NewPos = MovePos[1];
        MovePosNumber = 1;
    }

    // Update is called once per frame
    void Update()
    {
        MovePosTime += Time.deltaTime * 0.1f * MoveSpeed;

        if(MovePosTime >= 1f)
        {
            OldPos = MovePos[MovePosNumber];

            MovePosNumber++;
            if(MovePosNumber >= MovePos.Length)
            {
                MovePosNumber = 0;
            }
            NewPos = MovePos[MovePosNumber];

            MovePosTime = 0;
        }


        //GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(OldPos.transform.position, NewPos.transform.position, MovePosTime));
        //GetComponent<Rigidbody2D>().MovePosition(FollowPos.transform.position);
        //if (CatOn == true)
        //{
        //    GetComponent<Rigidbody2D>().MovePosition(FollowPos.transform.position);
        //}
        //else
        //{
        //    transform.position = Vector2.Lerp(transform.position, FollowPos.transform.position, 0.2f);
        //}
        transform.position = Vector2.Lerp(OldPos.transform.position, NewPos.transform.position, MovePosTime);

        //if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) + Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) >= 20)
        //{
        //    GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        //}
        if (Cat.gameObject.transform.parent == null)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            CatOn = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<CatContrl>() != null)
        {
            if(IsPike == false)
            {
                collision.transform.parent = transform;
                collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                CatOn = true;
            }
        }
    }

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (collision.gameObject.GetComponent<CatContrl>() != null)
    //    {
    //        collision.transform.parent = transform.parent;
    //        collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
    //        CatOn = true;
    //    }
    //}

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<CatContrl>() != null)
        {
            if (IsPike == false)
            {
                collision.transform.parent = null;
                collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = collision.gameObject.GetComponent<CatContrl>().CatWeight;
                //GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                CatOn = false;
            }
        }
    }
}
