using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownPike : MonoBehaviour
{
    [Header("平台")]
    public GameObject MoveGround;
    [Header("要移動的平台")]
    public GameObject[] Grounds;

    [Header("繩索兩端")]
    public GameObject PosA;
    public GameObject PosB;

    [Header("是否下墜?")]
    public bool IsDown;
    [Header("是否可以下墜?")]
    public bool CanDown;
    //[Header("所在位置")]
    //public float Pos;

    [Header("上升速度")]
    public float UpSpeed;

    [Header("上升延遲")]
    public float UpDelay;
    [Header("掉落延遲")]
    public float DownDelay;

    Vector3 pos;
    public void Start()
    {
        CanDown = true;
        MoveGround.GetComponent<Collider2D>().enabled = false;
        MoveGround.GetComponent<Rigidbody2D>().gravityScale = 0;
        MoveGround.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        IsDown = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (IsDown == false)
        {
            if (pos.y < PosA.transform.position.y)
            {
                MoveGround.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                //pos += new Vector3(0, UpSpeed, 0);
                pos += new Vector3(0, UpSpeed * Time.deltaTime, 0);
                Vector3 A = pos; 
                float B = PosA.transform.position.y - pos.y;
                MoveGround.GetComponent<Rigidbody2D>().MovePosition(pos);
                //MoveGround.GetComponent<Rigidbody2D>().velocity = pos;


                if (B <= 0.6f)
                {
                    MoveGround.tag = "Pike";
                }
                else
                {
                    MoveGround.tag = "Ground";
                }
                //for (int i = 0; i < Grounds.Length; i++)
                //{
                //    Grounds[i].transform.position += new Vector3(0, UpSpeed * Time.deltaTime, 0);
                //}
            }
            else
            {
                MoveGround.transform.position = PosA.transform.position;
                MoveGround.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                CanDown = true;
                MoveGround.tag = "Pike";
            }
        }
        else
        {
            MoveGround.tag = "Ground";

            if (Grounds[0].transform.position.y <= PosB.transform.position.y)
            {
                MoveGround.GetComponent<Collider2D>().enabled = true;
                MoveGround.GetComponent<Collider2D>().isTrigger = false;
                MoveGround.GetComponent<Rigidbody2D>().gravityScale = 0;
                MoveGround.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                pos = PosB.transform.position;
                StartCoroutine(DelayUp(UpDelay));
            }
        }
    }


    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "CatBody")
    //    {
    //        if(CanDown == true)
    //        {
    //            StartCoroutine(DelayUp(DownDelay));

    //            MoveGround.GetComponent<Collider2D>().enabled = false;
    //            MoveGround.GetComponent<Collider2D>().isTrigger = true;
    //            MoveGround.GetComponent<Rigidbody2D>().gravityScale = 5;
    //            //for (int i = 0; i < Grounds.Length; i++)
    //            //{
    //            //    Grounds[i].GetComponent<Rigidbody2D>().gravityScale = 5;
    //            //}
    //            CanDown = false;
    //        }
    //    }
    //}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "CatBody")
        {
            if (CanDown == true)
            {
                CanDown = false;
                MoveGround.tag = "Ground";
                //MoveGround.transform.position = PosA.transform.position;
                MoveGround.GetComponent<Collider2D>().enabled = false;
                MoveGround.GetComponent<Collider2D>().isTrigger = true;
                //MoveGround.GetComponent<Rigidbody2D>().gravityScale = 4f;
                StartCoroutine(DelayDown(DownDelay));
                //for (int i = 0; i < Grounds.Length; i++)
                //{
                //    Grounds[i].GetComponent<Rigidbody2D>().gravityScale = 5;
                //}
            }
        }
    }

    public IEnumerator DelayUp(float DelayTime)
    {
        yield return new WaitForSeconds(DelayTime);
        IsDown = false;
    }
    public IEnumerator DelayDown(float DelayTime)
    {
        yield return new WaitForSeconds(DelayTime);
        MoveGround.transform.position = PosA.transform.position;
        IsDown = true;
        MoveGround.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        MoveGround.GetComponent<Rigidbody2D>().gravityScale = 4;
        CanDown = false;
    }
}
