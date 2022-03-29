using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAni_LevelTrigger : MonoBehaviour
{
    [Header("開始運作的動畫")]
    public GameObject[] BossAni;
    [Header("開始運作的動畫")]
    public int Level;

    [Header("是否關閉運作?")]
    public bool IsClose;

    [Header("貓咪是否關閉運作?")]
    public bool IsCatStop;
    [Header("貓咪關閉運作多久")]
    public float StopTime;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CatContrl>() != null)
        {
            for (int i = 0; i < BossAni.Length; i++)
            {
                if(i == Level)
                {
                    BossAni[i].SetActive(true);
                    BossAni[i].GetComponent<Animator>().enabled = true;
                }
                else
                {
                    BossAni[i].SetActive(false);
                    BossAni[i].GetComponent<Animator>().enabled = false;
                }
            }
            GameObject.FindObjectOfType<CatContrl>().AllStart();


            if (IsClose == true)
            {
                BossAni_LevelTrigger[] elseAni;
                elseAni = GameObject.FindObjectsOfType<BossAni_LevelTrigger>();
                for (int i = 0; i < elseAni.Length; i++)
                {
                    elseAni[i].GetComponent<Collider2D>().enabled = true;
                }

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
            for (int i = 0; i < BossAni.Length; i++)
            {
                if (i == Level)
                {
                    BossAni[i].SetActive(true);
                    BossAni[i].GetComponent<Animator>().enabled = true;
                }
                else
                {
                    BossAni[i].SetActive(false);
                    //BossAni[i].GetComponent<Animator>().enabled = false;
                }
            }
            GameObject.FindObjectOfType<CatContrl>().AllStart();

            if (IsClose == true)
            {
                BossAni_LevelTrigger[] elseAni;
                elseAni = GameObject.FindObjectsOfType<BossAni_LevelTrigger>();
                for (int i = 0; i < elseAni.Length; i++)
                {
                    elseAni[i].GetComponent<Collider2D>().enabled = true;
                }

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
