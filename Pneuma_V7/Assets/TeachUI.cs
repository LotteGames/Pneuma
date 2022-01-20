using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeachUI : MonoBehaviour
{
    [Header("哪一個按鍵")]
    public KeyCode WhatKey;

    [Header("可以偵測有按鍵了嗎")]
    public bool CanGetKey;
    // Start is called before the first frame update
    void Start()
    {
        CanGetKey = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(WhatKey) && CanGetKey == true)
        {
            this.gameObject.GetComponent<Animator>().Play("Base Layer.TeachUI");
            this.transform.parent.GetComponent<TeachUI_Box>().GetKey();
            CanGetKey = false;
            Destroy(gameObject, 2);
        }
    }

    public void StartAni()
    {
        CanGetKey = true;
    }



}
