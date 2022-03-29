using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAni_Trigger : MonoBehaviour
{
    [Header("開始運作的動畫")]
    public GameObject BossAni;

    [Header("是否關閉運作?")]
    public bool IsClose;

    [Header("貓咪是否關閉運作?")]
    public bool IsCatStop;
    [Header("貓咪關閉運作多久")]
    public float StopTime;

    [Header("其他動畫")]
    public GameObject elseBossAni;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<CatContrl>() != null)
        {
            BossAni.SetActive(true);
            BossAni.GetComponent<Animator>().enabled = true;
            if (IsClose == true)
            {
                GetComponent<Collider2D>().enabled = false;
            }
            if (IsCatStop == true)
            {
                collision.GetComponent<CatContrl>().StopCat(StopTime);
                collision.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<CatContrl>() != null)
        {
            BossAni.SetActive(true);
            BossAni.GetComponent<Animator>().enabled = true;
            if (IsClose == true)
            {
                GetComponent<Collider2D>().enabled = false;
            }
            if (IsCatStop == true)
            {
                collision.gameObject.GetComponent<CatContrl>().StopCat(StopTime);
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }
        }
    }
}
