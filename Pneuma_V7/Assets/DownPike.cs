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
            if (Grounds[0].transform.position.y <= PosA.transform.position.y)
            {
                MoveGround.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                pos += new Vector3(0, UpSpeed * Time.deltaTime, 0);
                MoveGround.GetComponent<Rigidbody2D>().MovePosition(pos);
                //for (int i = 0; i < Grounds.Length; i++)
                //{
                //    Grounds[i].transform.position += new Vector3(0, UpSpeed * Time.deltaTime, 0);
                //}
            }
            else
            {
                MoveGround.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                CanDown = true;
            }
        }
        else
        {
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
                StartCoroutine(DelayDown(DownDelay));
                CanDown = false;
                MoveGround.GetComponent<Collider2D>().enabled = false;
                MoveGround.GetComponent<Collider2D>().isTrigger = true;
                MoveGround.GetComponent<Rigidbody2D>().gravityScale = 4f;
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
        IsDown = true;
        MoveGround.GetComponent<Rigidbody2D>().gravityScale = 4;
        CanDown = false;
    }
}
