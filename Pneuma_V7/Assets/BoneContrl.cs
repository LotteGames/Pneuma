using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneContrl : MonoBehaviour
{
    [Header("要生成的粒子特效")]
    public GameObject[] vingAni;
    [Header("幾秒後刪除粒子特效")]
    public float DeleteTime;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "CatBody")
        {
            GetComponent<Collider2D>().isTrigger = true;
            for (int i = 0; i < vingAni.Length; i++)
            {
                GameObject Ani = Instantiate(vingAni[i], transform.position, Quaternion.Euler(0, 0, 0),transform.parent);
                Ani.transform.localScale = transform.parent.transform.parent.transform.localScale;
                Ani.GetComponent<ParticleSystem>().startSize = transform.parent.transform.parent.transform.localScale.y;
                Destroy(Ani, DeleteTime);
            }
            StartCoroutine(Close());
        }
    }

    public IEnumerator Close()
    {
        GetComponent<Collider2D>().isTrigger = true;
        yield return new WaitForSeconds(0.5f);
        GetComponent<Collider2D>().isTrigger = false;
    }
}
