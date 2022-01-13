using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMorphObject : MonoBehaviour
{
    [Header("哪種變形的道具")]
    public CatContrl.CatMorph WhatMorph;

    [Header("持續時間")]
    public float CanGetTime;

    [Header("多久後再次生成")]
    public float CreateAgain;

    [Header("提示圖")]
    public Sprite[] Hints;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "CatBody")
        {
            if (collision.GetComponent<CatContrl>() != null)
            {
                collision.GetComponent<CatContrl>().NowCatMorph = WhatMorph;
                collision.GetComponent<CatContrl>().MorphTime = CanGetTime;
                collision.GetComponent<Animator>().SetBool("Cloud", false);
                if (collision.GetComponent<CatContrl>().NowCatMorph == CatContrl.CatMorph.Climb)
                {
                    collision.GetComponent<Animator>().SetBool("Climb", true);
                    collision.GetComponent<SpriteRenderer>().sortingOrder = 0;
                    collision.GetComponent<CircleCollider2D>().radius = 0.7f;
                }
                else
                {
                    collision.GetComponent<Animator>().SetBool("Climb", false);
                    collision.GetComponent<SpriteRenderer>().sortingOrder = 500;
                    collision.GetComponent<CircleCollider2D>().radius = 0.81f;
                }
                StartCoroutine(Close());
            }
        }
    }

    public IEnumerator Close()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(CreateAgain);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        GetComponent<Collider2D>().enabled = true;
    }
}
