using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBox : MonoBehaviour
{
    [Header("彈跳力道乘以多少")]
    public float PowerAdd;

    public GameObject Cat;

    private void Start()
    {
        Cat = GameObject.FindObjectOfType<CatContrl>().gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<CatContrl>() != null)
        {
            GetComponent<Animator>().Play("Base Layer.JumpBox");

            //GetComponent<Collider2D>().enabled
            //
            Cat.GetComponent<CatContrl>().CatMusic.PlayMusic(0);
            Cat.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
            Cat.GetComponent<Rigidbody2D>().AddForce(new Vector3(0, collision.GetComponent<CatContrl>().JumpPower * PowerAdd, 0));
            Cat.GetComponent<CatContrl>().NowCatAct = CatContrl.CatAct.Jump;
            Cat.GetComponent<CatContrl>().CanJump = false;//不能繼續跳
            Cat.GetComponent<CatContrl>().CanLong = true;//可以延長
            StartCoroutine(Cat.GetComponent<CatContrl>().JumpDebug(0.3f));
        }
    }
}
