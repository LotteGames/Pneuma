using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownPike : MonoBehaviour
{
    [Header("平台")]
    public GameObject MoveGround;
    //[Header("要移動的平台")]
    //public GameObject[] Grounds;

    [Header("繩索兩端")]
    public GameObject PosA;
    public GameObject PosB;

    [Header("是否下墜?")]
    public bool IsDown;
    [Header("是否可以下墜?")]
    public bool CanDown;
    //[Header("所在位置")]
    //public float Pos;
    [Header("下降速度")]
    public float DownSpeed;
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
        MoveGround.GetComponent<MoveGround>().MoveSpeed = 0;
        MoveGround.GetComponent<MoveGround>().NewPos = MoveGround.GetComponent<MoveGround>().MovePos[0];
        MoveGround.transform.position = PosA.transform.position;
        //MoveGround.GetComponent<Collider2D>().enabled = false;
        //MoveGround.GetComponent<Rigidbody2D>().gravityScale = 0;
        //MoveGround.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        //MoveGround.GetComponent<TouchGround>().CanIn = false;
        IsDown = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (IsDown == false)
        {
            if (MoveGround.GetComponent<MoveGround>().NewPos == MoveGround.GetComponent<MoveGround>().MovePos[0])//往上升
            {
                MoveGround.GetComponent<MoveGround>().MoveSpeed = UpSpeed;

                if(MoveGround.transform.position.y >= PosA.transform.position.y - 0.5f)
                {
                    MoveGround.tag = "Pike";
                }
                else
                {
                    MoveGround.tag = "Ground";
                }

                //
                //MoveGround.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                //pos += new Vector3(0, UpSpeed * Time.deltaTime, 0);
                //Vector3 A = pos; 
                //float B = PosA.transform.position.y - pos.y;
                //MoveGround.GetComponent<Rigidbody2D>().MovePosition(pos);


                //if (B <= 0.6f)
                //{
                //    MoveGround.tag = "Pike";
                //}
                //else
                //{
                //    MoveGround.tag = "Ground";
                //}
                //

            }
            else//固定了
            {
                //MoveGround.transform.position = PosA.transform.position;
                MoveGround.GetComponent<MoveGround>().MoveSpeed = 0;
                //MoveGround.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                if(CanDown == false)
                {
                    MoveGround.tag = "Pike";
                    CanDown = true;
                }
            }
        }
        else//往下掉
        {
          

            MoveGround.tag = "Ground";
            ////MoveGround.GetComponent<TouchGround>().CanIn = false;

            if (MoveGround.GetComponent<MoveGround>().NewPos == MoveGround.GetComponent<MoveGround>().MovePos[0])
            {
                if (MoveGround.GetComponent<MoveGround>().MoveSpeed != 0)
                {
                    StartCoroutine(DelayUp(UpDelay));
                }
                MoveGround.GetComponent<MoveGround>().MoveSpeed = 0;

                //MoveGround.GetComponent<Collider2D>().enabled = true;
                //MoveGround.GetComponent<Collider2D>().isTrigger = false;
                //MoveGround.GetComponent<Rigidbody2D>().gravityScale = 0;
                //MoveGround.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                //pos = PosB.transform.position;
            }
            else
            {
                DownSpeed += Time.deltaTime * 500f;
                MoveGround.GetComponent<MoveGround>().MoveSpeed = DownSpeed;
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
                ////MoveGround.transform.position = PosA.transform.position;
                //MoveGround.GetComponent<Collider2D>().enabled = false;
                //MoveGround.GetComponent<Collider2D>().isTrigger = true;
                ////MoveGround.GetComponent<Rigidbody2D>().gravityScale = 4f;
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
        MoveGround.GetComponent<MoveGround>().MoveSpeed = 0;
        yield return new WaitForSeconds(DelayTime);
        IsDown = false;
    }
    public IEnumerator DelayDown(float DelayTime)
    {
        DownSpeed = 5;
        MoveGround.GetComponent<MoveGround>().MoveSpeed = 0;
        yield return new WaitForSeconds(DelayTime);
        IsDown = true;
        CanDown = false;
        DownSpeed = 20;
        MoveGround.GetComponent<MoveGround>().MoveSpeed = DownSpeed;
        //MoveGround.transform.position = PosA.transform.position;
        //MoveGround.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        //MoveGround.GetComponent<Rigidbody2D>().gravityScale = 4;
    }
}
