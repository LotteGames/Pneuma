using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatContrl : MonoBehaviour
{
    private Animator CatAni;


    [Header("貓咪轉向哪一邊(打勾為右)")]
    public bool TurnRight;

    public enum CatMorph
    {
        NoMorph,
        //Long,
        Climb,
        Cloud
    }
    [Header("貓咪狀態機")]
    public CatMorph NowCatMorph = CatMorph.NoMorph;

    [Header("貓咪變形時間")]
    public float MorphTime = 0;
    public enum CatAct
    {
        Idle,
        Run,//動畫用
        Jump,
        Back,//頭部縮回去
        LongLongCat,//伸長狀態
        CatClimb,//爬牆狀態
        LongDownCat,//伸長往下狀態
        CatGetHold,//頭部抓到東西了
        CatDie,//貓咪死亡ㄌ
        CatStop//貓咪暫停
    }
    [Header("貓咪狀態機")]
    public CatAct NowCatAct = CatAct.Idle;

    [Header("移動速度")]
    public float MoveSpeed;
    [Header("跳躍力道")]
    public float JumpPower;
    [Header("飛行力道")]
    public float CloudPower;
    public float CloudTime;
    [Header("伸長的極限")]
    public float LongRemoveMax;
    [Header("縮短的極限")]
    public float LongRemoveMin;
    [Header("頭的位置")]
    public Vector2 HandPos;

    [Header("屁股重量")]
    public float CatWeight;

    [Header("不同的摩擦力")]
    public PhysicsMaterial2D CatM_0;
    public PhysicsMaterial2D CatM_1;

    [Header("抓住東西了")]
    public bool GetHold;
    [Header("可以進行操作")]
    public bool CanContrl = true;
    public float CanContrlTrue = 0;
    [Header("可以進行伸長")]
    public bool CanLong;
    public bool CanLongTrue;
    [Header("可以進行跳躍")]
    public bool CanJump;
    public bool CanJumpTrue;
    [Header("是否在爬牆")]
    public bool Climb;
    [Header("Idle後儲存")]
    public bool ReadySave;


    //[Header("伸長貓咪的光暈")]
    //public GameObject LongLight;
    [Header("伸長貓咪的物件")]
    public GameObject LongBody;
    public GameObject nowLongBody;

    [Header("液態伸長貓咪的物件")]
    public GameObject WaterLongBody;
    public GameObject nowWaterLongBody;

    [Header("貓咪目前持有的道具")]
    public GameObject GetObject;
    [Header("快樂的彩蛋帽帽")]
    public GameObject HappyHat;
    [Header("貓咪伸長的最長點")]
    public GameObject LongPos;

    [Header("貓咪彈跳的方向+線")]
    public GameObject WaterJumpPos_1;
    public GameObject WaterJumpPos_2;

    [Header("死亡的黑暗畫面")]
    public GameObject Black;
    [Header("手機UI畫面")]
    public GameObject Phone_UI;
    [Header("手機的跳躍鍵")]
    public GameObject JumpButton;
    [Header("左搖桿")]
    public GameObject Touch_Left;
    public GameObject Handler_Left;
    [Header("右搖桿")]
    public GameObject Touch_Right;
    public GameObject Handler_Right;

    [Header("貓貓音效器")]
    public MusicContrl CatMusic;
    // Start is called before the first frame update
    void Start()
    {
        CanContrlTrue = 0;
        PlayerPrefs.SetFloat("CatPos_X", transform.position.x);
        PlayerPrefs.SetFloat("CatPos_Y", transform.position.y);
        Debug.Log(PlayerPrefs.GetFloat("CatPos_X") + "," + PlayerPrefs.GetFloat("CatPos_Y"));
        CatAni = GetComponent<Animator>();
        HH = true;

#if UNITY_EDITOR || UNITY_STANDALONE
        Phone_UI.SetActive(false);   // 滑鼠偵測
#elif UNITY_ANDROID
		Phone_UI.SetActive(true);  // 觸碰偵測
#endif
    }
    bool HH;
    // Update is called once per frame
    private void Update()
    {
        Debug_Phone();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(HH == true)
            {
                HH = false;
            }
            else
            {
                HH = true;
            }
        }

        if(NowCatMorph == CatMorph.NoMorph)
        {
            MorphUpdate_NoMorph();
            GetComponent<Collider2D>().sharedMaterial = CatM_1;
            LongPos.SetActive(true);
            if(HH == true)
            {
                HappyHat.SetActive(true);
            }
            else
            {
                HappyHat.SetActive(false);
            }
            JumpButton.SetActive(true);
        }
        //else if(NowCatMorph == CatMorph.Long)
        //{
        //    MorphUpdate_Long();
        //    GetComponent<Collider2D>().sharedMaterial = CatM_0;
        //    LongPos.SetActive(true);
        //    HappyHat.SetActive(false);
        //}
        else if (NowCatMorph == CatMorph.Climb)
        {
            MorphUpdate_Climb();
            GetComponent<Collider2D>().sharedMaterial = CatM_1;
            LongPos.SetActive(false);
            HappyHat.SetActive(false);

            GetComponent<Animator>().SetBool("Climb", true);
            GetComponent<SpriteRenderer>().sortingOrder = 0;
            GetComponent<CircleCollider2D>().radius = 0.7f;

            MorphTime -= Time.deltaTime;
            if(MorphTime <= 0)
            {
                CloseWaterJump();

                GetComponent<Animator>().SetBool("Climb", false);
                GetComponent<SpriteRenderer>().sortingOrder = 500;
                GetComponent<CircleCollider2D>().radius = 0.81f;
                NowCatMorph = CatMorph.NoMorph;
            }
            JumpButton.SetActive(false);

        }
        else if (NowCatMorph == CatMorph.Cloud)
        {
            MorphUpdate_Cloud();
            GetComponent<Collider2D>().sharedMaterial = CatM_0;
            LongPos.SetActive(false);
            HappyHat.SetActive(false);

            MorphTime -= Time.deltaTime;
            if (MorphTime <= 0)
            {
                NowCatMorph = CatMorph.NoMorph;

                WaterJumpPos_1.SetActive(false);
                WaterJumpPos_2.SetActive(false);
                GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
                GetComponent<Rigidbody2D>().gravityScale = CatWeight;
                GetComponent<Animator>().SetBool("Cloud", false);
            }
            JumpButton.SetActive(false);

        }

        //撿到的道具跟隨
        if (GetObject != null)
        {
            float ObjectX = transform.position.x;
            if(GetObject.transform.position.x - transform.position.x <= -2 || GetObject.transform.position.x - transform.position.x >= 2)
            {
                ObjectX = transform.position.x;
            }
            else
            {
                ObjectX = GetObject.transform.position.x;
            }
            float ObjectY = transform.position.y + 3;

            GetObject.transform.position = Vector2.Lerp(GetObject.transform.position, new Vector3(ObjectX, ObjectY, 0), 0.03f);
        }
        //if(CanJumpTrue == false)
        //{
        //    StartCoroutine(JumpDebug(0.3f));
        //}
        if (CanLongTrue == false)
        {
            StartCoroutine(LongDebug());
        }
    }

    void MorphUpdate_NoMorph()
    {
        if (NowCatAct == CatAct.LongLongCat || NowCatAct == CatAct.Back || NowCatAct == CatAct.CatStop || Black.active == true)
        {
            RayWall();
        }
        else if (RayWall() == true)
        {
            Debug.Log("Is Climb");

            RayGround();

#if UNITY_EDITOR || UNITY_STANDALONE
            if ((Input.GetMouseButtonDown(0) || Key_0 == true) && CanLong == true && NowCatAct != CatAct.Back && NowCatAct != CatAct.LongDownCat)//進行伸長
            {
                // 滑鼠偵測
                CatMusic.PlayMusic(2);
                CanLong = false;
                NowCatAct = CatAct.LongLongCat;//切換到貓咪伸長的狀態
                GetComponent<Rigidbody2D>().gravityScale = 0;//貓咪屁股的重力先歸零
                GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);//貓咪屁股的位移力道歸零
                GetComponent<Collider2D>().isTrigger = true;

                nowLongBody = Instantiate(LongBody, transform.position, Quaternion.Euler(0, 0, 0));

                StartCoroutine(LongBack(2f));
            }
#elif UNITY_ANDROID

                        if (Key_0 == true && CanLong == true && NowCatAct != CatAct.Back && NowCatAct != CatAct.LongDownCat)//進行伸長
                        {
                            // 觸碰偵測
                            if(Key_0_Drag == false)
                            {
                                CatMusic.PlayMusic(2);
                                CanLong = false;
                                NowCatAct = CatAct.LongLongCat;//切換到貓咪伸長的狀態
                                GetComponent<Rigidbody2D>().gravityScale = 0;//貓咪屁股的重力先歸零
                                GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);//貓咪屁股的位移力道歸零
                                GetComponent<Collider2D>().isTrigger = true;

                                nowLongBody = Instantiate(LongBody, transform.position, Quaternion.Euler(0, 0, 0));

                                StartCoroutine(LongBack(1f));

                                Key_0_Drag = true;//只偵測一次
                            }
                        }
#endif

        }
        else
        {
            if (Input.GetKey(KeyCode.D) || WhatKey_ == "D")
            {
                TurnRight = true;
                CatAni.SetFloat("TurnRight", 1);
                CatAni.SetBool("Move", true);
                if (NowCatAct == CatAct.Idle || NowCatAct == CatAct.Run)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    NowCatAct = CatAct.Run;
                }
                if (CanContrlTrue > 0)
                {
                    CanContrlTrue -= Time.deltaTime;
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(MoveSpeed * 0.2f, 0));
                }
                else
                {
#if UNITY_EDITOR || UNITY_STANDALONE
                    float M = Mathf.Lerp(GetComponent<Rigidbody2D>().velocity.x, MoveSpeed, 0.5f);
#elif UNITY_ANDROID
                    float M = Mathf.Lerp(GetComponent<Rigidbody2D>().velocity.x, MoveSpeed, 0.8f);
#endif
                    GetComponent<Rigidbody2D>().velocity = new Vector2(M, GetComponent<Rigidbody2D>().velocity.y);
                }
            }
            else if (Input.GetKey(KeyCode.A) || WhatKey_ == "A")
            {
                TurnRight = false;
                CatAni.SetFloat("TurnRight", 0);
                CatAni.SetBool("Move", true);
                if (NowCatAct == CatAct.Idle || NowCatAct == CatAct.Run)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    NowCatAct = CatAct.Run;
                }
                if (CanContrlTrue > 0)
                {
                    CanContrlTrue -= Time.deltaTime;
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(-MoveSpeed * 0.2f, 0));
                }
                else
                {
#if UNITY_EDITOR || UNITY_STANDALONE
                    float M = Mathf.Lerp(GetComponent<Rigidbody2D>().velocity.x, -MoveSpeed, 0.5f);
#elif UNITY_ANDROID
                    float M = Mathf.Lerp(GetComponent<Rigidbody2D>().velocity.x, -MoveSpeed, 0.8f);
#endif
                    GetComponent<Rigidbody2D>().velocity = new Vector2(M, GetComponent<Rigidbody2D>().velocity.y);
                }
            }
            else if (NowCatAct != CatAct.LongLongCat)
            {
                if (NowCatAct != CatAct.Jump)
                {
                    if (NowCatAct != CatAct.CatStop)
                    {
                        NowCatAct = CatAct.Idle;
                    }
                }
                if (CanContrlTrue > 0)
                {
                    CanContrlTrue -= Time.deltaTime;
                }
                else
                {
                    float M = Mathf.Lerp(GetComponent<Rigidbody2D>().velocity.x, 0, 0.5f);
                    GetComponent<Rigidbody2D>().velocity = new Vector2(M, GetComponent<Rigidbody2D>().velocity.y);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (CanJump == true)
                {
                    CatMusic.PlayMusic(0);
                    GetComponent<Rigidbody2D>().velocity = new Vector3(GetComponent<Rigidbody2D>().velocity.x, 0, 0);
                    GetComponent<Rigidbody2D>().AddForce(new Vector3(0, JumpPower, 0));
                    NowCatAct = CatAct.Jump;
                    CanJump = false;
                    StartCoroutine(JumpDebug(0.3f));
                }
            }



#if UNITY_EDITOR || UNITY_STANDALONE

            if ((Input.GetMouseButtonDown(0) || Key_0 == true) && CanLong == true && NowCatAct != CatAct.Back && NowCatAct != CatAct.LongDownCat)//進行伸長
            {
                // 滑鼠偵測
                CatMusic.PlayMusic(2);
                CanLong = false;
                NowCatAct = CatAct.LongLongCat;//切換到貓咪伸長的狀態
                GetComponent<Rigidbody2D>().gravityScale = 0;//貓咪屁股的重力先歸零
                GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);//貓咪屁股的位移力道歸零
                GetComponent<Collider2D>().isTrigger = true;

                nowLongBody = Instantiate(LongBody, transform.position, Quaternion.Euler(0, 0, 0));
                //LongLight.SetActive(false);

                StartCoroutine(LongBack(2f));
            }

