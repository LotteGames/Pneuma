using System;
using TMPro;
using UnityEngine ;

[Serializable]
public class RoleBubble
{
    public Role role;

    public TMP_Text text;

    public RectTransform dialogueBubble;


    //public void SetCanvasOrder(int order) 
    //{
    //    canvas.sortingOrder = order;
    //}
    public void SetBubbleActive(bool value)
    {
        dialogueBubble.gameObject.SetActive(value);
    }
}
