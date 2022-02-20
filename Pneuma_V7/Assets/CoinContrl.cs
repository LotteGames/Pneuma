using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinContrl : MonoBehaviour
{
    [Header("吃到的特效")]
    public GameObject CoinAni;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "CatBody")
        {
            GameObject Ani = Instantiate(CoinAni, transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(Ani, 5);
            gameObject.SetActive(false);
        }
    }
}
