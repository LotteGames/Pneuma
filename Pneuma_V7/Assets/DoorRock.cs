using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRock : MonoBehaviour
{
    [Header("這個石像的碎片*2")]
    public GameObject[] myRock;
    [Header("石像的初始位置")]
    public Vector2[] myRockPos;
    [Header("是否獲得這些碎片了?")]
    public bool[] IsGet;
    [Header("初始紀錄")]
    public bool[] IsGetStart;
    [Header("這個石像門")]
    public GameObject myDoor;

    [Header("門開啟的特效")]
    public GameObject OpenAni;
    [Header("門開啟的特效_光球")]
    public GameObject OpenAni_LightBall;

    [Header("金幣儲存了")]
    public bool Save;

 

    public void PlayerSave()
    {
        if (myDoor.active == false)
        {
            Save = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        OpenAni_LightBall.transform.position = transform.position;
        myDoor.GetComponent<Animator>().enabled = false;

        for (int i = 0; i < IsGet.Length; i++)
        {
            myRockPos[i] = myRock[i].transform.position;
            IsGetStart = IsGet;
        }
    }

    public void SetStart()
    {
        if (Save == false)
        {
            for (int i = 0; i < IsGet.Length; i++)
            {
                IsGet[i] = IsGetStart[i];
                myRock[i].transform.position = myRockPos[i];
            }
            OpenAni_LightBall.transform.position = transform.position;
            myDoor.GetComponent<Animator>().enabled = false;
            myDoor.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < IsGet.Length; i++)
        {
            if (IsGet[i] == true)
            {
                myRock[i].transform.position = Vector3.Lerp(myRock[i].transform.position, transform.position, 0.1f);
            }
        }

        if(IsGet[0] == true && IsGet[1] == true && myDoor.active == true)
        {
            OpenAni_LightBall.SetActive(true);
            myDoor.GetComponent<Animator>().enabled = true;
            StartCoroutine(DelayOpen());
        }
    }

    public IEnumerator DelayOpen()
    {
        yield return new WaitForSeconds(1.2f);
        myDoor.SetActive(false);
        OpenAni_LightBall.SetActive(false);
        OpenAni.SetActive(true);
    }

    public void WhatKey(GameObject key)
    {
        var index = Array.FindIndex(myRock, x => x == key);
        if (index > -1)
        {
            IsGet[index] = true;
            key.GetComponent<Rigidbody2D>().gravityScale = 0;
            key.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            key.GetComponent<Collider2D>().enabled = false;
            key.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            Console.WriteLine("Value not found");
        }
    }
}
