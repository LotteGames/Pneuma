using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorToKey : MonoBehaviour
{
    [Header("影響門的按鈕")]
    public GameObject[] Keys;
    [Header("有幾個按鈕被按下了")]
    public int KeyDown;
    [Header("一層一層開啟的門(需要多個按鈕)")]
    public bool LevelOpen;

    [Header("初始門的位置")]
    public Vector2 StartPos;
    public Quaternion StartRot;

    [Header("開門後的位置")]
    public Vector2 OpenPos;
    public Quaternion OpenRot;

    [Header("開門後的位置")]
    public Vector2 NowPos;
    public Quaternion NowRot;

    [Header("開啟速度")]
    public float OpenSpeed;
    // Start is called before the first frame update
    void Start()
    {
        StartPos = transform.localPosition;
        StartRot = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        MyKeyDown();
    }

    public void MyKeyDown()
    {
        if(LevelOpen == true)
        {
            IsOpen();
            NowPos = Vector2.Lerp(StartPos, OpenPos, (float)KeyDown / (float)Keys.Length);
            NowRot = Quaternion.Lerp(StartRot, OpenRot, (float)KeyDown / (float)Keys.Length);
        }
        else
        {
            if (IsOpen() == true)
            {
                NowPos = OpenPos;
                NowRot = OpenRot;
            }
            else
            {
                NowPos = StartPos;
                NowRot = StartRot;
            }
        }
        transform.localPosition = Vector2.Lerp(transform.localPosition, NowPos, OpenSpeed);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, NowRot, OpenSpeed);
    }

    public bool IsOpen()
    {
        bool Retun = false;
        KeyDown = 0;
        for (int i = 0; i < Keys.Length; i++)
        {
            if (Keys[i].GetComponent<KeyToDoor>().IsTouch == false)
            {
                Retun = false;
            }
            else
            {
                KeyDown++;
            }
        }
        if(KeyDown == Keys.Length)
        {
            Retun = true;
        }

        return Retun;
    }
}