#elif UNITY_ANDROID
                        if (Key_0 == true && CanLong == true && NowCatAct != CatAct.Back && NowCatAct != CatAct.LongDownCat)//進行伸長
                        {
            		        // 觸碰偵測
                            if(Key_0_Drag == false)
                            {
                               CatMusic.PlayMusic(2);
                               CanLong = false;
                               NowCatAct = CatAct.LongLongCat;//切換到貓咪伸長的狀態
                               GetComponent<Rigidbody2D>().gravityScale = 0;//貓咪屁股的重力先歸零
                               GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);//貓咪屁股的位移力道歸零
                               GetComponent<Collider2D>().isTrigger = true;

                               nowLongBody = Instantiate(LongBody, transform.position, Quaternion.Euler(0, 0, 0));
                               //LongLight.SetActive(false);

                               StartCoroutine(LongBack(1f));

                                Key_0_Drag = true;//只偵測一次
                            }

                        }
#endif

        }

   
#if UNITY_EDITOR || UNITY_STANDALONE
        if ((Input.GetMouseButtonUp(0) || (Key_0 == false && Key_0_Drag == true)) && NowCatAct == CatAct.LongLongCat)
        {
            // 滑鼠偵測
            CatMusic.PlayMusic(3);
            //縮回去
            StartCoroutine(LongDebug());
            CanLong = false;
            GetComponent<Collider2D>().isTrigger = false;
            transform.parent = null;
            NowCatAct = CatAct.Back;
        }

#elif UNITY_ANDROID
                if ((Key_0 == false && Key_0_Drag == true) && NowCatAct == CatAct.LongLongCat)
                {
        		    // 觸碰偵測
                    CatMusic.PlayMusic(3);
                    //縮回去
                    StartCoroutine(LongDebug());
                    CanLong = false;
                    GetComponent<Collider2D>().isTrigger = false;
                    transform.parent = null;
                    NowCatAct = CatAct.Back;
                    Key_0_Drag = false;//只偵測一次
                }
#endif

        LongPictrue();
    }
  
    //void MorphUpdate_Long()
    //{
    //    if (NowCatAct == CatAct.LongLongCat || NowCatAct == CatAct.Back || NowCatAct == CatAct.CatStop || Black.active == true)
    //    {
    //        RayWall();
    //    }
    //    //else if (RayWall() == true)
    //    //{
    //    //    Debug.Log("Is Climb");

    //    //    RayGround();

    //    //    if (Input.GetMouseButtonDown(0) && CanLong == true && NowCatAct != CatAct.Back && NowCatAct != CatAct.LongDownCat)//進行伸長
    //    //    {
    //    //        CanLong = false;
    //    //        NowCatAct = CatAct.LongLongCat;//切換到貓咪伸長的狀態
    //    //        GetComponent<Rigidbody2D>().gravityScale = 0;//貓咪屁股的重力先歸零
    //    //        GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);//貓咪屁股的位移力道歸零
    //    //        GetComponent<Collider2D>().isTrigger = true;

    //    //        nowLongBody = Instantiate(LongBody, transform.position, Quaternion.Euler(0, 0, 0));

    //    //        StartCoroutine(LongBack(1f));
    //    //    }
    //    //}
    //    else
    //    {
    //        if (Input.GetKey(KeyCode.D) || WhatKey_ == "D")
    //        {
    //            TurnRight = true;
    //            CatAni.SetFloat("TurnRight", 1);
    //            CatAni.SetBool("Move", true);
    //            if (NowCatAct == CatAct.Idle || NowCatAct == CatAct.Run)
    //            {
    //                transform.rotation = Quaternion.Euler(0, 0, 0);
    //                NowCatAct = CatAct.Run;
    //            }
    //            GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed, GetComponent<Rigidbody2D>().velocity.y);
    //        }
    //        else if (Input.GetKey(KeyCode.A) || WhatKey_ == "A")
    //        {
    //            TurnRight = false;
    //            CatAni.SetFloat("TurnRight", 0);
    //            CatAni.SetBool("Move", true);
    //            if (NowCatAct == CatAct.Idle || NowCatAct == CatAct.Run)
    //            {
    //                transform.rotation = Quaternion.Euler(0, 180, 0);
    //                NowCatAct = CatAct.Run;
    //            }
    //            GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed, GetComponent<Rigidbody2D>().velocity.y);
    //        }
    //        else if (NowCatAct != CatAct.LongLongCat)
    //        {
    //            if (NowCatAct != CatAct.Jump)
    //            {
    //                if (NowCatAct != CatAct.CatStop)
    //                {
    //                    NowCatAct = CatAct.Idle;
    //                }
    //            }

    //            GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
    //        }

    //        if (Input.GetKeyDown(KeyCode.Space))
    //        {
    //            if (CanJump == true)
    //            {
    //                CatMusic.PlayMusic(0);
    //                GetComponent<Rigidbody2D>().velocity = new Vector3(GetComponent<Rigidbody2D>().velocity.x, 0, 0);
    //                GetComponent<Rigidbody2D>().AddForce(new Vector3(0, JumpPower, 0));
    //                NowCatAct = CatAct.Jump;
    //                CanJump = false;
    //                StartCoroutine(JumpDebug(0.05f));
    //            }
    //        }


    //        if (Input.GetMouseButtonDown(0) && CanLong == true && NowCatAct != CatAct.Back && NowCatAct != CatAct.LongDownCat)//進行伸長
    //        {
    //            CatMusic.PlayMusic(2);
    //            CanLong = false;
    //            NowCatAct = CatAct.LongLongCat;//切換到貓咪伸長的狀態
    //            GetComponent<Rigidbody2D>().gravityScale = 0;//貓咪屁股的重力先歸零
    //            GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);//貓咪屁股的位移力道歸零
    //            GetComponent<Collider2D>().isTrigger = true;

    //            nowLongBody = Instantiate(LongBody, transform.position, Quaternion.Euler(0, 0, 0));


    //            StartCoroutine(LongBack(1f));
    //        }
        
    //    }


    //    //
    //    if (Input.GetMouseButtonUp(0) && NowCatAct == CatAct.LongLongCat)
    //    {
    //        CatMusic.PlayMusic(3);
    //        //縮回去
    //        StartCoroutine(LongDebug());
    //        CanLong = false;
    //        GetComponent<Collider2D>().isTrigger = false;
    //        transform.parent = null;
    //        NowCatAct = CatAct.Back;
    //    }

    //    LongPictrue();

    //}

    public void LongPictrue()
    {

#if UNITY_EDITOR || UNITY_STANDALONE
        // 滑鼠偵測
        Vector3 MousePos;
        MousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

        Vector3 direction = MousePos - transform.position;

        direction.z = 0f;
        direction.Normalize();

        Vector3 RemoveMax = transform.position + direction * LongRemoveMax;

        float ButtRemove = Vector2.Distance(transform.position, MousePos);

        if (ButtRemove <= LongRemoveMax)
        {
            LongPos.transform.position = MousePos;
        }
        else
        {
            LongPos.transform.position = RemoveMax;
        }
#elif UNITY_ANDROID
                // 觸碰偵測
                Vector3 direction = Touch_Right.GetComponent<FixedJoystickHandler>().direction;

                 direction.z = 0f;
                direction.Normalize();

                Vector3 RemoveMax = transform.position + direction * LongRemoveMax;

                LongPos.transform.position = RemoveMax;

#endif
    }

    void MorphUpdate_Climb()
    {
        if (NowCatAct == CatAct.LongLongCat || NowCatAct == CatAct.Back || NowCatAct == CatAct.CatStop || Black.active == true)
        {
            RayWall();
        }
        else if (RayWall() == true)
        {
            Debug.Log("Is Climb");

            Climb_RayGround();
        }
        else
        {
            if (WaterJumpReady == false)
            {
                if (Input.GetKey(KeyCode.D) || WhatKey_ == "D")
                {
                    TurnRight = true;
                    CatAni.SetFloat("TurnRight", 1);
                    CatAni.SetBool("Move", true);
                    if (NowCatAct == CatAct.Idle || NowCatAct == CatAct.Run)
                    {
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                        NowCatAct = CatAct.Run;
                    }
                    GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed, GetComponent<Rigidbody2D>().velocity.y);
                }
                else if (Input.GetKey(KeyCode.A) || WhatKey_ == "A")
                {
                    TurnRight = false;
                    CatAni.SetFloat("TurnRight", 0);
                    CatAni.SetBool("Move", true);
                    if (NowCatAct == CatAct.Idle || NowCatAct == CatAct.Run)
                    {
                        transform.rotation = Quaternion.Euler(0, 180, 0);
                        NowCatAct = CatAct.Run;
                    }
                    GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed, GetComponent<Rigidbody2D>().velocity.y);
                }
                else if (CanJump != false && NowCatAct != CatAct.Jump)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
                }

#if UNITY_EDITOR || UNITY_STANDALONE
                if ((Input.GetMouseButtonDown(0) || Key_0 == true))
                {
                    //CloudTime = 5;
                    // 滑鼠偵測
                    if (CanJump == true)
                    {
                        GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
                        GetComponent<Rigidbody2D>().gravityScale = 0.01f;
                        GetComponent<Animator>().SetBool("WaterJumpReady", true);
                        //WaterJumpPower = 0.1f;
                        WaterJumpPower = 1f;
                    }
                }
#elif UNITY_ANDROID
                if (Key_0 == true)
                {   
                    // 觸碰偵測
                    if(Key_0_Drag == false)
                    {
                        if (CanJump == true)
                        {
                            GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
                            GetComponent<Rigidbody2D>().gravityScale = 0.01f;
                            GetComponent<Animator>().SetBool("WaterJumpReady", true);
                            //WaterJumpPower = 0.1f;
                            WaterJumpPower = 1f;
                        }
                        Key_0_Drag = true;//只偵測一次
                    }
                }
#endif

            }
            else
            {
                //WaterJumpPower += Time.deltaTime * 0.5f;
                //if(WaterJumpPower >= 1f)
                //{
                //    WaterJumpPower = 1f;
                //}
#if UNITY_EDITOR || UNITY_STANDALONE

                if (Input.GetMouseButtonUp(0) || (Key_0 == false && Key_0_Drag == true))
                {
                    //CloudTime = 5;
                    // 滑鼠偵測
                    if (CanJump == true)
                    {
                        Vector3 MousePos;
                        MousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

                        Vector3 direction = MousePos - transform.position;

                        direction.z = 0f;
                        direction.Normalize();
                        Vector3 PowerPath = direction;
                        Debug.Log(PowerPath);

                        CatMusic.PlayMusic(0);
                        NowCatAct = CatAct.Jump;
                        CanJump = false;
                        StartCoroutine(JumpDebug(0.3f));

                        //GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
                        //
                        //GetComponent<Rigidbody2D>().gravityScale = CatWeight;
                        //GetComponent<Rigidbody2D>().AddForce(PowerPath * JumpPower * 2 * WaterJumpPower);
                        //GetComponent<Animator>().SetBool("Jump", true);
                        //GetComponent<Animator>().SetBool("WaterJumpReady", false);
                        //
                        GetComponent<Rigidbody2D>().gravityScale = 0f;
                        //GetComponent<Collider2D>().isTrigger = true;
                        GetComponent<Rigidbody2D>().AddForce(PowerPath * JumpPower * 3 * WaterJumpPower);
                        GetComponent<Animator>().SetBool("Jump", true);
                        GetComponent<Animator>().SetBool("WaterJumpReady", false);
                    }
                }

#elif UNITY_ANDROID
                if (Key_0 == false && Key_0_Drag == true)
                {
		            // 觸碰偵測
                    if (CanJump == true)
                        {
                            Vector3 direction = Touch_Right.GetComponent<FixedJoystickHandler>().direction;

                            direction.z = 0f;
                            direction.Normalize();
                            Vector3 PowerPath = direction;
                            Debug.Log(PowerPath);

                            CatMusic.PlayMusic(0);
                            NowCatAct = CatAct.Jump;
                            CanJump = false;
                            StartCoroutine(JumpDebug(0.3f));

                   
                            GetComponent<Rigidbody2D>().gravityScale = 0f;
                            GetComponent<Rigidbody2D>().AddForce(PowerPath * JumpPower * 3 * WaterJumpPower);
                            GetComponent<Animator>().SetBool("Jump", true);
                            GetComponent<Animator>().SetBool("WaterJumpReady", false);
                        }
                    Key_0_Drag = false;//只偵測一次
                }
#endif


                if (Input.GetMouseButtonUp(1))
                {
                    GetComponent<Rigidbody2D>().gravityScale = CatWeight;
                    WaterJumpReady = false;
                    WaterJumpPos_1.SetActive(false);
                    WaterJumpPos_2.SetActive(false);
                    GetComponent<Animator>().SetBool("WaterJumpReady", false);
                    GetComponent<Animator>().SetBool("Jump", false);
                    if (TurnRight == true)
                    {
                        CatAni.SetFloat("TurnRight", 1);
                    }
                    else
                    {
                        CatAni.SetFloat("TurnRight", 0);
                    }
                    
                }
            }

        }

    }

    //bool WaterJump;

    [Header("噴氣特效")]
    public GameObject CloudAni;
    float WaterJumpPower;
    bool WaterJumpReady;
    void MorphUpdate_Cloud()
    {
        if (Black.active == true || NowCatAct == CatAct.CatStop)
        {

        }
        //else if (RayWall() == true)
        //{
        //    Debug.Log("Is Climb");

        //    RayGround();

        //}
        else
        {
            //GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
            transform.rotation = Quaternion.Euler(0, 0, 0);
            GetComponent<Rigidbody2D>().gravityScale = 0.01f;
            GetComponent<Animator>().SetBool("Cloud", true);
            WaterJumpPos_1.SetActive(true);
            WaterJumpPos_2.SetActive(true);

            //

            CloudPictrue();

            if (GetComponent<Rigidbody2D>().velocity.x > 0.5f)
            {
                CatAni.SetFloat("TurnRight", 1);
                TurnRight = true;
                CatAni.SetBool("Move", true);
            }
            else if (GetComponent<Rigidbody2D>().velocity.x < -0.5f)
            {
                CatAni.SetFloat("TurnRight", 0);
                TurnRight = false;
                CatAni.SetBool("Move", true);
            }
            //else
            //{
            //    CatAni.SetBool("Move", false);
            //}

            //

            if (TurnRight == true)
            {
                transform.rotation = Quaternion.Euler(0, 0, GetComponent<Rigidbody2D>().velocity.y * 3.5f);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, GetComponent<Rigidbody2D>().velocity.y * -3.5f);
            }
#if UNITY_EDITOR || UNITY_STANDALONE

            if (Input.GetMouseButton(0) || Key_0 == true)
            {
                if (TurnRight == true)
                {
                    transform.rotation = Quaternion.Euler(0, 0, GetComponent<Rigidbody2D>().velocity.y * 3.5f);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 0, GetComponent<Rigidbody2D>().velocity.y * -3.5f);
                }
                // 滑鼠偵測
                CloudMove();
            }
