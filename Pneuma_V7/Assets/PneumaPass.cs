using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PneumaPass : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CatContrl>())
        {
            triggerEvent.Invoke();
        }
    }

    public UnityEvent triggerEvent;
}
