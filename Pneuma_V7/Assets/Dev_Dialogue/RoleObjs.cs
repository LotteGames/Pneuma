using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
/// <summary>
/// For every character that need to speak, include cat
/// </summary>
public class RoleObjs : MonoBehaviour
{
    public Cinemachine.CinemachineTargetGroup.Target target;

    public Role role;

    public Vector3 Position 
    {
        get { return transform.position; }
    }

    public GameObject bubble;

    public void SetBubble(bool isActive)
    {
        bubble.SetActive(isActive);
    }

    public void SetText(string text) 
    {
        Text.text = text;
    }

    public void BubbleScaleUp(string text) 
    {
        Text.text = text;
        SetBubbleSize();
        bubble.GetComponent<RectTransform>().DOScale(Vector3.one, 0.2f);
    }
    public void BubbleScaleDown()
    {
        bubble.GetComponent<RectTransform>().DOScale(Vector3.zero, 0.2f);
    }


    public void SetBubbleSize() 
    {
        Text.ForceMeshUpdate();

        Vector2 textSize = Text.GetRenderedValues(false);

        Vector2 padding = new Vector2(2, 3.5f);

        if (textSize.x < textSize.y)
        {
            textSize = new Vector2(textSize.y, textSize.x);
        }

        bubble.GetComponent<RectTransform>().sizeDelta = textSize + padding;
    }

    public TMP_Text Text;


    public GameObject continueBtn;
    public void SetContinueBtn(bool isActive) 
    {
        continueBtn.SetActive(isActive);
    }
}