#elif UNITY_ANDROID
		    if (Key_0 == true)
            {
                
                if (TurnRight == true)
                {
                    transform.rotation = Quaternion.Euler(0, 0, GetComponent<Rigidbody2D>().velocity.y * 3.5f);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 0, GetComponent<Rigidbody2D>().velocity.y * -3.5f);
                }      
                // 觸碰偵測
                CloudMove_Phone(Touch_Right.GetComponent<FixedJoystickHandler>().direction);
            }
#endif

#if UNITY_EDITOR || UNITY_STANDALONE
            if (Input.GetMouseButtonUp(0) || (Key_0 == false && Key_0_Drag == true))
            {
                //CloudMove();
                //CatAni.SetBool("Move", false);

                // 滑鼠偵測
                CloudMove();
                CatAni.SetBool("Move", false);
            }
#elif UNITY_ANDROID
		    if (Input.GetMouseButtonUp(0) || (Key_0 == false && Key_0_Drag == true))
            {
                //CloudMove();
                //CatAni.SetBool("Move", false);
                // 觸碰偵測
                CloudMove_Phone(Touch_Right.GetComponent<FixedJoystickHandler>().direction);
                CatAni.SetBool("Move", false);
                Key_0_Drag = false;//只偵測一次
            }
#endif

            //if (Input.GetMouseButtonDown(0))
            //{
            //    if (CloudTime > 0)
            //    {
            //        CloudTime--;
            //        CloudMove();
            //    }
            //}

            //if (GetComponent<Rigidbody2D>().velocity.x > 0.5f)
            //{
            //    //CatAni.SetFloat("TurnRight", 1);
            //    //TurnRight = true;
            //    //CatAni.SetBool("Move", true);
            //}
            //else if (GetComponent<Rigidbody2D>().velocity.x < -0.5f)
            //{
            //    //CatAni.SetFloat("TurnRight", 0);
            //    //TurnRight = false;
            //    //CatAni.SetBool("Move", true);
            //}
            //else
            //{
            //    CatAni.SetBool("Move", false);
            //}

            if (TurnRight == true)
            {
                transform.rotation = Quaternion.Euler(0, 0, GetComponent<Rigidbody2D>().velocity.y * 3.5f);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, GetComponent<Rigidbody2D>().velocity.y * -3.5f);
            }
            //

            /*if (GetComponent<Rigidbody2D>().gravityScale != 0.01f)
              {
                  if (Input.GetKey(KeyCode.D))
                  {
                      TurnRight = true;
                      CatAni.SetFloat("TurnRight", 1);
                      CatAni.SetBool("Move", true);
                      if (NowCatAct == CatAct.Idle || NowCatAct == CatAct.Run)
                      {
                          transform.rotation = Quaternion.Euler(0, 0, 0);
                          NowCatAct = CatAct.Run;
                      }
                      GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed, GetComponent<Rigidbody2D>().velocity.y);
                  }
                  else if (Input.GetKey(KeyCode.A))
                  {
                      TurnRight = false;
                      CatAni.SetFloat("TurnRight", 0);
                      CatAni.SetBool("Move", true);
                      if (NowCatAct == CatAct.Idle || NowCatAct == CatAct.Run)
                      {
                          transform.rotation = Quaternion.Euler(0, 180, 0);
                          NowCatAct = CatAct.Run;
                      }
                      GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed, GetComponent<Rigidbody2D>().velocity.y);
                  }
                  else if (NowCatAct != CatAct.LongLongCat)
                  {
                      if (NowCatAct != CatAct.Jump)
                      {
                          if (NowCatAct != CatAct.CatStop)
                          {
                              NowCatAct = CatAct.Idle;
                          }
                      }

                      GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
                  }

                  if (Input.GetKeyDown(KeyCode.Space))
                  {
                      if (CanJump == true)
                      {
                          CatMusic.PlayMusic(0);
                          GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
                          GetComponent<Rigidbody2D>().AddForce(new Vector3(0, JumpPower, 0));
                          NowCatAct = CatAct.Jump;
                          CanJump = false;
                          StartCoroutine(JumpDebug(0.05f));
                      }
                  }
                  if (Input.GetMouseButtonDown(1))
                  {
                      //CloudTime = 5;
                      GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
                      transform.rotation = Quaternion.Euler(0, 0, 0);
                      GetComponent<Rigidbody2D>().gravityScale = 0.01f;
                      GetComponent<Animator>().SetBool("Cloud", true);
                      WaterJumpPos_1.SetActive(true);
                      WaterJumpPos_2.SetActive(true);
                  }
              }
              else
              {

                  CloudPictrue();

                  if (GetComponent<Rigidbody2D>().velocity.x > 0.5f)
                  {
                      CatAni.SetFloat("TurnRight", 1);
                      TurnRight = true;
                      CatAni.SetBool("Move", true);
                  }
                  else if (GetComponent<Rigidbody2D>().velocity.x < -0.5f)
                  {
                      CatAni.SetFloat("TurnRight", 0);
                      TurnRight = false;
                      CatAni.SetBool("Move", true);
                  }
                  else
                  {
                      CatAni.SetBool("Move", false);
                  }

                  if (TurnRight == true)
                  {
                      transform.rotation = Quaternion.Euler(0, 0, GetComponent<Rigidbody2D>().velocity.y * 3.5f);
                  }
                  else
                  {
                      transform.rotation = Quaternion.Euler(0, 0, GetComponent<Rigidbody2D>().velocity.y * -3.5f);
                  }

                  //if (Input.GetMouseButton(0))
                  //{
                  //    if (TurnRight == true)
                  //    {
                  //        transform.rotation = Quaternion.Euler(0, 0, GetComponent<Rigidbody2D>().velocity.y * 3.5f);
                  //    }
                  //    else
                  //    {
                  //        transform.rotation = Quaternion.Euler(0, 0, GetComponent<Rigidbody2D>().velocity.y * -3.5f);
                  //    }
                  //    CloudMove();
                  //}

                  if (Input.GetMouseButtonDown(0))
                  {
                      if (CloudTime > 0)
                      {
                          CloudTime--;
                          CloudMove();
                      }
                  }

                  if (Input.GetMouseButtonUp(1))
                  {
                      WaterJumpPos_1.SetActive(false);
                      WaterJumpPos_2.SetActive(false);
                      GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
                      GetComponent<Rigidbody2D>().gravityScale = CatWeight;
                      GetComponent<Animator>().SetBool("Cloud", false);
                  }
              }*/
        }
    }

    public void CloudMove()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        // 滑鼠偵測
        Vector3 MousePos;
        MousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

        Vector3 direction = MousePos - transform.position;
#elif UNITY_ANDROID
        // 觸碰偵測
        Vector3 direction = Touch_Right.GetComponent<FixedJoystickHandler>().direction;
#endif

        direction.z = 0f;
        direction.Normalize();
        //float targetAngle = Mathf.Atan2(direction.y, direction.x);
        GameObject C = Instantiate(CloudAni, transform.position, Quaternion.Euler(0, 0, 0));
        Destroy(C, 2);

        //GetComponent<Rigidbody2D>().AddForce(direction * CloudPower * 8); // 第一種版本

        GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
        GetComponent<Rigidbody2D>().AddForce(direction * CloudPower * 120); // 第二種版本

        if (GetComponent<Rigidbody2D>().velocity.x > 0.5f)
        {
            CatAni.SetFloat("TurnRight", 1);
            TurnRight = true;
            CatAni.SetBool("Move", true);
        }
        else if (GetComponent<Rigidbody2D>().velocity.x < -0.5f)
        {
            CatAni.SetFloat("TurnRight", 0);
            TurnRight = false;
            CatAni.SetBool("Move", true);
        }


        if (TurnRight == true)
        {
            transform.rotation = Quaternion.Euler(0, 0, GetComponent<Rigidbody2D>().velocity.y * 3.5f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, GetComponent<Rigidbody2D>().velocity.y * -3.5f);
        }

        //CatMusic.PlayMusic(4);
    }

    public void CloudMove_Phone(Vector2 ScrollPos)
    {
       
        //
        Vector3 direction = ScrollPos;
        direction.z = 0f;
        direction.Normalize();
        //float targetAngle = Mathf.Atan2(direction.y, direction.x);
        GameObject C = Instantiate(CloudAni, transform.position, Quaternion.Euler(0, 0, 0));
        Destroy(C, 2);

        //GetComponent<Rigidbody2D>().AddForce(direction * CloudPower * 8); // 第一種版本

        GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
        GetComponent<Rigidbody2D>().AddForce(direction * CloudPower * 120); // 第二種版本

        if (GetComponent<Rigidbody2D>().velocity.x > 0.5f)
        {
            CatAni.SetFloat("TurnRight", 1);
            TurnRight = true;
            CatAni.SetBool("Move", true);
        }
        else if (GetComponent<Rigidbody2D>().velocity.x < -0.5f)
        {
            CatAni.SetFloat("TurnRight", 0);
            TurnRight = false;
            CatAni.SetBool("Move", true);
        }


        if (TurnRight == true)
        {
            transform.rotation = Quaternion.Euler(0, 0, GetComponent<Rigidbody2D>().velocity.y * 3.5f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, GetComponent<Rigidbody2D>().velocity.y * -3.5f);
        }

        //CatMusic.PlayMusic(4);
    }

    public void CloudPictrue()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        // 滑鼠偵測
        Vector3 MousePos;
        MousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

        Vector3 direction = MousePos - transform.position;
#elif UNITY_ANDROID
        // 觸碰偵測
        Vector3 direction = Touch_Right.GetComponent<FixedJoystickHandler>().direction;
