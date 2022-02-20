using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class FixedJoystickHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [System.Serializable]
    public class VirtualJoystickEvent : UnityEvent<Vector3> { }

    public CatContrl Cat;

    public Transform content;
    public UnityEvent beginControl;
    public VirtualJoystickEvent controlling;
    public UnityEvent endControl;

    public Image jsContainer;
    public Image joystick;
    public Vector3 InputDirection = Vector3.zero;
    public void Start()
    {
        // Get the Component we attach this script (JoystickContainer)
        jsContainer = GetComponent<Image>();

        // Get the only one child component (Joystick)
        joystick = transform.GetChild(0).GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)//「開始控制」
    {
        this.beginControl.Invoke();
    }
    public void OnBeginDrag()//「開始控制」
    {
        this.beginControl.Invoke();
    }

    public void OnDrag(PointerEventData eventData)//「控制中」
    {
        if (this.content)
        {
            this.controlling.Invoke(this.content.localPosition.normalized);
        }

        Getcontect(eventData);
        Left_Touch(direction);
        Debug.Log("direction : " + direction);
    }

    public Vector2 direction;
    public void Getcontect(PointerEventData eventData)
    {
        Vector2 position = Vector2.zero;

        // Get InputDirection
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            jsContainer.rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out position
        );

        float x = (position.x / jsContainer.rectTransform.sizeDelta.x);
        float y = (position.y / jsContainer.rectTransform.sizeDelta.y);

        InputDirection = new Vector3(x, y, 0);
        InputDirection = (InputDirection.magnitude > 1) ? InputDirection.normalized : InputDirection;

        // Define the area in which joystick can move around
        joystick.rectTransform.anchoredPosition = new Vector3(
            InputDirection.x * jsContainer.rectTransform.sizeDelta.x / 3,
            InputDirection.y * jsContainer.rectTransform.sizeDelta.y / 3
        );

        direction = InputDirection;
    }

    public void Left_Touch(Vector2 Rot)
    {
        float angle = Mathf.Atan2(Rot.y, Rot.x) * Mathf.Rad2Deg;
        if (angle < 0)
            angle = 360 + angle;

        Debug.Log(angle);

        if (angle >= 45 && angle <= 135)
        {
            Cat.GetStart("W");
            Debug.Log("W");
        }
        if (angle > 135 && angle < 225)
        {
            Cat.GetStart("A");
            Debug.Log("A");
        }
        if (angle >= 225 && angle <= 315)
        {
            Cat.GetStart("S");
            Debug.Log("S");
        }
        if (angle > 315 || angle < 45)
        {
            Cat.GetStart("D");
            Debug.Log("D");
        }
    }

    //public void OnDrag()//「控制中」
    //{
    //    if (this.content)
    //    {
    //        this.controlling.Invoke(this.content.localPosition.normalized);
    //    }
    //}

    public void OnEndDrag(PointerEventData eventData)//「控制結束」
    {
        this.endControl.Invoke();
    }

    //public void OnEndDrag()//「控制結束」
    //{
    //    this.endControl.Invoke();
    //}
}
