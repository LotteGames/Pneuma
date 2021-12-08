using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class DoorOpenEvent : MonoBehaviour
{
    public KeyToDoor keyToDoor;

    bool isInvoke = false;

    public UnityEvent unityEvent;

    private void Update()
    {
        if (keyToDoor.IsTouch && !isInvoke)
        {
            isInvoke = true;

            unityEvent.Invoke();
        }
    }
}