#endif

        direction.z = 0f;
        direction.Normalize();
        //float targetAngle = Mathf.Atan2(direction.y, direction.x);

        Vector3 RemoveMax = transform.position + direction * 2.3f;

        //float ButtRemove = Vector2.Distance(transform.position, MousePos);

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        WaterJumpPos_1.transform.rotation = Quaternion.Euler(0, 0, targetAngle - 90);
        WaterJumpPos_2.transform.rotation = Quaternion.Euler(0, 0, targetAngle - 90);

        WaterJumpPos_1.transform.position = RemoveMax;
        WaterJumpPos_2.transform.position = RemoveMax;
        //if (ButtRemove <= LongRemoveMax)
        //{
        //    WaterJumpPos_1.transform.position = MousePos;
        //    WaterJumpPos_2.transform.position = MousePos;

        //}
        //else
        //{
        //    WaterJumpPos_1.transform.position = RemoveMax;
        //    WaterJumpPos_2.transform.position = RemoveMax;       
        //}

        float OverPos = (CloudTime + 1) / 4;
        Debug.Log(OverPos);

        WaterJumpPos_1.GetComponent<LineRenderer>().SetPosition(0, transform.position - direction * 0.2f);
        WaterJumpPos_1.GetComponent<LineRenderer>().SetPosition(1, WaterJumpPos_1.transform.position);
        WaterJumpPos_2.GetComponent<LineRenderer>().SetPosition(0, transform.position - direction * 0.2f);
        Vector2 P_2 = Vector2.Lerp(transform.position, WaterJumpPos_2.transform.position, OverPos);
        WaterJumpPos_2.GetComponent<LineRenderer>().SetPosition(1, P_2);

    }

    public void JumpBox()
    {
        CanContrlTrue = 0.5f;
        //StartCoroutine(ContrlDebug(0.5f));
    }

    //public IEnumerator ContrlDebug(float DelayTime)
    //{
    //    CanContrlTrue = false;
    //    yield return new WaitForSeconds(DelayTime);
    //    CanContrlTrue = true;
    //}
    //public IEnumerator JumpDebug(float DelayTime)
    //{
    //    CanJumpTrue = false;
    //    yield return new WaitForSeconds(DelayTime);
    //    CanJumpTrue = true;
    //}
    public IEnumerator JumpDebug(float DelayTime)
    {
        CanJumpTrue = false;
        yield return new WaitForSeconds(DelayTime);
        CanJumpTrue = true;
    }
    public IEnumerator LongDebug()
    {
        CanLongTrue = false;
        yield return new WaitForSeconds(0.05f);
        CanLongTrue = true;
    }


    private void FixedUpdate()
    {
        CatActUpdate();
    }

    //public void ButtFallow()//屁屁跟隨頭部
    //{
    //    ButtRemove = Vector2.Distance(Butt.transform.position, transform.position);
    //    if (ButtRemove >= ButtRemoveMin)
    //    {
    //        Butt.GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(Butt.transform.position, transform.position, ButtMoveSpeed));

    //        //Butt.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector3(MoveSpeed, 0, 0));
    //        //Butt.transform.position = Vector2.Lerp(Butt.transform.position, transform.position, 0.1f);
    //    }
    //    if (ButtRemove <= ButtRemoveMin * 3 && ButtPower > 0 && transform.position.y - Butt.transform.position.y >= transform.position.x - Butt.transform.position.x)
    //    {
    //        GetComponent<Rigidbody2D>().AddForce(new Vector3(0, 1f, 0) * ButtPower);
    //        ButtPower = 0;
    //    }
    //}


    public IEnumerator LongBack(float DelayTime)
    {
        yield return new WaitForSeconds(DelayTime);
        if(NowCatAct == CatAct.LongLongCat)
        {
            CatMusic.PlayMusic(3);
            //縮回去
            StartCoroutine(LongDebug());
            CanLong = false;
            GetComponent<Collider2D>().isTrigger = false;
            transform.parent = null;
            NowCatAct = CatAct.Back;
        }
    }

    public void CatActUpdate()
    {
        if (NowCatAct == CatAct.Run)
        {
            CatAni.SetBool("Move", true);
        }
        else if (NowCatAct == CatAct.Idle || NowCatAct == CatAct.CatStop)
        {
            CatAni.SetBool("Move", false);
        }
        else if (NowCatAct == CatAct.Jump)
        {
            CatAni.SetBool("Jump", true);
            if (TurnRight == true)
            {
                if (NowCatMorph != CatMorph.Climb)
                {
                    transform.rotation = Quaternion.Euler(0, 0, GetComponent<Rigidbody2D>().velocity.y * 2.5f);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
            else
            {
                if (NowCatMorph != CatMorph.Climb)
                {
                    transform.rotation = Quaternion.Euler(0, 180, GetComponent<Rigidbody2D>().velocity.y * 2.5f);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
            }
        }
        else if (NowCatAct == CatAct.LongLongCat)
        {
            if(NowCatMorph == CatMorph.Climb)
            {

            }
            else
            {
                CatAni.SetBool("Long", true);
                LongLongCat();
            }
        }
        else if (NowCatAct == CatAct.Back)
        {
            CatAni.SetBool("Long", true);
            FallowHand();
        }
        else if (NowCatAct == CatAct.CatClimb)
        {
            FallowHand();
        }
        else if (NowCatAct == CatAct.CatDie)
        {

        }
    }

    public bool Ray_Left;
    public bool Ray_Right;
    public bool Ray_Up;
    public IEnumerator RayL_Check()
    {
        yield return new WaitForSeconds(0.2f);
        Ray_Left = true;
    }
    public IEnumerator RayR_Check()
    {
        yield return new WaitForSeconds(0.2f);
        Ray_Right = true;
    }
    public IEnumerator RayU_Check()
    {
        yield return new WaitForSeconds(0.2f);
        Ray_Up = true;
    }

    public bool RayWall()
    {

        Physics2D.queriesStartInColliders = false;

        RaycastHit2D hit_L_1 = Physics2D.Raycast(transform.position + new Vector3(0, -0.25f, 0), new Vector3(-1f, 0, 0), 1);
        RaycastHit2D hit_L_2 = Physics2D.Raycast(transform.position + new Vector3(0, 0.25f, 0), new Vector3(-1f, 0, 0), 1);
        RaycastHit2D hit_D_1 = Physics2D.Raycast(transform.position + new Vector3(-0.2f, 0, 0), new Vector3(0, -1f, 0), 1);
        RaycastHit2D hit_D_2 = Physics2D.Raycast(transform.position + new Vector3(0.2f, 0, 0), new Vector3(0, -1f, 0), 1);
        RaycastHit2D hit_R_1 = Physics2D.Raycast(transform.position + new Vector3(0, -0.25f, 0), new Vector3(1f, 0, 0), 1);
        RaycastHit2D hit_R_2 = Physics2D.Raycast(transform.position + new Vector3(0, 0.25f, 0), new Vector3(1f, 0, 0), 1);
        RaycastHit2D hit_U_1 = Physics2D.Raycast(transform.position + new Vector3(-0.2f, 0, 0), new Vector3(0, 1f, 0), 1);
        RaycastHit2D hit_U_2 = Physics2D.Raycast(transform.position + new Vector3(0.2f, 0, 0), new Vector3(0, 1f, 0), 1);

        Debug.DrawLine(transform.position + new Vector3(0, -0.25f, 0), transform.position + new Vector3(-1f, -0.25f, 0), Color.red);
        Debug.DrawLine(transform.position + new Vector3(0, 0.25f, 0), transform.position + new Vector3(-1f, 0.25f, 0), Color.red);
        Debug.DrawLine(transform.position + new Vector3(-0.2f, 0, 0), transform.position + new Vector3(-0.2f, -1f, 0), Color.blue);
        Debug.DrawLine(transform.position + new Vector3(0.2f, 0, 0), transform.position + new Vector3(0.2f, -1f, 0), Color.blue);
        Debug.DrawLine(transform.position + new Vector3(0, -0.25f, 0), transform.position + new Vector3(1f, -0.25f, 0), Color.green);
        Debug.DrawLine(transform.position + new Vector3(0, 0.25f, 0), transform.position + new Vector3(1f, 0.25f, 0), Color.green);
        Debug.DrawLine(transform.position + new Vector3(-0.2f, 0, 0), transform.position + new Vector3(-0.2f, 1f, 0), Color.yellow);
        Debug.DrawLine(transform.position + new Vector3(0.2f, 0, 0), transform.position + new Vector3(0.2f, 1f, 0), Color.yellow);



        float RotX = 0;
        float RotY = 0;

        bool IsClimb = false;

        if (NowCatAct == CatAct.LongLongCat)
        {

            if (hit_D_1.collider != null)
            {

                if (hit_D_1.collider.gameObject.tag == "Ground" || hit_D_1.collider.gameObject.tag == "Wall" || hit_D_1.collider.gameObject.tag == "DoorGround")
                {
                    if (hit_D_1.collider.GetComponent<MoveGround>() != null)
                    {
                        hit_D_1.collider.GetComponent<MoveGround>().MoveObject(gameObject);
                    }
                }
        
            }
            else if (hit_D_2.collider != null)
            {
                if (hit_D_2.collider.gameObject.tag == "Ground" || hit_D_2.collider.gameObject.tag == "Wall" || hit_D_2.collider.gameObject.tag == "DoorGround")
                {
                    if (hit_D_2.collider.GetComponent<MoveGround>() != null)
                    {
                        hit_D_2.collider.GetComponent<MoveGround>().MoveObject(gameObject);
                    }
                }
            }


            if (hit_U_1.collider != null)
            {
                if (hit_U_1.collider.gameObject.tag == "Ground" || hit_U_1.collider.gameObject.tag == "Wall")
                {
                    if (hit_U_1.collider.GetComponent<MoveGround>() != null)
                    {
                        hit_U_1.collider.GetComponent<MoveGround>().MoveObject(gameObject);
                    }
                }
            }
            else if (hit_U_2.collider != null)
            {
                if (hit_U_2.collider.gameObject.tag == "Ground" || hit_U_2.collider.gameObject.tag == "Wall")
                {
                    if (hit_U_2.collider.GetComponent<MoveGround>() != null)
                    {
                        hit_U_2.collider.GetComponent<MoveGround>().MoveObject(gameObject);
                    }
                }
            }

            if (hit_L_1.collider != null)
            {
                if (hit_L_1.collider.gameObject.tag == "Ground" || hit_L_1.collider.gameObject.tag == "Wall")
                {
                    if (hit_L_1.collider.GetComponent<MoveGround>() != null)
                    {
                        hit_L_1.collider.GetComponent<MoveGround>().MoveObject(gameObject);
                    }
                }
            }
            else if (hit_L_2.collider != null)
            {
                if (hit_L_2.collider.gameObject.tag == "Ground" || hit_L_2.collider.gameObject.tag == "Wall")
                {
                    if (hit_L_2.collider.GetComponent<MoveGround>() != null)
                    {
                        hit_L_2.collider.GetComponent<MoveGround>().MoveObject(gameObject);
                    }
                }
            }

            if (hit_R_1.collider != null)
            {
                if (hit_R_1.collider.gameObject.tag == "Ground" || hit_R_1.collider.gameObject.tag == "Wall")
                {
                    if (hit_R_1.collider.GetComponent<MoveGround>() != null)
                    {
                        hit_R_1.collider.GetComponent<MoveGround>().MoveObject(gameObject);
                    }
                }
            }
            else if (hit_R_2.collider != null)
            {
                if (hit_R_2.collider.gameObject.tag == "Ground" || hit_R_2.collider.gameObject.tag == "Wall")
                {
                    if (hit_R_2.collider.GetComponent<MoveGround>() != null)
                    {
                        hit_R_2.collider.GetComponent<MoveGround>().MoveObject(gameObject);
                    }
                }

            }
        }


        if (NowCatAct == CatAct.Jump || NowCatAct == CatAct.Run || NowCatAct == CatAct.CatClimb || NowCatAct == CatAct.Idle)
        {

            if (hit_D_1.collider != null)
            {

                if (hit_D_1.collider.gameObject.tag == "Ground" || hit_D_1.collider.gameObject.tag == "Wall" || hit_D_1.collider.gameObject.tag == "DoorGround")
                {
                    RotY = -1;


                    if (hit_D_1.collider.GetComponent<MoveGround>() != null)
                    {
                        hit_D_1.collider.GetComponent<MoveGround>().MoveObject(gameObject);
                    }
                    //if (Input.GetKey(KeyCode.S))
                    //{
                    //    RotY = -1;
                    //}
                    //else
                    //{
                    //    RotY = 0;
                    //}
                }
                else
                {
                    RotY = 0;
                }
            }
            else if (hit_D_2.collider != null)
            {
                Debug.Log(hit_D_2.collider.gameObject.tag);
                if (hit_D_2.collider.gameObject.tag == "Ground" || hit_D_2.collider.gameObject.tag == "Wall" || hit_D_2.collider.gameObject.tag == "DoorGround")
                {
                    RotY = -1;

                    if (hit_D_2.collider.GetComponent<MoveGround>() != null)
                    {
                        hit_D_2.collider.GetComponent<MoveGround>().MoveObject(gameObject);
                    }
                    //if (Input.GetKey(KeyCode.S))
                    //{
                    //    RotY = -1;
                    //}
                    //else
                    //{
                    //    RotY = 0;
                    //}
                }
                //else
                //{
                //    RotY = 0;
                //}
            }


            if (hit_U_1.collider != null)
            {
                if (hit_U_1.collider.gameObject.tag == "Ground" || hit_U_1.collider.gameObject.tag == "Wall")
                {

                    if (Input.GetKey(KeyCode.W) || WhatKey_ == "W")
                    {
                        RotY = 1;
                        GetComponent<Rigidbody2D>().gravityScale = 0;
                        IsClimb = true;

                        if (hit_U_1.collider.GetComponent<MoveGround>() != null)
                        {
                            hit_U_1.collider.GetComponent<MoveGround>().MoveObject(gameObject);
                        }

                        if (CanLongTrue == true)
                        {
                            //LongLight.SetActive(true);
                            CanLong = true;
                        }
                        if (CanJumpTrue == true)
                        {
                            CanJump = true;
                            CatAni.SetBool("Jump", false);
                        }
                    }
                    else
                    {
                        if (NowCatMorph != CatMorph.Climb)
                        {
                            RotY = 0;
                            if (WaterFly == false)
                            {
                                GetComponent<Rigidbody2D>().gravityScale = CatWeight;
                            }
                        }
                        else
                        {
                            RotY = 1;
                        }
                    }
                }
                //else
                //{
                //    RotY = 0;
                //}
            }
            else if (hit_U_2.collider != null)
            {
                if (hit_U_2.collider.gameObject.tag == "Ground" || hit_U_2.collider.gameObject.tag == "Wall")
                {
                    if (Input.GetKey(KeyCode.W) || WhatKey_ == "W")
                    {
                        RotY = 1;
                        GetComponent<Rigidbody2D>().gravityScale = 0;
                        IsClimb = true;

                        if (hit_U_2.collider.GetComponent<MoveGround>() != null)
                        {
                            hit_U_2.collider.GetComponent<MoveGround>().MoveObject(gameObject);
                        }


                        if (CanLongTrue == true)
                        {
                            //LongLight.SetActive(true);
                            CanLong = true;
                        }
                        if (CanJumpTrue == true)
                        {
                            CanJump = true;
                            CatAni.SetBool("Jump", false);
                        }
                    }
                    else
                    {
                        if (NowCatMorph != CatMorph.Climb)
                        {
                            RotY = 0;
                            if (WaterFly == false)
                            {
                                GetComponent<Rigidbody2D>().gravityScale = CatWeight;
                            }
                        }
                        else
                        {
                            RotY = 1;
                        }
                    }
                }
                //else
                //{
                //    RotY = 0;
                //}
            }

            if (hit_U_1.collider == null && hit_U_2.collider == null && hit_D_1.collider == null && hit_D_2.collider == null)
            {
                RotY = 0;

                if (WaterFly == false)
                {
                    GetComponent<Rigidbody2D>().gravityScale = CatWeight;
                }
            }



            if (hit_L_1.collider != null)
            {
                if (hit_L_1.collider.gameObject.tag == "Ground" || hit_L_1.collider.gameObject.tag == "Wall")
                {
                    if (Input.GetKey(KeyCode.A) || WhatKey_ == "A")
                    {
                        RotX = -1;
                        IsClimb = true;

                        if (hit_L_1.collider.GetComponent<MoveGround>() != null)
                        {
                            hit_L_1.collider.GetComponent<MoveGround>().MoveObject(gameObject);
                        }

                        if (CanLongTrue == true)
                        {
                            //LongLight.SetActive(true);
                            CanLong = true;
                        }
                        if (CanJumpTrue == true)
                        {
                            CanJump = true;
                            CatAni.SetBool("Jump", false);
                        }
                    }
                    else
                    {
                        if (NowCatMorph != CatMorph.Climb)
                        {
                            RotX = 0;
                        }
                        else
                        {
                            RotX = -1;
                        }
                    }

                    //if (Input.GetKey(KeyCode.A))
                    //{
                    //    RotX = -1;
                    //    IsClimb = true;
                    //}
                    //else
                    //{
                    //    RotX = 0;
                    //}
                }
                //else
                //{
                //    RotX = 0;
                //}
            }
            else if (hit_L_2.collider != null)
            {
                if (hit_L_2.collider.gameObject.tag == "Ground" || hit_L_2.collider.gameObject.tag == "Wall")
                {
                    if (Input.GetKey(KeyCode.A) || WhatKey_ == "A")
                    {
                        RotX = -1;
                        IsClimb = true;

                        if (hit_L_2.collider.GetComponent<MoveGround>() != null)
                        {
                            hit_L_2.collider.GetComponent<MoveGround>().MoveObject(gameObject);
                        }

                        if (CanLongTrue == true)
                        {
                            //LongLight.SetActive(true);
                            CanLong = true;
                        }
                        if (CanJumpTrue == true)
                        {
                            CanJump = true;
                            CatAni.SetBool("Jump", false);
                        }
                    }
                    else
                    {
                        if (NowCatMorph != CatMorph.Climb)
                        {
                            RotX = 0;
                        }
                        else
                        {
                            RotX = -1;
                        }
                    }

                    //if (Input.GetKey(KeyCode.A))
                    //{
                    //    RotX = -1;
                    //    IsClimb = true;
                    //}
                    //else
                    //{
                    //    RotX = 0;
                    //}
                }
                //else
                //{
                //    RotX = 0;
                //}
            }

            if (hit_R_1.collider != null)
            {
                if (hit_R_1.collider.gameObject.tag == "Ground" || hit_R_1.collider.gameObject.tag == "Wall")
                {
                    if (Input.GetKey(KeyCode.D) || WhatKey_ == "D")
                    {
                        RotX = 1;
                        IsClimb = true;

                        if (hit_R_1.collider.GetComponent<MoveGround>() != null)
                        {
                            hit_R_1.collider.GetComponent<MoveGround>().MoveObject(gameObject);
                        }

                        if (CanLongTrue == true)
                        {
                            //LongLight.SetActive(true);
                            CanLong = true;
                        }
                        if (CanJumpTrue == true)
                        {
                            CanJump = true;
                            CatAni.SetBool("Jump", false);
                        }
                    }
                    else
                    {
                        if (NowCatMorph != CatMorph.Climb)
                        {
                            RotX = 0;
                        }
                        else
                        {
                            RotX = 1;
                        }
                    }
                }
            }
            else if (hit_R_2.collider != null)
            {
                if (hit_R_2.collider.gameObject.tag == "Ground" || hit_R_2.collider.gameObject.tag == "Wall")
                {
                    if (Input.GetKey(KeyCode.D) || WhatKey_ == "D")
                    {
                        RotX = 1;
                        IsClimb = true;

                        if (hit_R_2.collider.GetComponent<MoveGround>() != null)
                        {
                            hit_R_2.collider.GetComponent<MoveGround>().MoveObject(gameObject);
                        }


                        if (CanLongTrue == true)
                        {
                            //LongLight.SetActive(true);
                            CanLong = true;
                        }
                        if (CanJumpTrue == true)
                        {
                            CanJump = true;
                            CatAni.SetBool("Jump", false);
                        }
                    }
                    else
                    {
                        if (NowCatMorph != CatMorph.Climb)
                        {
                            RotX = 0;
                        }
                        else
                        {
                            RotX = 1;
                        }
                    }
                }
                //else
                //{
                //    RotX = 0;
                //}
                //posRight = hit_R.point;//打到的位置
            }
            if (hit_R_1.collider == null && hit_R_2.collider == null && hit_L_1.collider == null && hit_L_2.collider == null)
            {
                RotX = 0;
                Debug.Log(RotX);
            }
        }

        Rot = new Vector2(RotX, RotY);

        if (Rot == new Vector2(0, 0) || Rot == new Vector2(0, -1))
        {
            if (NowCatMorph != CatMorph.Climb)
            {
                IsClimb = false;
            }
            else
            {
                IsClimb = true;
            }
        }
        else
        {
            IsClimb = true;
        }
        //Debug.Log(Rot);
       // Debug.Log(IsClimb);
        Climb = IsClimb;
   
        return IsClimb;
  


    }

    //private bool CanClimb = true;
    //public IEnumerator CatJumpNoClimb()
    //{
    //    CanClimb = false;
    //    yield return new WaitForSeconds(0.25f);
    //    CanClimb = true;
    //}

    public Quaternion RotForWall = Quaternion.Euler(0, 0, 0);
    public Vector2 Rot;

    public void Climb_RayGround()
    {


        RaycastHit2D hit_LeftUp = Physics2D.Raycast(transform.position, new Vector3(-1f, 1, 0), 0.9f);
        RaycastHit2D hit_LeftDown = Physics2D.Raycast(transform.position, new Vector3(-1, -1f, 0), 0.9f);
        RaycastHit2D hit_RightUp = Physics2D.Raycast(transform.position, new Vector3(1f, 1, 0), 0.9f);
        RaycastHit2D hit_RightDown = Physics2D.Raycast(transform.position, new Vector3(1, -1f, 0), 0.9f);

        Debug.DrawLine(transform.position, transform.position + new Vector3(-1f, 1, 0), Color.black);
        Debug.DrawLine(transform.position, transform.position + new Vector3(-1, -1f, 0), Color.black);
        Debug.DrawLine(transform.position, transform.position + new Vector3(1f, 1, 0), Color.black);
        Debug.DrawLine(transform.position, transform.position + new Vector3(1, -1f, 0), Color.black);


        if (Rot == new Vector2(0, 0))
        {
            if (CanJumpTrue == true)
            {
           
                if (hit_LeftUp.collider != null && hit_LeftUp.collider.GetComponent<MoveGround>() != null)
                {
                    if (hit_LeftUp.collider.GetComponent<MoveGround>().IsPike == false)
                    {
                        GetComponent<Rigidbody2D>().velocity = new Vector2(-1f, 1f) * MoveSpeed * 0.6f;
                    }
                }
                if (hit_LeftDown.collider != null && hit_LeftDown.collider.GetComponent<MoveGround>() != null)
                {
                    if (hit_LeftDown.collider.GetComponent<MoveGround>().IsPike == false)
                    {
                        GetComponent<Rigidbody2D>().velocity = new Vector2(-1f, -1f) * MoveSpeed * 0.3f;
                    }
                }
                if (hit_RightUp.collider != null && hit_RightUp.collider.GetComponent<MoveGround>() != null)
                {
                    if (hit_RightUp.collider.GetComponent<MoveGround>().IsPike == false)
                    {
                        GetComponent<Rigidbody2D>().velocity = new Vector2(1f, 1f) * MoveSpeed * 0.6f;
                    }
                }
                if (hit_RightDown.collider != null && hit_RightDown.collider.GetComponent<MoveGround>() != null)
                {
                    if (hit_RightDown.collider.GetComponent<MoveGround>().IsPike == false)
                    {
                        GetComponent<Rigidbody2D>().velocity = new Vector2(1f, -1f) * MoveSpeed * 0.5f;
                    }
                }


                if (hit_LeftUp.collider != null)
                {
                    if (hit_LeftUp.collider.gameObject.tag == "Ground" || hit_LeftUp.collider.gameObject.tag == "Wall")
                    {
                        GetComponent<Rigidbody2D>().gravityScale = 0;
                        CatAni.SetBool("Jump", false);

                        CatAni.SetFloat("TurnRight", 0.666f);
                        transform.rotation = Quaternion.Euler(0, 0, -90);

                        if (Input.GetKey(KeyCode.W) || WhatKey_ == "W")
                        {
                            TurnRight = false;
                            CatAni.SetBool("Move", true);
                            GetComponent<Rigidbody2D>().velocity = new Vector2(0, MoveSpeed * 0.7f);
                        }
                        else if (Input.GetKey(KeyCode.A) || WhatKey_ == "A")
                        {
                            TurnRight = true;
                            CatAni.SetBool("Move", true);
                            GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed * 0.7f, 0);
                        }
                        else
                        {
                            if (hit_LeftUp.collider != null && hit_LeftUp.collider.GetComponent<MoveGround>() != null)
                            {
                                if (hit_LeftUp.collider.GetComponent<MoveGround>().IsPike == false)
                                {
                                    GetComponent<Rigidbody2D>().velocity = new Vector2(-1f, 1f) * MoveSpeed;
                                }
                            }
                            else
                            {
                                CatAni.SetBool("Move", false);
                                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                                WaterJump(Rot);
                            }
                        
                        }

                        if (CanJumpTrue == true)
                        {
                            CanJump = true;
                        }

                    }
                }
                if (hit_LeftDown.collider != null)
                {
                    if (hit_LeftDown.collider.gameObject.tag == "Ground" || hit_LeftDown.collider.gameObject.tag == "Wall")
                    {
                        GetComponent<Rigidbody2D>().gravityScale = 0;
                        CatAni.SetBool("Jump", false);

                        CatAni.SetFloat("TurnRight", 0.666f);
                        transform.rotation = Quaternion.Euler(0, 0, 0);

                        if (Input.GetKey(KeyCode.S) || WhatKey_ == "S")
                        {
                            TurnRight = true;
                            CatAni.SetBool("Move", true);
                            GetComponent<Rigidbody2D>().velocity = new Vector2(0, -MoveSpeed * 0.7f);
                        }
                        else if (Input.GetKey(KeyCode.A) || WhatKey_ == "A")
                        {
                            TurnRight = false;
                            CatAni.SetBool("Move", true);
                            GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed * 0.7f, 0);
                        }
                        else
                        {
                            if (hit_LeftDown.collider != null && hit_LeftDown.collider.GetComponent<MoveGround>() != null)
                            {
                                if (hit_LeftDown.collider.GetComponent<MoveGround>().IsPike == false)
                                {
                                    GetComponent<Rigidbody2D>().velocity = new Vector2(-1f, -1f) * MoveSpeed;
                                }
                            }
                            else
                            {
                                CatAni.SetBool("Move", false);
                                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                                WaterJump(Rot);
                            }
                   
                        }

                        if (CanJumpTrue == true)
                        {
                            CanJump = true;
                        }

                    }
                }
                if (hit_RightUp.collider != null)
                {
                    if (hit_RightUp.collider.gameObject.tag == "Ground" || hit_RightUp.collider.gameObject.tag == "Wall")
                    {
                        GetComponent<Rigidbody2D>().gravityScale = 0;
                        CatAni.SetBool("Jump", false);

                        CatAni.SetFloat("TurnRight", 0.666f);
                        transform.rotation = Quaternion.Euler(0, 0, 180);

                        if (Input.GetKey(KeyCode.W) || WhatKey_ == "W")
                        {
                            TurnRight = true;
                            CatAni.SetBool("Move", true);
                            GetComponent<Rigidbody2D>().velocity = new Vector2(0, MoveSpeed * 0.7f);
                        }
                        else if (Input.GetKey(KeyCode.D) || WhatKey_ == "D")
                        {
                            TurnRight = false;
                            CatAni.SetBool("Move", true);
                            GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed * 0.7f, 0);
                        }
                        else
                        {
                            if (hit_RightUp.collider != null && hit_RightUp.collider.GetComponent<MoveGround>() != null)
                            {
                                if (hit_RightUp.collider.GetComponent<MoveGround>().IsPike == false)
                                {
                                    GetComponent<Rigidbody2D>().velocity = new Vector2(1f, 1f) * MoveSpeed;
                                }
                            }
                            else
                            {
                                CatAni.SetBool("Move", false);
                                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                                WaterJump(Rot);
                            }
                           
                        }
                        if (CanJumpTrue == true)
                        {
                            CanJump = true;
                        }
                    }
                }
                if (hit_RightDown.collider != null)
                {
                    if (hit_RightDown.collider.gameObject.tag == "Ground" || hit_RightDown.collider.gameObject.tag == "Wall")
                    {
                        GetComponent<Rigidbody2D>().gravityScale = 0;
                        CatAni.SetBool("Jump", false);

                        CatAni.SetFloat("TurnRight", 0.666f);
                        transform.rotation = Quaternion.Euler(0, 0, 90);

                        if (Input.GetKey(KeyCode.S) || WhatKey_ == "S")
                        {
                            TurnRight = false;
                            CatAni.SetBool("Move", true);
                            GetComponent<Rigidbody2D>().velocity = new Vector2(0, -MoveSpeed * 0.7f);
                        }
                        else if (Input.GetKey(KeyCode.D) || WhatKey_ == "D")
                        {
                            TurnRight = true;
                            CatAni.SetBool("Move", true);
                            GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed * 0.7f, 0);
                        }
                        else
                        {
                          
                            if (hit_RightDown.collider != null && hit_RightDown.collider.GetComponent<MoveGround>() != null)
                            {
                                if (hit_RightDown.collider.GetComponent<MoveGround>().IsPike == false)
                                {
                                    GetComponent<Rigidbody2D>().velocity = new Vector2(1f, -1f) * MoveSpeed;
                                }
                            }
                            else
                            {
                                CatAni.SetBool("Move", false);
                                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                                WaterJump(Rot);
                            }
                          
                        }
                        if (CanJumpTrue == true)
                        {
                            CanJump = true;
                        }
                    }
                }


                if (hit_LeftUp.collider != null)
                {
                    
                }
                else if (hit_LeftDown.collider != null)
                {
                  
                }
                else if (hit_RightUp.collider != null)
                {
                  
                }
                else if (hit_RightDown.collider != null)
                {
                   
                }
                else
                {
                    if (WaterFly == false)
                    {
                        GetComponent<Rigidbody2D>().gravityScale = CatWeight;
                    }

                    if (GetComponent<Rigidbody2D>().velocity.x > 0.5f)
                    {
                        CatAni.SetFloat("TurnRight", 1);
                        TurnRight = true;
                    }
                    else if (GetComponent<Rigidbody2D>().velocity.x < -0.5f)
                    {
                        CatAni.SetFloat("TurnRight", 0);
                        TurnRight = false;
                    }
                    else
                    {
                        CatAni.SetFloat("TurnRight", 0.5f);
                    }


                    if (Input.GetKey(KeyCode.D) || WhatKey_ == "D")
                    {
                        TurnRight = true;
                        CatAni.SetFloat("TurnRight", 1);
                        if (NowCatAct == CatAct.Idle || NowCatAct == CatAct.Run)
                        {
                            transform.rotation = Quaternion.Euler(0, 0, 0);
                            CatAni.SetBool("Move", true);
                            NowCatAct = CatAct.Run;
                        }
                        GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed, GetComponent<Rigidbody2D>().velocity.y);
                    }
                    else if (Input.GetKey(KeyCode.A) || WhatKey_ == "A")
                    {
                        TurnRight = false;
                        CatAni.SetFloat("TurnRight", 0);
                        if (NowCatAct == CatAct.Idle || NowCatAct == CatAct.Run)
                        {
                            transform.rotation = Quaternion.Euler(0, 180, 0);
                            CatAni.SetBool("Move", true);
                            NowCatAct = CatAct.Run;
                        }
                        GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed, GetComponent<Rigidbody2D>().velocity.y);
                    }
                    else if (CanJump != false && NowCatAct != CatAct.Jump)
                    {
                        CatAni.SetBool("Move", false);
                        GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
                    }
                    WaterJump(Rot);
                }
            }
        }
        else if (Rot == new Vector2(0, -1))
        {
            if (WaterFly == false)
            {
                GetComponent<Rigidbody2D>().gravityScale = CatWeight;
            }

            if (CatAni.GetFloat("TurnRight") == 0.5f)
            {
                CatAni.SetFloat("TurnRight", 1);
            }
            if (CatAni.GetFloat("TurnRight") == 0.666f)
            {
                CatAni.SetFloat("TurnRight", 1);
            }


            if ((Input.GetKey(KeyCode.D) || WhatKey_ == "D") && WaterJumpReady == false)
            {
                TurnRight = true;
                CatAni.SetFloat("TurnRight", 1);
                if (NowCatAct == CatAct.Idle || NowCatAct == CatAct.Run)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    CatAni.SetBool("Move", true);
                    NowCatAct = CatAct.Run;
                }
                GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed, GetComponent<Rigidbody2D>().velocity.y);
            }
            else if ((Input.GetKey(KeyCode.A) || WhatKey_ == "A") && WaterJumpReady == false)
            {
                TurnRight = false;
                CatAni.SetFloat("TurnRight", 0);
                if (NowCatAct == CatAct.Idle || NowCatAct == CatAct.Run)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    CatAni.SetBool("Move", true);
                    NowCatAct = CatAct.Run;
                }
                GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed, GetComponent<Rigidbody2D>().velocity.y);
            }
            else if (CanJump != false && NowCatAct != CatAct.Jump)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
            }
            WaterJump(Rot);

        }
        else if (Rot == new Vector2(-1, -1))
        {
            GetComponent<Rigidbody2D>().gravityScale = 0;

            CatAni.SetFloat("TurnRight", 0.333f);

            transform.rotation = Quaternion.Euler(0, 0, 0);

            if (Input.GetKey(KeyCode.W) || WhatKey_ == "W")
            {
                TurnRight = true;
                CatAni.SetBool("Move", true);
                GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed / 2, MoveSpeed * 0.7f);
            }
            else if (Input.GetKey(KeyCode.D) || WhatKey_ == "D")
            {
                TurnRight = false;
                CatAni.SetBool("Move", true);
                GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed * 0.7f, -MoveSpeed / 2);
            }
            else
            {
                CatAni.SetBool("Move", false);
                GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed / 2, -MoveSpeed / 2);
            }
        }
        else if (Rot == new Vector2(1, -1))
        {
            GetComponent<Rigidbody2D>().gravityScale = 0;

            CatAni.SetFloat("TurnRight", 0.333f);

            transform.rotation = Quaternion.Euler(0, 0, 90);

            if (Input.GetKey(KeyCode.W) || WhatKey_ == "W")
            {
                TurnRight = false;
                CatAni.SetBool("Move", true);
                GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed / 2, MoveSpeed * 0.7f);
            }
            else if (Input.GetKey(KeyCode.A) || WhatKey_ == "A")
            {
                TurnRight = true;
                CatAni.SetBool("Move", true);
                GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed * 0.7f, -MoveSpeed / 2);
            }
            else
            {
                CatAni.SetBool("Move", false);
                GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed / 2, -MoveSpeed / 2);
            }
        }
        else if (Rot == new Vector2(1, 1))
        {
            GetComponent<Rigidbody2D>().gravityScale = 0;

            CatAni.SetFloat("TurnRight", 0.333f);

            transform.rotation = Quaternion.Euler(0, 0, 180);

            if (Input.GetKey(KeyCode.S) || WhatKey_ == "S")
            {
                TurnRight = false;
                CatAni.SetBool("Move", true);
                GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed / 2, -MoveSpeed * 0.7f);
            }
            else if (Input.GetKey(KeyCode.A) || WhatKey_ == "A")
            {
                TurnRight = true;
                CatAni.SetBool("Move", true);
                GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed * 0.7f, MoveSpeed / 2);
            }
            else
            {
                CatAni.SetBool("Move", false);
                GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed / 2, MoveSpeed / 2);
            }
        }
        else if (Rot == new Vector2(-1, 1))
        {
            GetComponent<Rigidbody2D>().gravityScale = 0;

            CatAni.SetFloat("TurnRight", 0.333f);

            transform.rotation = Quaternion.Euler(0, 0, -90);

            if (Input.GetKey(KeyCode.S) || WhatKey_ == "S")
            {
                TurnRight = false;
                CatAni.SetBool("Move", true);
                GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed / 2, -MoveSpeed * 0.7f);
            }
            else if (Input.GetKey(KeyCode.D) || WhatKey_ == "D")
            {
                TurnRight = true;
                CatAni.SetBool("Move", true);
                GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed * 0.7f, MoveSpeed / 2);
            }
            else
            {
                CatAni.SetBool("Move", false);
                GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed / 2, MoveSpeed / 2);
            }
        }
        else
        {
            if (Rot.x == -1 && Ray_Left == true)
            {
                GetComponent<Rigidbody2D>().gravityScale = 0;
                if (Input.GetKey(KeyCode.W) || WhatKey_ == "W")
                {
                    TurnRight = true;
                    CatAni.SetBool("Move", true);
                    GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed, MoveSpeed * 0.7f);
                }
                else if (Input.GetKey(KeyCode.S) || WhatKey_ == "S")
                {
                    TurnRight = false;
                    CatAni.SetBool("Move", true);
                    GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed, -MoveSpeed);
                }
                else
                {
                    CatAni.SetBool("Move", false);
                    GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed, 0);
                    Debug.Log("AAA");
                }

                if (TurnRight == true)
                {
                    RotForWall = Quaternion.Euler(0, 180, 90);
                }
                else
                {
                    RotForWall = Quaternion.Euler(0, 0, -90);
                }
                transform.rotation = RotForWall;
            }
            else if (Rot.x == 1 && Ray_Right == true)
            {
                GetComponent<Rigidbody2D>().gravityScale = 0;
                //GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed, GetComponent<Rigidbody2D>().velocity.y);
                GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed, -MoveSpeed * 0.1f);

                if ((Input.GetKey(KeyCode.W) || WhatKey_ == "W") && WaterJumpReady == false)
                {
                    TurnRight = true;
                    CatAni.SetBool("Move", true);
                    GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed, MoveSpeed * 0.7f);
                }
                else if ((Input.GetKey(KeyCode.S) || WhatKey_ == "S") && WaterJumpReady == false)
                {
                    TurnRight = false;
                    CatAni.SetBool("Move", true);
                    GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed, -MoveSpeed);
                }
                else
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed, 0);
                    CatAni.SetBool("Move", false);
                    Debug.Log("DDD");
                }

                if (TurnRight == true)
                {
                    RotForWall = Quaternion.Euler(0, 0, 90);
                }
                else
                {
                    RotForWall = Quaternion.Euler(0, 180, -90);
                }
                transform.rotation = RotForWall;
            }
            else if (Rot.y == 1 && Ray_Up == true)
            {
                GetComponent<Rigidbody2D>().gravityScale = 0;
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, MoveSpeed);

                if ((Input.GetKey(KeyCode.A) || WhatKey_ == "A") && WaterJumpReady == false)
                {
                    if (NowCatMorph == CatMorph.Climb)
                    {
                        TurnRight = true;
                        CatAni.SetBool("Move", true);
                        GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed * 0.7f, MoveSpeed);
                    }
                    else
                    {
                        CatAni.SetBool("Move", false);
                        TurnRight = true;
                    }
                }
                else if ((Input.GetKey(KeyCode.D) || WhatKey_ == "D") && WaterJumpReady == false)
                {
                    if (NowCatMorph == CatMorph.Climb)
                    {
                        TurnRight = false;
                        CatAni.SetBool("Move", true);
                        GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed * 0.7f, MoveSpeed);
                    }
                    else
                    {
                        CatAni.SetBool("Move", false);
                        TurnRight = false;
                    }
                }
                else
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(0, MoveSpeed);
                    CatAni.SetBool("Move", false);
                    Debug.Log("WWW");
                }

                if (TurnRight == true)
                {
                    RotForWall = Quaternion.Euler(0, 0, 180);
                }
                else
                {
                    RotForWall = Quaternion.Euler(0, 180, 180);
                }
                transform.rotation = RotForWall;
            }
            else
            {
                Climb = false;
            }


            if (RotForWall.y == 0)
            {
                CatAni.SetFloat("TurnRight", 1);
            }
            else
            {
                CatAni.SetFloat("TurnRight", 0);
            }

            WaterJump(Rot);

        }
    }

    public void WaterJumpPictrue()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        // 滑鼠偵測
        Vector3 MousePos;
        MousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

        Vector3 direction = MousePos - transform.position;
