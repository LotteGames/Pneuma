using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockKey : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "CatBody")
        {
            transform.parent.GetComponent<DoorRock>().WhatKey(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "CatBody")
        {
            transform.parent.GetComponent<DoorRock>().WhatKey(gameObject);
        }
    }
}
