using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBreak : MonoBehaviour
{
    [Header("破壞時的粒子特效")]
    public GameObject BreakAni;
    [Header("牙齒陷阱")]
    public GameObject PikeTooth;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Boss_Breaking>() != null)
        {
            Instantiate(BreakAni, PikeTooth.transform.position, Quaternion.Euler(-90, 0, 0));
            collision.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Boss_Breaking>() != null)
        {
            Instantiate(BreakAni, PikeTooth.transform.position, Quaternion.Euler(-90, 0, 0));
            collision.gameObject.SetActive(false);
        }
    }
}
