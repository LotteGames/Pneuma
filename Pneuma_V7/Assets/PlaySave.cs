using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySave : MonoBehaviour
{
    [Header("裝花朵的物件")]
    public GameObject FlowerBox;
    [Header("儲存名稱")]
    public string FlowerBoxName;

    [Header("初始數值")]
    public bool IsOriginal;

    // Start is called before the first frame update
    void Start()
    {
        //for (int i = 0; i < FlowerBox.transform.childCount; i++)
        //{
        //    if (IsOriginal == true)
        //    {
        //        PlayerPrefs.SetString(FlowerBoxName + i, "F");
        //    }

        //    FlowerBox.transform.GetChild(i).GetComponent<RedBall>().Number = FlowerBoxName + i;
        //    if(PlayerPrefs.GetString(FlowerBoxName + i) != "T")
        //    {
        //        PlayerPrefs.SetString(FlowerBoxName + i, "F");
        //        Debug.Log(FlowerBoxName + i + "：F");
        //    }
        //    else
        //    {
        //        FlowerBox.transform.GetChild(i).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = FlowerBox.transform.GetChild(i).GetComponent<RedBall>().TouchPictrue;
        //    }
        //}
        
    }

  
}
