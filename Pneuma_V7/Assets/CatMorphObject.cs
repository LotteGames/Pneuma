using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMorphObject : MonoBehaviour
{
    [Header("哪種變形的道具")]
    public CatContrl.CatMorph WhatMorph;

    [Header("持續時間")]
    public float CanGetTime;

    [Header("多久後再次生成")]
    public float CreateAgain;
    // Start is called before the first frame update
    void Start()
    {
        
    }


}
