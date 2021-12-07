using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSystem : MonoBehaviour
{
    [Header("可以操控的角色")]
    public GameObject[] CanContrlPlayer;
    int nowPlayer;

    [Header("現在操控的角色")]
    public GameObject NowContrlPlayer;
    [Header("攝影機跟隨的位置")]
    public GameObject CameraFallowPos;

    [Header("上下左右按鍵")]
    public KeyCode UpKey;
    public KeyCode DownKey;
    public KeyCode LeftKey;
    public KeyCode RightKey;

    [Header("技能按鍵(可自由加減)")]
    public KeyCode[] Skill_;

    // Start is called before the first frame update
    void Start()
    {
        nowPlayer = 0;
        NowContrlPlayer = CanContrlPlayer[nowPlayer];
        NowContrlPlayer.GetComponent<PlayerContrl>().InitEvent.Invoke();
    }

    public void FixedUpdate()
    {

    }

    // Update is called once per frame
    void Update()
    {
        NowContrlPlayer.GetComponent<PlayerContrl>().MainEvent.Invoke();

        //if (Input.GetKeyDown(KeyCode.Tab))
        //{
        //    nowPlayer++;
        //    if(nowPlayer >= CanContrlPlayer.Length)
        //    {
        //        nowPlayer = 0;
        //    }
        //    NowContrlPlayer = CanContrlPlayer[nowPlayer];
        //    //if(NowContrlPlayer.GetComponent<GrandmaContrl>() == true)
        //    //{
        //    //    NowContrlPlayer.GetComponent<GrandmaContrl>().LookNowContrl();
        //    //}
        //}
        //CameraFallowPos.transform.position = NowContrlPlayer.transform.position;
    }
}
