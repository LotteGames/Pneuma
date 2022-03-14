using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenKey : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<CatContrl>() != null)
        {
            transform.parent.gameObject.GetComponent<ElevatorContrl>().ChangeSpeed(10);
            transform.parent.gameObject.GetComponent<ElevatorContrl>().GetStart();
           gameObject.SetActive(false);
        }
    }
}
