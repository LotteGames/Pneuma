using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorContrl : MonoBehaviour
{
    public MoveGround moveGround;
    public CatContrl Cat;
    public GameObject OverPos;
    public GameObject OpenKey;

    public float Speed;
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
        OpenKey.SetActive(true);
        ChangeSpeed(0);
    }

    public void ChangeSpeed(float speed)
    {
        moveGround.MoveSpeed = speed;
    }
}
