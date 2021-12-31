using System;
using TMPro;
using UnityEngine ;

[Serializable]
public class RoleBubble
{
    public Role role;

    public TMP_Text text;

    public RectTransform dialogueBubble;

    public GameObject continueBtn;

    public void SetBubbleActive(bool value)
    {
        dialogueBubble.gameObject.SetActive(value);
    }
    public void SetContinueActive(bool value)
    {
        continueBtn.SetActive(value);
    }
}
