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

    [Header("動畫初始角度")]
    public Vector3 AniRot;
    [Header("動畫初始位置")]
    public Vector3 StartPos;
    // Start is called before the first frame update
    void Start()
    {
        StartPos = BigBug.transform.position;
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
        BigBug.transform.rotation = Quaternion.Euler(AniRot);
        BigBug.transform.position = StartPos;
        BigBug.SetActive(true);

        GetComponent<Animator>().enabled = false;
        BigBug.SetActive(false);
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
}
