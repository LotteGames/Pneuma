using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
public class MenuSelect : MonoBehaviour
{
    public int index = 0;

    public EventSystem eventSystem;
    GameObject lastObj;

    public RectTransform mark;

    public Ease ease;
    public float duration = 0.5f;

    public GameObject firstSelected;

    private void Start()
    {
        eventSystem.SetSelectedGameObject(firstSelected);
    }

    private void Update()
    {
        GameObject obj_1 = (eventSystem.currentSelectedGameObject);

        if (obj_1 != lastObj)
        {
            if (obj_1 != null)
            {
                TextMeshProUGUI text = obj_1.GetComponentInChildren<TextMeshProUGUI>();

                mark.DOMove(obj_1.GetComponent<RectTransform>().position, duration).SetEase(ease);


                coroutine_obj = StartCoroutine(MoveObjText(text));

                if (lastObj != null)
                {
                    coroutine_last = StartCoroutine(MoveLastText(lastObj.GetComponentInChildren<TextMeshProUGUI>()));
                }

                lastObj = obj_1;
            }
        }
    }



    Coroutine coroutine_obj, coroutine_last;

    IEnumerator MoveObjText(TextMeshProUGUI text)
    {
        float duration = this.duration, time = 0;

        string originText = text.text;

        while (time < duration)
        {
            time += Time.deltaTime;
            text.SetText("<space=" + time + "em>" + originText);
            //text.SetText( "< space = "+time+" em >"+ originText);
            yield return new WaitForEndOfFrame();
        }

    }
    IEnumerator MoveLastText(TextMeshProUGUI text)
    {
        float duration = this.duration, time = 0;

        string originText = text.text;
        string[] split = originText.Split('>');
        string targetText = split[split.Length - 1];

        while (time < duration)
        {
            duration -= Time.deltaTime;
            // <space=5em> 
            text.SetText("<space=" + ((duration < 0) ? 0 : duration) + "em>" + targetText);
            //text.SetText("< space = " + duration + " em >" + originText);
            yield return new WaitForEndOfFrame();
        }
        text.SetText(targetText);
    }

}
