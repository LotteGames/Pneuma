using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class ResultTitle : MonoBehaviour
{
    public RectTransform rectTransform_1;

    public float duration_1 = 1;

    public void ShowTitle_1()
    {
        rectTransform_1.DOSizeDelta(new Vector2(700, 100), duration_1);
    }

    public GameObject btn;

    public void SetBtnActive(bool value) 
    {
        btn.SetActive(value);
    }
}
