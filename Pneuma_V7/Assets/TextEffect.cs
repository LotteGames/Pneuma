using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
/// <summary>
/// 物件自己根據指令做效果
/// </summary>
public class TextEffect : MonoBehaviour
{
    public float duration = 0.5f;
    [SerializeField]
    private float currentDuration = 0;

    TextMeshProUGUI textMesh;
    public string originText;
    public bool IsHoverOn
    {
        get
        {
            return isHoverOn;
        }
        set
        {
            isHoverOn = value;

            if (value)
            {
                StartAdd();
                eventHoverOn.Invoke();
            }
            else
            {
                StartDecrease();
                eventHoverOff.Invoke();
            }
        }
    }
    private bool isHoverOn = false;

    public void StartAdd()
    {
        if (coroutine_AddSpace != null)
        {
            StopCoroutine(coroutine_AddSpace);
        }
        if (coroutine_DecreaseSpace != null)
        {
            StopCoroutine(coroutine_DecreaseSpace);
        }
        coroutine_AddSpace = StartCoroutine(AddSpace());
    }

    public void StartDecrease()
    {
        if (coroutine_AddSpace != null)
        {
            StopCoroutine(coroutine_AddSpace);
        }
        if (coroutine_DecreaseSpace != null)
        {
            StopCoroutine(coroutine_DecreaseSpace);
        }
        coroutine_DecreaseSpace = StartCoroutine(DecreaseSpace());
    }


    Coroutine coroutine_AddSpace, coroutine_DecreaseSpace;

    IEnumerator AddSpace()
    {
        while (currentDuration < duration)
        {
            currentDuration += .01f;
            currentDuration = (currentDuration >= 0.5f) ? 0.5f : currentDuration;

            string space = "<space=" + currentDuration + "em>";

            textMesh.SetText(space + originText);
            yield return new WaitForSeconds(.01f);
        }
    }

    IEnumerator DecreaseSpace()
    {
        while (0 < currentDuration)
        {
            currentDuration -= .01f;
            currentDuration = (currentDuration <= 0.05f) ? 0f : currentDuration;
            string space = "<space=" + currentDuration + "em>";

            textMesh.SetText(space + originText);
            yield return new WaitForSeconds(.01f);
        }
    }

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        originText = textMesh.text;
    }

    public UnityEvent eventHoverOn, eventHoverOff;
}