#elif UNITY_ANDROID
        // 觸碰偵測
        Vector3 direction = Touch_Right.GetComponent<FixedJoystickHandler>().direction;
#endif



        direction.z = 0f;
        direction.Normalize();
        //float targetAngle = Mathf.Atan2(direction.y, direction.x);

        //float Remove = Vector2.Distance(MousePos, transform.position);

        Vector3 RemoveMax = transform.position + direction * LongRemoveMax;
        Vector3 RemoveMin = transform.position + direction * 2.3f;

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        WaterJumpPos_1.transform.rotation = Quaternion.Euler(0, 0, targetAngle - 90);
        WaterJumpPos_2.transform.rotation = Quaternion.Euler(0, 0, targetAngle - 90);


        WaterJumpPos_1.transform.position = RemoveMax;
        WaterJumpPos_2.transform.position = RemoveMax;

        //if (Remove <= 2f)
        //{
        //    WaterJumpPos_1.transform.position = RemoveMin;
        //    WaterJumpPos_2.transform.position = RemoveMin;
        //    WaterJumpPos_2.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
        //}
        //else
        //{
        //    WaterJumpPos_1.transform.position = RemoveMax;
        //    WaterJumpPos_2.transform.position = RemoveMax;
        //    WaterJumpPos_2.GetComponent<SpriteRenderer>().color = new Color(0, 1, 1, 1);
        //}


        WaterJumpPos_1.GetComponent<LineRenderer>().SetPosition(0, transform.position);
        WaterJumpPos_1.GetComponent<LineRenderer>().SetPosition(1, WaterJumpPos_1.transform.position);
        WaterJumpPos_2.GetComponent<LineRenderer>().SetPosition(0, transform.position);
        Vector2 P_2 = Vector2.Lerp(transform.position, WaterJumpPos_2.transform.position, WaterJumpPower);
        WaterJumpPos_2.GetComponent<LineRenderer>().SetPosition(1, P_2);

    }

    public void WaterBack(Vector3 partner)
    {
        //Vector3 partner;
        //partner = nowWaterLongBody.transform.GetChild(0).transform.position;
        //
        Vector3 direction = partner - transform.position;
        direction.z = 0f;
        direction.Normalize();

        GetComponent<Rigidbody2D>().AddForce(direction * 1000); // 第二種版本
    }

    public bool WaterFly;

    public void WaterJump(Vector2 RayVect)
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0) || Key_0 == true)
        {
            // 滑鼠偵測
            if (CanJump == true)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
                GetComponent<Rigidbody2D>().gravityScale = 0.01f;
                GetComponent<Animator>().SetBool("WaterJumpReady", true);
                //WaterJumpPower = 0.1f;
                WaterJumpPower = 1f;
                WaterJumpReady = true;
            } 
        }
