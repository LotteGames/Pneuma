using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
/// <summary>
/// 只呼叫物件去做事
/// 
/// 滑鼠懸浮在某個物件上時，會有文字的效果跟標記移動的效果
/// 還會告訴物件滑鼠/鍵盤選在某個物件上，以及離開某個物件了
/// 
/// 鍵盤操作的話，則是鍵盤按了才會有文字效果，並且標記只會停留在當前選擇的物件上
/// </summary>
public class MenuSelect : MonoBehaviour
{
    public GameObject firstSelected, lastSelected;

    public GraphicRaycaster graphicRaycaster;
    public EventSystem eventSystem;

    public Transform mark;
    public Ease ease;
    public float markMoveSec;

    public TextEffect MouseHoverOn()
    {
        PointerEventData pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();

        graphicRaycaster.Raycast(pointerEventData, results);

        for (int i = 0; i < results.Count; i++)
        {
            TextEffect hoverOn = results[i].gameObject.GetComponentInChildren<TextEffect>();

            if (hoverOn != null)
            {
                return hoverOn;
            }
        }
        return null;
    }
    public TextEffect lastTextEffect;

    private void Start()
    {
        SetSelectedObj(firstSelected);
        lastSelected = firstSelected;

        lastTextEffect = firstSelected.GetComponentInChildren<TextEffect>();
        lastTextEffect.IsHoverOn = true;
    }

    private void Update()
    {
        if (eventSystem.currentSelectedGameObject != null)
        {
            Debug.LogError(eventSystem.currentSelectedGameObject.name);
        }
        else
        {
            eventSystem.SetSelectedGameObject(lastSelected);
        }


        TextEffect textEffect = MouseHoverOn();
        if (textEffect != null && lastTextEffect != textEffect)
        {
            textEffect.IsHoverOn = true;

            lastTextEffect.IsHoverOn = false;

            lastTextEffect = textEffect;


            MoveMark(textEffect);


            GameObject buttonObj = textEffect.GetComponentInParent<Button>().gameObject;
            SetSelectedObj(buttonObj);
            lastSelected = buttonObj;
        }


        GameObject currentSelectedObj = eventSystem.currentSelectedGameObject;

        if (lastSelected != currentSelectedObj && currentSelectedObj != null)
        {

            TextEffect effect = currentSelectedObj.GetComponentInChildren<TextEffect>();

            if (effect != null)
            {
                effect.IsHoverOn = true;

                lastTextEffect.IsHoverOn = false;

                lastTextEffect = effect;


                MoveMark(effect);

            }

            lastSelected = effect.GetComponentInParent<Button>().gameObject;
        }
    }

    public void MoveMark(TextEffect textEffect)
    {
        mark.DOMoveY(textEffect.transform.position.y, markMoveSec).SetEase(ease);
    }
    public void SetSelectedObj(GameObject selectedObj)
    {
        eventSystem.SetSelectedGameObject(selectedObj);
    }
}
