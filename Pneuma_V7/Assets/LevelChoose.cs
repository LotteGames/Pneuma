using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelChoose : MonoBehaviour
{
    [Header("關卡的按鈕")]
    public GameObject[] LevelButton;

    [Header("關卡的按鈕的位置")]
    public Vector3[] LevelButtonPos;

    [Header("關卡的按鈕的大小")]
    public Vector3[] LevelButtonSize;

    //[Header("關卡的按鈕紀錄動畫")]
    //public Animator[] Ani;

    [Header("哪一個按鈕")]
    public int[] ButtonToWhereScense;

    [Header("哪一個按鈕")]
    public int WhatButton;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < LevelButtonPos.Length; i++)
        {
            LevelButtonPos[i] = LevelButton[i].GetComponent<RectTransform>().position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < LevelButtonPos.Length; i++)
        {
            int x = i - WhatButton;
            if(x < 0)
            {
                x += LevelButtonPos.Length;
            }
            LevelButton[x].GetComponent<RectTransform>().position = Vector3.Lerp(LevelButton[x].GetComponent<RectTransform>().position, LevelButtonPos[i], 0.08f);
            LevelButton[x].GetComponent<RectTransform>().localScale = Vector3.Lerp(LevelButton[x].GetComponent<RectTransform>().localScale, LevelButtonSize[i], 0.08f); ;
        }
    }

    public void GoToScense(int whatButton)
    {
        if(WhatButton == whatButton)
        {
            SceneManager.LoadScene(ButtonToWhereScense[WhatButton]);
        }
        else
        {
            WhatButton = whatButton;
        }
    }

}
