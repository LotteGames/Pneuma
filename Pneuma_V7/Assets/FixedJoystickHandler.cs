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

    public bool Right;
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
        if(Right == false)
        {
            Left_Touch(eventData);
        }
        Debug.Log("direction : " + direction);
    }

    //public Vector2 OldDirection;//最大值42.6
    public Vector2 direction;
    public float OneOf_direction;
    public float Remove;//最大值42.62199
    public void Getcontect(PointerEventData eventData)
    {
        Vector2 position = Vector2.zero;
        Vector2 position_2 = content.GetComponent<RectTransform>().anchoredPosition;

        //// Get InputDirection
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(
        //    jsContainer.rectTransform,
        //    eventData.position,
        //    eventData.pressEventCamera,
        //    out position
        //);

        //float x = (position.x / jsContainer.rectTransform.sizeDelta.x);
        //float y = (position.y / jsContainer.rectTransform.sizeDelta.y);

        //InputDirection = new Vector3(x, y, 0);
        //InputDirection = (InputDirection.magnitude > 1) ? InputDirection.normalized : InputDirection;

        //// Define the area in which joystick can move around
        //joystick.rectTransform.anchoredPosition = new Vector3(
        //    InputDirection.x * jsContainer.rectTransform.sizeDelta.x / 3,
        //    InputDirection.y * jsContainer.rectTransform.sizeDelta.y / 3
        //);

        //OldDirection = position;
        direction = position_2;
        Remove = Vector2.Distance(position_2, Vector2.zero);
        OneOf_direction = Remove / 42.5f;
    }

    public void Left_Touch(PointerEventData eventData)
    {
        Vector2 position = Vector2.zero;
        Vector2 position_2 = content.GetComponent<RectTransform>().anchoredPosition;


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

        direction = position_2;

        //

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0)
            angle = 360 + angle;

        Debug.Log(angle);

        if (angle > 30 && angle < 120)
        {
            Cat.GetStart("W");
            Debug.Log("W");
        }
        if (angle >= 120 && angle <= 240)
        {
            Cat.GetStart("A");
            Debug.Log("A");
        }
        if (angle > 240 && angle < 300)
        {
            Cat.GetStart("S");
            Debug.Log("S");
        }
        if (angle >= 300 || angle <= 30)
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
