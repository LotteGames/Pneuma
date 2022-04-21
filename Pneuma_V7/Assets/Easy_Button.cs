using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Easy_Button : MonoBehaviour
{
    [Header("難易度影響的物件")]
    public GameObject[] Easy_Object;
    [Header("現在的難易度")]
    public Text Now_IsEasy;

    // Start is called before the first frame update
    void Start()
    {
        EasyButton_OnClick();
    }


    public void EasyButton_OnClick()
    {
        if(Now_IsEasy.text == "目前：簡單")
        {
            Now_IsEasy.text = "目前：困難";
            for (int i = 0; i < Easy_Object.Length; i++)
            {
                Easy_Object[i].SetActive(true);
            }
        }
        else
        {
            Now_IsEasy.text = "目前：簡單";
            for (int i = 0; i < Easy_Object.Length; i++)
            {
                Easy_Object[i].SetActive(false);
            }
        }
    }
}