#elif UNITY_ANDROID
        if (Key_0 == true)
        {
                // 觸碰偵測
                if(Key_0_Drag == false)
                {
                    if (CanJump == true)
                    {
                        GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
                        GetComponent<Rigidbody2D>().gravityScale = 0.01f;
                        GetComponent<Animator>().SetBool("WaterJumpReady", true);
                        //WaterJumpPower = 0.1f;
                        WaterJumpPower = 1f;
                        WaterJumpReady = true;
                    }
                    Key_0_Drag = true;//只偵測一次
                }
        }
#endif
        if (WaterJumpReady == true)
        {
            WaterJumpPictrue();
            WaterJumpPos_1.SetActive(true);
            WaterJumpPos_2.SetActive(true);
            //WaterJumpPower += Time.deltaTime * 0.5f;
            //if (WaterJumpPower >= 1)
            //{
            //    WaterJumpPower = 1;
            //}


#if UNITY_EDITOR || UNITY_STANDALONE
            // 滑鼠偵測
            Vector3 MousePos;
            MousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

            Vector3 direction = MousePos - transform.position;
#elif UNITY_ANDROID
        // 觸碰偵測
        Vector3 direction = Touch_Right.GetComponent<FixedJoystickHandler>().direction;
#endif


            direction.z = 0f;
            direction.Normalize();
            Vector3 PowerPath = direction;
            Debug.Log(PowerPath);

            if (CatAni.GetFloat("TurnRight") != 0.666f)
            {
                if (PowerPath.x * 2 >= 0.8f)
                {
                    CatAni.SetFloat("TurnRight", 1);
                    TurnRight = true;
                }
                else if (PowerPath.x * 2 <= -0.8f)
                {
                    CatAni.SetFloat("TurnRight", 0);
                    TurnRight = false;
                }
                else
                {
                    CatAni.SetFloat("TurnRight", 0.333f);
                    TurnRight = true;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {

                Debug.Log(RayVect);
                if (RayVect.x == -1)
                {
                    Ray_Left = false;
                    StartCoroutine(RayL_Check());
                }
                if (RayVect.x == 1)
                {
                    Ray_Right = false;
                    StartCoroutine(RayR_Check());
                }
                if (RayVect.y == 1)
                {
                    Ray_Up = false;
                    StartCoroutine(RayU_Check());
                }


                //CloudTime = 5;
                if (CanJump == true)
                {
                    nowWaterLongBody = Instantiate(WaterLongBody, transform.position, Quaternion.Euler(0, 0, 0));
                    WaterFly = true;

                    //

                    CatMusic.PlayMusic(0);
                    NowCatAct = CatAct.Jump;
                    CanJump = false;
                    StartCoroutine(JumpDebug(0.3f));

                    //GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);

                    //
                    //GetComponent<Rigidbody2D>().gravityScale = CatWeight;
                    //GetComponent<Rigidbody2D>().AddForce(PowerPath * JumpPower * 2 * WaterJumpPower);
                    //GetComponent<Animator>().SetBool("Jump", true);
                    //GetComponent<Animator>().SetBool("WaterJumpReady", false);
                    WaterJumpPos_1.SetActive(false);
                    WaterJumpPos_2.SetActive(false);
                    //

                    GetComponent<Rigidbody2D>().gravityScale = 0f;
                    //GetComponent<Collider2D>().isTrigger = true;
                    GetComponent<Rigidbody2D>().AddForce(PowerPath * JumpPower * 3 * WaterJumpPower);
                    GetComponent<Animator>().SetBool("Jump", true);
                    GetComponent<Animator>().SetBool("WaterJumpReady", false);

                    //if (PowerPath.x * 2 >= 0.2f)
                    //{
                    //    CatAni.SetFloat("TurnRight", 1);
                    //    TurnRight = true;
                    //}
                    //else if (PowerPath.x * 2 <= -0.2f)
                    //{
                    //    CatAni.SetFloat("TurnRight", 0);
                    //    TurnRight = false;
                    //}
                    //else
                    //{
                    //    CatAni.SetFloat("TurnRight", 0.5f);
                    //    TurnRight = true;
                    //}
                    CatAni.SetFloat("TurnRight", 0.5f);
                    TurnRight = true;

                    WaterJumpReady = false;
                }
            }

            if (Input.GetMouseButtonUp(1))
            {
                CloseWaterJump();
            }
        }
    }

    public void CloseWaterJump()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        // 滑鼠偵測
        Vector3 MousePos;
        MousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

        Vector3 direction = MousePos - transform.position;
#elif UNITY_ANDROID
        // 觸碰偵測
        Vector3 direction = Touch_Right.GetComponent<FixedJoystickHandler>().direction;
#endif


        direction.z = 0f;
        direction.Normalize();
        Vector3 PowerPath = direction;


        GetComponent<Rigidbody2D>().gravityScale = CatWeight;
        WaterJumpReady = false;
        WaterJumpPos_1.SetActive(false);
        WaterJumpPos_2.SetActive(false);
        GetComponent<Animator>().SetBool("WaterJumpReady", false);
        GetComponent<Animator>().SetBool("Jump", false);
        if (PowerPath.x * 2 >= 0.2f)
        {
            CatAni.SetFloat("TurnRight", 1);
            TurnRight = true;
        }
        else if (PowerPath.x * 2 <= -0.2f)
        {
            CatAni.SetFloat("TurnRight", 0);
            TurnRight = false;
        }
        else
        {
            CatAni.SetFloat("TurnRight", 1f);
            TurnRight = true;
        }
        WaterFly = false;
        Destroy(nowWaterLongBody);
    }
    public void RayGround()
    {


        //float RotForWall_Y = 0;

        if ((Input.GetKey(KeyCode.A) || WhatKey_ == "A") && CanJumpTrue == true)
        {
            if(Rot.x == -1)
            {
                //GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed, GetComponent<Rigidbody2D>().velocity.y);
                GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed, -MoveSpeed * 0.06f);//貼著牆壁的下墜速度

                if (NowCatMorph != CatMorph.Climb)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        if (CanJump == true)
                        {
                            NowCatAct = CatAct.Jump;

                            CatMusic.PlayMusic(0);
                            GetComponent<Rigidbody2D>().velocity = new Vector3(GetComponent<Rigidbody2D>().velocity.x, 0, 0);
                            GetComponent<Rigidbody2D>().AddForce(new Vector3(0, JumpPower, 0));
                            CanJumpTrue = false;
                            StartCoroutine(JumpDebug(0.3f));
                            CanJump = false;
                        }
                    }
                }
            

                if (NowCatMorph == CatMorph.Climb)
                {
                    if (Input.GetKey(KeyCode.W) || WhatKey_ == "W")
                    {
                        TurnRight = true;
                        CatAni.SetBool("Move", true);
                        GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed, MoveSpeed * 1.2f);
                    }
                    else if (Input.GetKey(KeyCode.S) || WhatKey_ == "S")
                    {
                        TurnRight = false;
                        CatAni.SetBool("Move", true);
                        GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed, -MoveSpeed);
                    }
                    else
                    {
                        if (Climb == true)
                        {
                            CatAni.SetBool("Move", false);
                            Debug.Log("AAA");
                        }
                    }
                }
                else
                {
                    CatAni.SetBool("Move", false);
                    TurnRight = true;
                }
            }
        }
        if ((Input.GetKey(KeyCode.D) || WhatKey_ == "D") && CanJumpTrue == true)
        {
            if (Rot.x == 1)
            {
                //GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed, GetComponent<Rigidbody2D>().velocity.y);
                GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed, -MoveSpeed * 0.06f);//貼著牆壁的下墜速度

                if (NowCatMorph != CatMorph.Climb)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        if (CanJump == true)
                        {
                            NowCatAct = CatAct.Jump;

                            CatMusic.PlayMusic(0);
                            GetComponent<Rigidbody2D>().velocity = new Vector3(GetComponent<Rigidbody2D>().velocity.x, 0, 0);
                            GetComponent<Rigidbody2D>().AddForce(new Vector3(0, JumpPower, 0));
                            CanJumpTrue = false;
                            StartCoroutine(JumpDebug(0.3f));
                            CanJump = false;
                        }
                    }
                }

                if (NowCatMorph == CatMorph.Climb)
                {
                    if (Input.GetKey(KeyCode.W) || WhatKey_ == "W")
                    {
                        TurnRight = true;
                        CatAni.SetBool("Move", true);
                        GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed, MoveSpeed * 1.2f);
                    }
                    else if (Input.GetKey(KeyCode.S) || WhatKey_ == "S")
                    {
                        TurnRight = false;
                        CatAni.SetBool("Move", true);
                        GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed, -MoveSpeed);
                    }
                    else
                    {
                        if (Climb == true)
                        {
                            CatAni.SetBool("Move", false);
                            Debug.Log("DDD");
                        }
                    }
                }
                else
                {
                    CatAni.SetBool("Move", false);
                    TurnRight = true;
                }

            }
        }
        if (Input.GetKey(KeyCode.W) || WhatKey_ == "W")
        {
            if (Rot.y == 1)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, MoveSpeed);

                if (Input.GetKey(KeyCode.A) || WhatKey_ == "A")
                {
                    if(NowCatMorph == CatMorph.Climb)
                    {
                        TurnRight = true;
                        CatAni.SetBool("Move", true);
                        GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed * 1.2f, MoveSpeed);
                    }
                    else
                    {
                        CatAni.SetBool("Move", false);
                        TurnRight = true;
                    }
                }
                else if (Input.GetKey(KeyCode.D) || WhatKey_ == "D")
                {
                    if (NowCatMorph == CatMorph.Climb)
                    {
                        TurnRight = false;
                        CatAni.SetBool("Move", true);
                        GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed * 1.2f, MoveSpeed);
                    }
                    else
                    {
                        CatAni.SetBool("Move", false);
                        TurnRight = false;
                    }
                }
                else
                {
                    if (Climb == true)
                    {
                        CatAni.SetBool("Move", false);
                        Debug.Log("WWW");
                    }
                }
            }
        }

     


        if (Rot == new Vector2(-1, 0))
        {
            if (TurnRight == true)
            {
                RotForWall = Quaternion.Euler(0, 180, 90);
            }
            else
            {
                RotForWall = Quaternion.Euler(0, 0, -90);
            }
            //transform.rotation = Quaternion.Slerp(Quaternion.Euler(transform.rotation.x, RotForWall.y, transform.rotation.z), RotForWall, 5 * Time.deltaTime);
            transform.rotation = RotForWall;

        }
        else if (Rot == new Vector2(1, 0))
        {
            if (TurnRight == true)
            {
                RotForWall = Quaternion.Euler(0, 0, 90);
            }
            else
            {
                RotForWall = Quaternion.Euler(0, 180, -90);
            }    
            transform.rotation = RotForWall;
        }
        else if (Rot == new Vector2(0, 1))
        {
            if (TurnRight == true)
            {
                RotForWall = Quaternion.Euler(0, 0, 180);
            }
            else
            {
                RotForWall = Quaternion.Euler(0, 180, 180);
            }
            transform.rotation = RotForWall;
        }
        else if (Rot == new Vector2(1, 1))
        {
            if (TurnRight == true)
            {
                RotForWall = Quaternion.Euler(0, 0, 135);
            }
            else
            {
                RotForWall = Quaternion.Euler(0, 180, -135);
            }
            transform.rotation = RotForWall;
        }
        else if (Rot == new Vector2(1, -1))
        {
            if (TurnRight == true)
            {
                RotForWall = Quaternion.Euler(0, 0, 45);
            }
            else
            {
                RotForWall = Quaternion.Euler(0, 180, -45);
            }
            transform.rotation = RotForWall;
        }
        else if (Rot == new Vector2(-1, -1))
        {
            if (TurnRight == true)
            {
                RotForWall = Quaternion.Euler(0, 180, 45);
            }
            else
            {
                RotForWall = Quaternion.Euler(0, 0, -45);
            }
            transform.rotation = RotForWall;
        }
        else if (Rot == new Vector2(-1, 1))
        {
            if (TurnRight == true)
            {
                RotForWall = Quaternion.Euler(0, 180, 135);
            }
            else
            {
                RotForWall = Quaternion.Euler(0, 0, -135);
            }
            transform.rotation = RotForWall;
        }
        else if (Rot == new Vector2(0, 0) || Rot == new Vector2(0, -1))
        {
            Climb = false;
        }

     
        if (RotForWall.y == 0)
        {
            CatAni.SetFloat("TurnRight", 1);
        }
        else
        {
            CatAni.SetFloat("TurnRight", 0);
        }

        //if (hit.collider)
        //{
        //    if (hit.collider.gameObject.tag == "Ground")
        //    {
        //        if (NowCatAct == CatAct.LongLongCat || NowCatAct == CatAct.LongDownCat)
        //        {

        //        }
        //        else
        //        {
        //            NowCatAct = CatAct.Idle;
        //            if (TurnRight == true)
        //            {
        //                transform.rotation = Quaternion.Euler(0, 0, 0);
        //            }
        //            else
        //            {
        //                transform.rotation = Quaternion.Euler(0, 180, 0);
        //            }
        //        }
        //        CanLong = true;
        //        CanJump = true;
        //        CatAni.SetBool("Jump", false);
        //    }
        //}
        //else
        //{
        //    CanJump = false;
        //    CatAni.SetBool("Jump", true);
        //}

    }

    public void LongLongCat()//很長很長的貓
    {

#if UNITY_EDITOR || UNITY_STANDALONE
        // 滑鼠偵測
        Vector3 MousePos;
        MousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

        Vector3 direction = MousePos - transform.position;
#elif UNITY_ANDROID
        // 觸碰偵測
        Vector3 direction = Touch_Right.GetComponent<FixedJoystickHandler>().direction;
#endif

        direction.z = 0f;
        direction.Normalize();
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, targetAngle), TurnSpeed * Time.deltaTime);

        //GetComponent<Rigidbody2D>().MoveRotation(Quaternion.Euler(0, 0, targetAngle - TurnRun));
        transform.rotation = Quaternion.Euler(0, 0, targetAngle);

       
        HandPos = nowLongBody.transform.GetChild(0).transform.position;
        Vector3 worldPos = HandPos;

        if (worldPos.x < transform.position.x)
        {
            TurnRight = false;
            CatAni.SetFloat("TurnRight", 0);
        }
        else
        {
            TurnRight = true;
            CatAni.SetFloat("TurnRight", 1);
        }

    }

    public void FallowHand()//屁股縮回去
    {
        HandPos = nowLongBody.transform.GetChild(0).transform.position;
        float ButtRemove = Vector2.Distance(transform.position, HandPos);
        Vector2 RemovePos = HandPos - new Vector2(transform.position.x, transform.position.y);
        if (ButtRemove >= LongRemoveMin)
        {
            GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(transform.position, HandPos, 0.15f));


            //GetComponent<Rigidbody2D>().AddForce(RemovePos * ButtRemove * 30);

            //Butt.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector3(MoveSpeed, 0, 0));
            //Butt.transform.position = Vector2.Lerp(Butt.transform.position, transform.position, 0.1f);
        }
    }
    public void StopCat(float DelayTime)
    {
        AllCoinSave();
        StartCoroutine(CatStopDelay(DelayTime));
    }

    public IEnumerator CatStopDelay(float DelayTime)
    {
        NowCatAct = CatAct.CatStop;
        yield return new WaitForSeconds(DelayTime);
        NowCatAct = CatAct.Idle;
    }
    public IEnumerator CatDeath()
    {
        NowCatAct = CatAct.CatDie;
        Phone_UI.SetActive(false);
        CatMusic.PlayMusic(1);
        GetComponent<Animator>().SetBool("Cloud", false);
        CatAni.SetBool("Die", true);
        if (TurnRight == true)
        {
            transform.rotation = Quaternion.Euler(0, 0, 15);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 15);
        }
        Black.SetActive(true);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        GetComponent<Rigidbody2D>().gravityScale = 2.5f;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 18000));
        WaterJumpPos_1.SetActive(false);
        WaterJumpPos_2.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        transform.position = new Vector2(PlayerPrefs.GetFloat("CatPos_X"), PlayerPrefs.GetFloat("CatPos_Y")) + new Vector2(0, 0.2f);
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        GetComponent<Rigidbody2D>().gravityScale = CatWeight;
        GetComponent<Collider2D>().enabled = true;
        GetComponent<Collider2D>().isTrigger = false;
        GetComponent<Animator>().SetBool("Cloud", false);
        AllStart();
        CatAni.SetBool("Die", false);
        yield return new WaitForSeconds(1);
        Black.SetActive(false);
        NowCatAct = CatAct.Idle;
