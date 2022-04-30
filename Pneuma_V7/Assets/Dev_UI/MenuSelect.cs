using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

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
