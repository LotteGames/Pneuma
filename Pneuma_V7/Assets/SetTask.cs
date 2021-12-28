using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class SetTask : MonoBehaviour
{
    public int task;

    public UnityEvent unityEvent;

    public void InvokeEvent()
    {
        unityEvent.Invoke();
    }
}
