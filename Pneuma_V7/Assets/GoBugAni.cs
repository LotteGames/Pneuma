using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBugAni : MonoBehaviour
{
    [Header("貓咪")]
    public CatContrl Cat;
    [Header("大蟲蟲")]
    public GameObject BigBug;
    [Header("吼叫聲")]
    public GameObject Sonic;

    [Header("破壞的平台")]
    public GameObject BreakGround;

    // Start is called before the first frame update
    void Start()
    {
        Cat = GameObject.FindObjectOfType<CatContrl>();
        Sonic.SetActive(false);
        BigBug.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<CatContrl>() != null)
        {
            Cat.StopCat(8);
            Cat.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            Sonic.SetActive(true);
            BigBug.SetActive(true);
            GetComponent<Animator>().enabled = true;
        }
    }

    public void Close()
    {
        Destroy(BreakGround);
        Destroy(gameObject);
    }
}
