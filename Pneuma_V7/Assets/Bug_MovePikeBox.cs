using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug_MovePikeBox : MonoBehaviour
{
    [Header("要生成的滾動蟲蟲")]
    public GameObject Bug_MovePike;
    [Header("生成前的動畫")]
    public GameObject Bug_CreateAni;

    [Header("生成間隔時間")]
    public float CreateTime;
    float T;
    [Header("生成次數")]
    public int CreateCount;

    public int SaveStartCount;

    // Start is called before the first frame update
    void Start()
    {
        Bug_CreateAni.SetActive(false);
        SaveStartCount = CreateCount;
        T = 0;
    }

    public void AllStart()
    {
        T = 0;
        CreateCount = SaveStartCount;
    }

    // Update is called once per frame
    void Update()
    {
        T += Time.deltaTime;

        if (T >= CreateTime - 3)
        {
            Bug_CreateAni.SetActive(true);
        }

        if (T >= CreateTime)
        {
            Instantiate(Bug_MovePike, transform.position, Quaternion.Euler(0, 0, 0));
            Bug_CreateAni.SetActive(false);
            CreateCount--;
            T = 0;
        }
        if(CreateCount <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