#if UNITY_EDITOR || UNITY_STANDALONE
        Phone_UI.SetActive(false);   // 滑鼠偵測
#elif UNITY_ANDROID
		Phone_UI.SetActive(true);  // 觸碰偵測
#endif
    }

    public void AllStart()
    {
        CoinBox[] coinBox = GameObject.FindObjectsOfType<CoinBox>();
        for (int i = 0; i < coinBox.Length; i++)
        {
            coinBox[i].SetStart();
        }
        DownPike[] downPike = GameObject.FindObjectsOfType<DownPike>();
        for (int i = 0; i < downPike.Length; i++)
        {
            downPike[i].SetStart();
        }
        MoveGround[] moveGround = GameObject.FindObjectsOfType<MoveGround>();
        for (int i = 0; i < moveGround.Length; i++)
        {
            moveGround[i].SetStart();
        }
    }
    public void AllCoinSave()
    {
        CoinBox[] coinBox = GameObject.FindObjectsOfType<CoinBox>();
        for (int i = 0; i < coinBox.Length; i++)
        {
            coinBox[i].PlayerSave();
        }
    }

    [Header("按了甚麼鍵")]
    public string WhatKey_;

    //[Header("跳躍鍵")]
    //public bool JumpKey;

    [Header("是否按了滑鼠左鍵")]
    public bool Key_0;//滑鼠左鍵
    public bool Key_0_Drag;//滑鼠左鍵按住
    //private bool Key_1;//滑鼠右鍵

    public void GetStart(string Key_)
    {
        WhatKey_ = Key_;
    }

    public void GetStartRight()
    {
        Key_0 = true;
    }

    public void GetEnd_Left()
    {
        WhatKey_ = "";
    }

    public void GetEnd_Right()
    {
        //WhatKey_ = "";
        Key_0 = false;
        StartCoroutine(RightTouch_Debug(0.1f));
    }

    public IEnumerator RightTouch_Debug(float T)
    {
        yield return new WaitForSeconds(T);
        Key_0_Drag = false;
    }

    public void GetJump()
    {
        if (NowCatAct == CatAct.LongLongCat || NowCatAct == CatAct.Back || NowCatAct == CatAct.CatStop || Black.active == true)
        {

        }
        else if (RayWall() == true)
        {
            if (CanJump == true)
            {
                NowCatAct = CatAct.Jump;

                CatMusic.PlayMusic(0);
                GetComponent<Rigidbody2D>().velocity = new Vector3(GetComponent<Rigidbody2D>().velocity.x, 0, 0);
                GetComponent<Rigidbody2D>().AddForce(new Vector3(0, JumpPower, 0));
                CanJumpTrue = false;
                StartCoroutine(JumpDebug(0.3f));
                CanJump = false;
            }
        }
        else
        {
            if (CanJump == true)
            {
                NowCatAct = CatAct.Jump;

                CatMusic.PlayMusic(0);
                GetComponent<Rigidbody2D>().velocity = new Vector3(GetComponent<Rigidbody2D>().velocity.x, 0, 0);
                GetComponent<Rigidbody2D>().AddForce(new Vector3(0, JumpPower, 0));
                CanJumpTrue = false;
                StartCoroutine(JumpDebug(0.3f));
                CanJump = false;
            }
        }
    }

        

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Bar" || collision.gameObject.tag == "DoorGround")
        {
            if (NowCatAct == CatAct.LongLongCat || NowCatAct == CatAct.LongDownCat || NowCatAct == CatAct.Back)
            {

            }
            else
            {
                if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || WhatKey_ == "A" || WhatKey_ == "D" || WhatKey_ == "W")
                {

                }
                else
                {
                    if (CanJumpTrue == true)
                    {
                        if (NowCatAct != CatAct.CatStop)
                        {
                            NowCatAct = CatAct.Idle;
                        }
                    }
                }

                if (TurnRight == true)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
            }

            if (CanLongTrue == true)
            {
                //LongLight.SetActive(true);
                CanLong = true;
            }
            if (CanJumpTrue == true)
            {
                CanJump = true;
                CatAni.SetBool("Jump", false);
            }
            else
            {
                StartCoroutine(JumpDebug(0.3f));
            }

            //if (NowCatMorph == CatMorph.Climb)
            //{
            //    if (Ray_Left == false)
            //    {
            //        StartCoroutine(RayL_Check());
            //    }
            //    if (Ray_Right == false)
            //    {
            //        StartCoroutine(RayR_Check());
            //    }
            //    if (Ray_Up == false)
            //    {
            //        StartCoroutine(RayU_Check());
            //    }
            //}

            if (GetComponent<Rigidbody2D>().gravityScale != 0.01f)
            {
                CloudTime = 3;
            }

            if (WaterFly == true && CanJumpTrue == true)
            {
                nowWaterLongBody.transform.GetChild(0).GetComponent<FindPartner_Water>().NextAct = true;
                WaterFly = false;
            }
        }
        if (collision.gameObject.tag == "Wall")
        {
            if (NowCatMorph == CatMorph.Climb)
            {
                if (NowCatAct == CatAct.LongLongCat || NowCatAct == CatAct.LongDownCat || NowCatAct == CatAct.CatStop || NowCatAct == CatAct.Back)
                {

                }
                else
                {
                    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || WhatKey_ != "")
                    {

                    }
                    else
                    {
                        if (CanJumpTrue == true)
                        {
                            if (NowCatAct != CatAct.CatStop)
                            {
                                NowCatAct = CatAct.Idle;
                            }
                        }
                    }
                }
                if (CanLongTrue == true)
                {
                    //LongLight.SetActive(true);
                    CanLong = true;
                }
                if (CanJumpTrue == true)
                {
                    CanJump = true;
                    CatAni.SetBool("Jump", false);
                }

                if (WaterFly == true && CanJumpTrue == true)
                {
                    nowWaterLongBody.transform.GetChild(0).GetComponent<FindPartner_Water>().NextAct = true;
                    WaterFly = false;
                }
            }
        }

        //if (collision.gameObject.tag == "Wall")
        //{
        //    if (NowCatMorph == CatMorph.Climb || NowCatMorph == CatMorph.NoMorph)
        //    {

        //    }
        //}

        if (collision.gameObject.tag == "Pike")
        {
            //碰到陷阱，到儲存點復活
            StartCoroutine(CatDeath());
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        //if (collision.gameObject.tag == "Bar")
        //{
        //    if (Input.GetKeyDown(KeyCode.S))
        //    {
        //        Butt.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        //        Butt.GetComponent<Rigidbody2D>().gravityScale = 0;
        //    }
        //    if (Input.GetKey(KeyCode.S))
        //    {
        //        DownRemove += DownSpeed * Time.deltaTime;
        //        NowCatAct = CatAct.LongDownCat;
        //        Debug.Log("Down");
        //    }

        //}


        if (collision.gameObject.tag == "Props")//道具
        {
            if (GetObject != null)
            {
                GetObject.GetComponent<CanGetObject>().SetStart(collision.transform.position);
            }
            GetObject = collision.gameObject;
            if (GetObject.GetComponent<CanGetObject>() != null)
            {
                GetObject.GetComponent<CanGetObject>().SetGet();
            }
        }
        if (collision.gameObject.tag == "Pike")
        {
            //碰到陷阱，到儲存點復活
            StartCoroutine(CatDeath());
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Pike")
        {
            //碰到陷阱，到儲存點復活
            StartCoroutine(CatDeath());
        }
        if (collision.gameObject.tag == "CanHold")
        {
            if (collision.GetComponent<SpriteRenderer>().sprite != collision.transform.parent.GetComponent<RedBall>().TouchPictrue)
            {
                collision.GetComponent<SpriteRenderer>().sprite = collision.transform.parent.GetComponent<RedBall>().TouchPictrue;
                collision.GetComponent<CreateAni>().Create(collision.transform.position, 2);
            }
            else
            {
                collision.GetComponent<CreateAni>().Create(collision.transform.position, 2);
            }

            if (collision.transform.parent.GetComponent<RedBall>().IsOne == true)
            {
                collision.transform.parent.GetComponent<Animator>().SetBool("Break", true);
                collision.transform.parent.GetComponent<RedBall>().OpenAgain();
            }

            if (collision.transform.parent.GetComponent<RedBall>().IsOne == false)
            {
                collision.transform.parent.GetComponent<Animator>().Play("RedBall_Touch");
            }

            //CanLong = true;
            if(NowCatMorph != CatMorph.Climb)
            {
                CanJump = true;
            }
            GetHold = true;
            PlayerPrefs.SetString(collision.transform.parent.GetComponent<RedBall>().Number, "T");

        }
        if (collision.gameObject.tag == "NPC")
        {
            collision.GetComponent<GrandmaGhost>().TouchGrandma(GetComponent<CatContrl>());
        }
        if (collision.gameObject.tag == "DownPike")
        {
            collision.gameObject.GetComponent<TouchDownPike>().Down();
        }
    }
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Bar")
    //    {
    //        if (NowCatAct == CatAct.LongLongCat || NowCatAct == CatAct.LongDownCat)
    //        {

    //        }
    //        else
    //        {
    //            NowCatAct = CatAct.Jump;
    //        }
    //    }
    //}

    public Text Key_0_Text;
    public Text Key_0_Drag_Text;
    public Text Key_Text;

    public void Debug_Phone()
    {
        if(Key_0 == true)
        {
            Key_0_Text.color = Color.white;
        }
        else
        {
            Key_0_Text.color = Color.gray;
        }

        if (Key_0_Drag == true)
        {
            Key_0_Drag_Text.color = Color.white;
        }
        else
        {
            Key_0_Drag_Text.color = Color.gray;
        }

        Key_Text.text = "Key_" + WhatKey_;
    }
}
