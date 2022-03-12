using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRock : MonoBehaviour
{
    [Header("這個石像的碎片*2")]
    public GameObject[] myRock;
    [Header("是否獲得這些碎片了?")]
    public bool[] IsGet;
    [Header("這個石像門")]
    public GameObject myDoor;
    // Start is called before the first frame update
    void Start()
    {
        
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

        if(IsGet[0] == true && IsGet[1] == true)
        {
            myDoor.SetActive(false);
        }
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
