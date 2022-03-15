using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorContrl : MonoBehaviour
{
    [Header("電梯本體")]
    public MoveGround moveGround;
    [Header("貓")]
    public CatContrl Cat;
    [Header("最後的位置")]
    public GameObject OverPos;
    public GameObject OpenKey;

    [Header("上升速度")]
    public float Speed;

    [Header("紅色移動陷阱蟲")]
    public GameObject RedBox;
    [Header("綠色噴汁蟲")]
    public GameObject GreenBox;
    
    // Start is called before the first frame update
    void Start()
    {
        Cat = GameObject.FindObjectOfType<CatContrl>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Cat.NowCatAct == CatContrl.CatAct.CatDie)
        {
            CatDie();
        }

        if (moveGround.transform.position.y >= OverPos.transform.position.y - 0.5f)
        {
            ChangeSpeed(0);
        }
    }

    public void CatDie()
    {
        Bug_MovePike[] Bug = GameObject.FindObjectsOfType<Bug_MovePike>();
        for (int i = 0; i < Bug.Length; i++)
        {
            Destroy(Bug[i].gameObject);
        }

        OpenKey.SetActive(true);
        ChangeSpeed(0);
        for (int i = 0; i < RedBox.transform.childCount; i++)
        {
            RedBox.transform.GetChild(i).gameObject.SetActive(true);
            RedBox.transform.GetChild(i).GetComponent<Bug_MovePikeBox>().AllStart(); 
        }
        RedBox.SetActive(false);
        GreenBox.SetActive(false);
    }

    public void GetStart()
    {
        RedBox.SetActive(true);
        GreenBox.SetActive(true);
    }

    public void ChangeSpeed(float speed)
    {
        moveGround.MoveSpeed = speed;
    }
}
