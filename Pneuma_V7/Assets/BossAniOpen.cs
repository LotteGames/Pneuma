using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAniOpen : MonoBehaviour
{
    [Header("誰碰到會觸發")]
    public GameObject WhoTouch;
    [Header("大蟲蟲")]
    public GameObject BigBug;
    [Header("提示動畫")]
    public GameObject Ani;



    // Start is called before the first frame update
    void Start()
    {
        Ani.SetActive(false);
        BigBug.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == WhoTouch)
        {
            Ani.SetActive(true);
            BigBug.SetActive(true);
            GetComponent<Animator>().enabled = true;
        }
    }

    public void Close()
    {
        Ani.SetActive(false);
        BigBug.SetActive(false);
        GetComponent<Animator>().enabled = false;

        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
}
