using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OldCatContrl : MonoBehaviour
{
    [Header("屁股")]
    public GameObject Butt;
    [Header("屁股距離頭的位置")]
    public float ButtRemove;
    [Header("屁股距離頭的位置(最近距離限制)")]
    public float ButtRemoveMin;
    [Header("屁股距離頭的位置(最遠距離限制)")]
    public float ButtRemoveMax;
    [Header("屁股移動速度(0~1)")]
    public float SetButtMoveSpeed;
    [Header("屁股移動速度(0~1)")]
    public float ButtMoveSpeed;

    [Header("屁股重量")]
    public float ButtH;
    [Header("頭重量")]
    public float HandH;

    [Header("貓咪的身體")]
    public GameObject CatBody;

    [Header("移動速度")]
    public float MoveSpeed;
    [Header("跳躍力道")]
    public float JumpPower;

    [Header("延伸速度")]
    public float HandSpeed;
    [Header("轉頭速度")]
    public float HandTurnSpeed;


    [Header("向下延伸距離")]
    public float DownRemove;
    [Header("向下延伸距離極限")]
    public float DownRemove_max;
    [Header("向下延伸速度")]
    public float DownSpeed;

    public enum CatAct
    {
        Idle,
        Run,//動畫用
        Jump,
        Back,//頭部縮回去
        LongLongCat,//伸長狀態
        LongDownCat,//伸長往下狀態
        CatGetHold,//頭部抓到東西了
        CatDie,//貓咪死亡ㄌ
    }
    [Header("貓咪狀態機")]
    public CatAct NowCatAct = CatAct.Idle;


    [Header("阻擋物(眼睛)")]
    public GameObject Eye;

    [Header("抓住東西了")]
    public bool GetHold;
    [Header("可以進行伸長")]
    public bool CanLong;
    [Header("可以進行跳躍")]
    public bool CanJump;
    [Header("Idle後儲存")]
    public bool ReadySave;


    [Header("吸引貓咪的紅光")]
    public GameObject RedBall;

    [Header("貓咪目前持有的道具")]
    public GameObject GetObject;

    [Header("貓咪屁股的撞擊力")]
    public float SetButtPower;
    public float ButtPower;

    [Header("重生或換背景時的黑背景")]
    public GameObject Black;


    // Start is called before the first frame update
    public void Init()
    {
        PlayerPrefs.SetFloat("CatPos_X", transform.position.x);
        PlayerPrefs.SetFloat("CatPos_Y", transform.position.y);
        Debug.Log(PlayerPrefs.GetFloat("CatPos_X") + "," +  PlayerPrefs.GetFloat("CatPos_Y"));
        ButtMoveSpeed = SetButtMoveSpeed;
    }

    private void FixedUpdate()
    {
        //if (NowCatAct == CatAct.Idle || NowCatAct == CatAct.Run || NowCatAct == CatAct.Jump)
        //{
        //    //ButtFallow();//屁屁跟隨頭        
        //    CatMove();//貓咪移動程式
        //}
        //else if (NowCatAct == CatAct.LongLongCat)
        //{
        //    //LongLongCat();
        //}
        //else if (NowCatAct == CatAct.Back)
        //{
        //    //HandFallow();//頭跟隨屁屁
        //}
        //else if (NowCatAct == CatAct.CatGetHold)
        //{
        //   // ButtFallow();//屁屁跟隨頭
        //    CatMove();//貓咪移動程式
        //}
        //else if (NowCatAct == CatAct.LongDownCat)
        //{
        //    //LongDownCat();//屁屁下墜
        //    CatMove();//貓咪移動程式
        //}



    }

    public void Update()
    {
        if (NowCatAct != CatAct.CatDie)
        {
            if (NowCatAct == CatAct.Idle || NowCatAct == CatAct.Run || NowCatAct == CatAct.Jump)
            {
                //ButtFallow();//屁屁跟隨頭        
                CatMove();//貓咪移動程式
            }
            else if (NowCatAct == CatAct.LongLongCat)
            {
                //LongLongCat();
            }
            else if (NowCatAct == CatAct.Back)
            {
                //HandFallow();//頭跟隨屁屁
            }
            else if (NowCatAct == CatAct.CatGetHold)
            {
                // ButtFallow();//屁屁跟隨頭
                CatMove();//貓咪移動程式
            }
            else if (NowCatAct == CatAct.LongDownCat)
            {
                //LongDownCat();//屁屁下墜
                CatMove();//貓咪移動程式
            }

        }

        if (NowCatAct == CatAct.Idle || NowCatAct == CatAct.Run || NowCatAct == CatAct.Jump)
        {
            ButtFallow();//屁屁跟隨頭        
            CatJump();//貓咪跳躍程式
        }
        else if (NowCatAct == CatAct.LongLongCat)
        {
            LongLongCat();
        }
        else if (NowCatAct == CatAct.Back)
        {
            HandFallow();//頭跟隨屁屁
        }
        else if (NowCatAct == CatAct.CatGetHold)
        {
            ButtFallow();//屁屁跟隨頭
            CatJump();//貓咪跳躍程式
        }
        else if (NowCatAct == CatAct.LongDownCat)
        {
            LongDownCat();//屁屁下墜

            if (Input.GetKey(KeyCode.W))
            {
                DownRemove -= DownSpeed * Time.deltaTime;
                Butt.GetComponent<Rigidbody2D>().gravityScale = 0;
                if (DownRemove <= 1f)
                {
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 3000));
                    NowCatAct = CatAct.Idle;
                    DownRemove = 0;
                    Butt.GetComponent<Rigidbody2D>().gravityScale = ButtH;
                    Butt.layer = LayerMask.NameToLayer("Cat");
                }
            }
        }
        else if (NowCatAct == CatAct.CatDie)
        {
            ButtFallow();//屁屁跟隨頭        
        }


        if (NowCatAct == CatAct.Jump)
        {
            Butt.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
        //else if(NowCatAct != CatAct.LongDownCat)
        //{
        //    Butt.GetComponent<Rigidbody2D>().gravityScale = 3;
        //}

        if (RedBall != null)
        {
            Vector2 MousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
            float MouseToRedBall = Vector2.Distance(RedBall.transform.position, MousePos);
            if(MouseToRedBall >= 5)//滑鼠距離花朵多遠就會解除
            {
                RedBall.GetComponent<RedBall>().OnMouseLeft();
            }
        }

        if(GetObject != null)
        {
            GetObject.transform.position = Vector2.Lerp(GetObject.transform.position, Butt.transform.position + new Vector3(0, 3f, 0), 0.03f);
        }

        if(ButtRemove <= ButtRemoveMin && NowCatAct != CatAct.Jump)
        {
            Butt.GetComponent<Rigidbody2D>().gravityScale = ButtH;
            CatBody.SetActive(false);
        }
        else if (NowCatAct != CatAct.Idle)
        {
            CatBody.SetActive(true);
        }

        //if (ReadySave == true)
        //{
        //    if (NowCatAct == CatAct.Idle || NowCatAct == CatAct.Run)
        //    {
        //        PlayerPrefs.SetFloat("CatPos_X", transform.position.x);
        //        PlayerPrefs.SetFloat("CatPos_Y", transform.position.y);
        //        Debug.Log(PlayerPrefs.GetFloat("CatPos_X") + "," + PlayerPrefs.GetFloat("CatPos_Y"));
        //        ReadySave = false;
        //    }
        //}
     
        //if (Input.GetMouseButtonDown(0) && CanLong == true && NowCatAct != CatAct.Back)//進行伸長
        //{
        //    CanLong = false;
        //    Eye.GetComponent<Collider2D>().enabled = false;//關閉阻礙物的Collder(不然會卡到屁股導致屁股亂飛)
        //    NowCatAct = CatAct.LongLongCat;//切換到貓咪伸長的狀態
        //    GetComponent<Rigidbody2D>().gravityScale = 0;//貓咪頭部的重力先歸零
        //    GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);//貓咪頭部的位移力道歸零
        //    Butt.GetComponent<Rigidbody2D>().gravityScale = 5;//貓咪屁股的重力打開(這樣屁屁會掉下去)
        //    Butt.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);//貓咪屁股的位移力道歸零
        //}

        //if (Input.GetMouseButtonUp(0))//結束伸長
        //{
        //    if (GetHold == true)
        //    {
        //        Eye.GetComponent<Collider2D>().enabled = true;//開啟阻礙物的Collder(阻礙物避免屁股衝過頭)
        //        NowCatAct = CatAct.CatGetHold;//切換到貓咪抓到東西的狀態
        //        GetComponent<Rigidbody2D>().gravityScale = 5;//貓咪頭部的重力恢復
        //        Butt.GetComponent<Rigidbody2D>().gravityScale = 0;//貓咪屁股的重力歸零(屁股本身就會自動追蹤身體了)
        //        CanLong = true;
        //        CanJump = true;//抓住後可再進行跳躍
        //        if (transform.position.y - Butt.transform.position.y >= transform.position.x - Butt.transform.position.x)
        //        {
        //            ButtPower = SetButtPower;
        //        }
        //    }
        //    else
        //    {
        //        StartCoroutine(BackIdle());//頭跟隨屁屁

        //        Eye.GetComponent<Collider2D>().enabled = true;//開啟阻礙物的Collder(阻礙物避免屁股衝過頭)
        //        NowCatAct = CatAct.Back;//切換到貓咪Idle的狀態
        //    }
        //}
    }

    // Update is called once per frame
    public void Main()
    {
   

        //if (NowCatAct == CatAct.Jump)
        //{
        //    Butt.GetComponent<Rigidbody2D>().gravityScale = 0;
        //}
        //else
        //{
        //    Butt.GetComponent<Rigidbody2D>().gravityScale = 5;
        //}





        if (Input.GetMouseButtonDown(0) && CanLong == true && NowCatAct != CatAct.Back && NowCatAct != CatAct.LongDownCat)//進行伸長
        {
            CanLong = false;
            Eye.GetComponent<Collider2D>().enabled = false;//關閉阻礙物的Collder(不然會卡到屁股導致屁股亂飛)
            NowCatAct = CatAct.LongLongCat;//切換到貓咪伸長的狀態
            GetComponent<Rigidbody2D>().gravityScale = 0;//貓咪頭部的重力先歸零
            GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);//貓咪頭部的位移力道歸零
            Butt.GetComponent<Rigidbody2D>().gravityScale = ButtH / 5;//貓咪屁股的重力打開(這樣屁屁會掉下去)
            Butt.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);//貓咪屁股的位移力道歸零
        }

        if (Input.GetMouseButtonUp(0))//結束伸長
        {
            if(NowCatAct == CatAct.LongLongCat)
            {

                if (GetHold == true)
                {
                    Eye.GetComponent<Collider2D>().enabled = true;//開啟阻礙物的Collder(阻礙物避免屁股衝過頭)
                    NowCatAct = CatAct.CatGetHold;//切換到貓咪抓到東西的狀態
                                                  //GetComponent<Rigidbody2D>().gravityScale = HandH;//貓咪頭部的重力恢復
                                                  //Butt.GetComponent<Rigidbody2D>().gravityScale = 0;//貓咪屁股的重力歸零(屁股本身就會自動追蹤身體了)
                    StartCoroutine(delta_TouchFollwer());
                    CanLong = true;
                    CanJump = true;//抓住後可再進行跳躍
                    if (RedBall != null)
                    {
                        if (RedBall.GetComponent<RedBall>().IsOne == true)
                        {
                            RedBall.GetComponent<Animator>().SetBool("Break", true);
                            RedBall.GetComponent<RedBall>().OpenAgain();
                        }
                        else if(RedBall.GetComponent<RedBall>().IsOne == false)
                        {
                            RedBall.GetComponent<Animator>().Play("RedBall_Touch");
                        }
                    }
                    if (transform.position.y - Butt.transform.position.y >= transform.position.x - Butt.transform.position.x)
                    {
                        ButtPower = SetButtPower;
                    }
                }
                else
                {
                    Eye.GetComponent<Collider2D>().enabled = true;//開啟阻礙物的Collder(阻礙物避免屁股衝過頭)
                    NowCatAct = CatAct.CatGetHold;//切換到貓咪抓到東西的狀態
                                                  //GetComponent<Rigidbody2D>().gravityScale = HandH;//貓咪頭部的重力恢復
                                                  //Butt.GetComponent<Rigidbody2D>().gravityScale = 0;//貓咪屁股的重力歸零(屁股本身就會自動追蹤身體了)
                    StartCoroutine(delta_TouchFollwer());

                    if (RedBall != null)
                    {
                        if (RedBall.GetComponent<RedBall>().IsOne == true)
                        {
                            RedBall.GetComponent<Animator>().SetBool("Break", true);
                            RedBall.GetComponent<RedBall>().OpenAgain();
                        }
                        else if (RedBall.GetComponent<RedBall>().IsOne == false)
                        {
                            RedBall.GetComponent<Animator>().Play("RedBall_Touch");
                        }
                    }
                    if (transform.position.y - Butt.transform.position.y >= transform.position.x - Butt.transform.position.x)
                    {
                        ButtPower = SetButtPower;
                    }
                    //舊版本
                    //StartCoroutine(BackIdle());//頭跟隨屁屁

                    //Eye.GetComponent<Collider2D>().enabled = true;//開啟阻礙物的Collder(阻礙物避免屁股衝過頭)
                    //NowCatAct = CatAct.Back;//切換到貓咪Idle的狀態
                }
            }
        }
    }

    public void CatMove()
    {
        if (Input.GetKey(KeyCode.D))
        {
            if(NowCatAct == CatAct.Idle || NowCatAct == CatAct.Run)
            {
                NowCatAct = CatAct.Run;
            }
            //transform.position += new Vector3(MoveSpeed, 0, 0) * Time.deltaTime;
            //transform.Translate(new Vector3(MoveSpeed, 0, 0) * Time.deltaTime);
            GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed, GetComponent<Rigidbody2D>().velocity.y);
            //GetComponent<Rigidbody2D>().MovePosition(new Vector3(transform.position.x + MoveSpeed * Time.deltaTime, transform.position.y, 0));
            //GetComponent<Rigidbody2D>().AddForce(new Vector2(MoveSpeed * 1000 * Time.deltaTime, 0));
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (NowCatAct == CatAct.Idle || NowCatAct == CatAct.Run)
            {
                NowCatAct = CatAct.Run;
            }
            //transform.position += new Vector3(-MoveSpeed, 0, 0) * Time.deltaTime;
            //GetComponent<Rigidbody2D>().MovePosition(transform.position + new Vector3(-MoveSpeed, 0, 0));
            GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed, GetComponent<Rigidbody2D>().velocity.y);
            //GetComponent<Rigidbody2D>().MovePosition(new Vector3(transform.position.x - MoveSpeed * Time.deltaTime, transform.position.y, 0));
            //GetComponent<Rigidbody2D>().AddForce(new Vector2(-MoveSpeed * 1000 * Time.deltaTime, 0));
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
        }
    }

    public void CatJump()
    {
        if (Input.GetKeyDown(KeyCode.W) && CanJump == true || Input.GetKeyDown(KeyCode.Space) && CanJump == true)
        {
            NowCatAct = CatAct.Jump;

            //ButtMoveSpeed = 0.6f;
            GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
            GetComponent<Rigidbody2D>().AddForce(new Vector3(0, JumpPower, 0));
            //GetComponent<Rigidbody2D>().velocity = new Vector3(GetComponent<Rigidbody2D>().velocity.x , GetComponent<Rigidbody2D>().velocity.y + MoveSpeed, 0);
            CanJump = false;
        }

    }


    public void LongLongCat()//很長很長的貓
    {
        Vector3 MousePos;

        if (RedBall != null)
        {
            MousePos = RedBall.transform.position;
        }
        else
        {
            MousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        }

        Vector3 direction = MousePos - Butt.transform.position;
        direction.z = 0f;
        direction.Normalize();
        //float targetAngle = Mathf.Atan2(direction.y, direction.x);

        Vector3 RemoveMax = Butt.transform.position + direction * ButtRemoveMax;

        ButtRemove = Vector2.Distance(Butt.transform.position, MousePos);



        if (ButtRemove <= ButtRemoveMax)
        {
            GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(transform.position, MousePos, HandSpeed));
        }
        else
        {
            GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(transform.position, RemoveMax, HandSpeed));
        }
    }

    public void LongDownCat()//很長很長的貓往下墜
    {
        ButtRemove = Vector2.Distance(Butt.transform.position, transform.position);

        Vector3 NowButtPos;
        if(DownRemove > DownRemove_max)
        {
            DownRemove = DownRemove_max;
        }
        NowButtPos = transform.position - new Vector3(0, DownRemove, 0);
        Butt.layer = LayerMask.NameToLayer("CatBody");
        Butt.GetComponent<Rigidbody2D>().gravityScale = 0;
        Butt.GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(Butt.transform.position, NowButtPos, 0.1f));
    }

    public void ButtFallow()//屁屁跟隨頭部
    {
        ButtRemove = Vector2.Distance(Butt.transform.position, transform.position);
        if (ButtRemove >= ButtRemoveMin)
        {
            Butt.GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(Butt.transform.position, transform.position, ButtMoveSpeed));

            //Butt.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector3(MoveSpeed, 0, 0));
            //Butt.transform.position = Vector2.Lerp(Butt.transform.position, transform.position, 0.1f);
        }
        if (ButtRemove <= ButtRemoveMin * 3 && ButtPower > 0 && transform.position.y - Butt.transform.position.y >= transform.position.x - Butt.transform.position.x)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector3(0, 1f, 0) * ButtPower);
            ButtPower = 0;
        }
    }

    public void HandFallow()//頭部縮回去
    {
        ButtRemove = Vector2.Distance(Butt.transform.position, transform.position);
        if (ButtRemove >= ButtRemoveMin)
        {
            GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(transform.position, Butt.transform.position, ButtMoveSpeed));
            //Butt.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector3(MoveSpeed, 0, 0));
            //Butt.transform.position = Vector2.Lerp(Butt.transform.position, transform.position, 0.1f);
        }
    }

    public IEnumerator BackIdle()//頭部縮回去
    {
        GetComponent<Rigidbody2D>().gravityScale = HandH;//貓咪頭部的重力恢復
        Butt.GetComponent<Rigidbody2D>().gravityScale = 0;//貓咪屁股的重力歸零(屁股本身就會自動追蹤身體了)

        yield return new WaitForSeconds(0.2f);
        NowCatAct = CatAct.Idle; 
    }

    public IEnumerator delta_TouchFollwer()//碰到花之後等待幾秒下落
    {
        GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);//貓咪頭部的位移力道歸零
        Butt.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);//貓咪屁股的位移力道歸零
        GetComponent<Rigidbody2D>().gravityScale = 0f;//貓咪頭部的重力恢復
        Butt.GetComponent<Rigidbody2D>().gravityScale = 0;//貓咪屁股的重力歸零(屁股本身就會自動追蹤身體了)

        yield return new WaitForSeconds(0.2f);

        GetComponent<Rigidbody2D>().gravityScale = HandH;//貓咪頭部的重力恢復

        NowCatAct = CatAct.Idle;
    }

    public void GetBall(GameObject Ball)
    {
        RedBall = Ball;
    }
    public void LeaveBall()
    {
        RedBall = null;
    }
    public IEnumerator CatDeath()
    {
        NowCatAct = CatAct.CatDie;

        GetComponent<Collider2D>().enabled = false;
        Black.SetActive(true);
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        GetComponent<Rigidbody2D>().gravityScale = 0;
        yield return new WaitForSeconds(1);
        transform.position = new Vector2(PlayerPrefs.GetFloat("CatPos_X"), PlayerPrefs.GetFloat("CatPos_Y"));
        Butt.transform.position = new Vector2(PlayerPrefs.GetFloat("CatPos_X") - 0.1f, PlayerPrefs.GetFloat("CatPos_Y"));
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        GetComponent<Rigidbody2D>().gravityScale = HandH;
        GetComponent<Collider2D>().enabled = true;
        yield return new WaitForSeconds(1);
        Black.SetActive(false);

        NowCatAct = CatAct.Idle;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Bar")
        {
            if (NowCatAct == CatAct.LongLongCat || NowCatAct == CatAct.LongDownCat)
            {

            }
            else
            {
                NowCatAct = CatAct.Idle;
            }
            CanLong = true;
            CanJump = true;
            ButtMoveSpeed = SetButtMoveSpeed;
        }
        if (collision.gameObject.tag == "Bar")
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Butt.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                Butt.GetComponent<Rigidbody2D>().gravityScale = 0;
            }
            if (Input.GetKey(KeyCode.S))
            {
                DownRemove += DownSpeed * Time.deltaTime;
                NowCatAct = CatAct.LongDownCat;
                Debug.Log("Down");
            }

        }


        if (collision.gameObject.tag == "Props")
        {
            if (GetObject != null)
            {
                GetObject.GetComponent<CanGetObject>().SetStart(collision.transform.position);
            }
            GetObject = collision.gameObject;
            if(GetObject.GetComponent<CanGetObject>() != null)
            {
                GetObject.GetComponent<CanGetObject>().SetGet();
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Bar")
        //{
        //    if (NowCatAct == CatAct.LongLongCat || NowCatAct == CatAct.LongDownCat)
        //    {

        //    }
        //    else
        //    {
        //        NowCatAct = CatAct.Idle;
        //    }
        //    CanLong = true;
        //    CanJump = true;
        //    ButtMoveSpeed = SetButtMoveSpeed;
        //}
        if (collision.gameObject.tag == "Bar")
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Butt.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                Butt.GetComponent<Rigidbody2D>().gravityScale = 0;
            }
            if (Input.GetKey(KeyCode.S))
            {
                DownRemove += DownSpeed * Time.deltaTime;
                NowCatAct = CatAct.LongDownCat;
                Debug.Log("Down");
            }
        }
        if (collision.gameObject.tag == "Pike")
        {
            //碰到陷阱，到儲存點復活
            StartCoroutine(CatDeath());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Bar")
        {
            if (NowCatAct == CatAct.LongLongCat || NowCatAct == CatAct.LongDownCat)
            {

            }
            else
            {
                NowCatAct = CatAct.Jump;
            }            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.gameObject.tag == "CanHold")
        {
            if(collision.GetComponent<SpriteRenderer>().sprite != collision.transform.parent.GetComponent<RedBall>().TouchPictrue)
            {
                collision.GetComponent<SpriteRenderer>().sprite = collision.transform.parent.GetComponent<RedBall>().TouchPictrue;
                collision.GetComponent<CreateAni>().Create(collision.transform.position, 2);
                collision.transform.parent.GetComponent<CreateAni>().Create(collision.transform.position);
            }
            else
            {
                collision.GetComponent<CreateAni>().Create(collision.transform.position, 2);
            }

            if (collision.transform.parent.GetComponent<RedBall>().IsOne == true && NowCatAct != CatAct.LongLongCat)
            {
                collision.transform.parent.GetComponent<Animator>().SetBool("Break", true);
                collision.transform.parent.GetComponent<RedBall>().OpenAgain();
            }

            if (collision.transform.parent.GetComponent<RedBall>().IsOne == false && NowCatAct != CatAct.LongLongCat)
            {
                collision.transform.parent.GetComponent<Animator>().Play("RedBall_Touch");
            }

            CanLong = true;
            CanJump = true;
            GetHold = true;
            PlayerPrefs.SetString(collision.transform.parent.GetComponent<RedBall>().Number, "T");

        }
        if (collision.gameObject.tag == "NPC")
        {
            collision.GetComponent<GrandmaGhost>().TouchGrandma(GetComponent<CatContrl>());
        }
     
    
    }

    

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "CanHold")
    //    {
    //        GetHold = false;
    //    }
    //}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "RedBall")
        {
            if (RedBall == collision.gameObject)
            {
                RedBall = null;
            }
        }
        if (collision.gameObject.tag == "CanHold")
        {
            GetHold = false;
        }
    }
}
