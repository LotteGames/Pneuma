using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakGround : MonoBehaviour
{
    [Header("父物件")]
    public BreakGroundBox Box;

    [Header("要生成的石頭特效")]
    public GameObject RockAni;

    [Header("準備掉落~")]
    public bool Down = false;

    [Header("踩到後幾秒掉落")]
    public float DownDelayTime;


    public IEnumerator DownDelay()
    {
        yield return new WaitForSeconds(DownDelayTime);
        GetComponent<Collider2D>().isTrigger = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Rigidbody2D>().gravityScale = 3;
    }
    public IEnumerator OpenAct(float Delay)
    {
        yield return new WaitForSeconds(Delay);
        gameObject.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<CatContrl>() != null)
        {
            if (Down == false && collision.transform.position.y >= transform.position.y)
            {
                Down = true;
                StartCoroutine(DownDelay());
            }
        }

        //if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Wall")
        //{
        //    if (Down == true)
        //    {
        //        Box.InsNew();
        //        GameObject Ani = Instantiate(RockAni, transform.position, Quaternion.Euler(-90, 0, 0));
        //        Destroy(Ani, 3);
        //        Destroy(gameObject);
        //    }
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Wall")
        {
            if (Down == true)
            {
                Down = false;
                Box.InsNew();
                GameObject Ani = Instantiate(RockAni, transform.position, Quaternion.Euler(-90, 0, 0));
                Destroy(Ani, 3);
                Destroy(gameObject);
            }
        }
    }
}
