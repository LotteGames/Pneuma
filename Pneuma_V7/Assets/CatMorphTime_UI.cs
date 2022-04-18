using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatMorphTime_UI : MonoBehaviour
{
    [Header("最大值")]
    public float MorphTimeMax;
    [Header("目前數值")]
    public float MorphTime;
    [Header("1~0")]
    public float OneCount;
    [Header("Red")]
    public Color Red;
    [Header("Blue")]
    public Color Blue;
    [Header("圖片")]
    public Image TimeForAmount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MorphTime = GameObject.FindObjectOfType<CatContrl>().MorphTime;
        OneCount = MorphTime / MorphTimeMax;

        TimeForAmount.fillAmount = OneCount;
        TimeForAmount.GetComponent<Image>().color = Color.Lerp(Red, Blue, OneCount);
        GetComponent<Image>().color = Color.Lerp(Red, Blue, OneCount);

        if (MorphTime <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
