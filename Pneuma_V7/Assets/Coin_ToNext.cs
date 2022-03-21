using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Coin_ToNext : MonoBehaviour
{
    [Header("吃到的特效")]
    public GameObject CoinAni;

    [Header("下個位置")]
    public GameObject NextPos;

    [Header("貓咪")]
    public GameObject Cat;

    [Header("操控系統")]
    public GameObject System;
    // Start is called before the first frame update
    void Start()
    {
        Cat = GameObject.FindObjectOfType<CatContrl>().gameObject;
        System = GameObject.FindObjectOfType<Teaching_System>().gameObject;
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "CatBody")
        {
            if(NextPos != null)
            {
                Cat.transform.position = NextPos.transform.position;

                if (Cat.GetComponent<CatContrl>().nowLongBody != null)
                {
                    Cat.GetComponent<Collider2D>().isTrigger = false;
                    Cat.GetComponent<Animator>().SetBool("Long", false);
                    Cat.GetComponent<Rigidbody2D>().gravityScale = 6.5f;//貓咪屁股的重力恢復
                    Destroy(Cat.GetComponent<CatContrl>().nowLongBody);
                }

            }
            else
            {
                SceneManager.LoadScene(3);
            }
            System.GetComponent<Teaching_System>().NextLevel();


            GameObject Ani = Instantiate(CoinAni, transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(Ani, 5);
            Destroy(gameObject);
        }
    }
}
