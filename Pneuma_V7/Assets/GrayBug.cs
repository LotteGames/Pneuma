using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrayBug : MonoBehaviour
{
    [Header("移動速度")]
    public float MoveSpeed;
    [Header("旋轉速度")]
    public float RotSpeed;
    public float Rot;
    public float RR;
    [Header("幾秒生成一個咬痕")]
    public float EatSpeed;
    [Header("幾秒生成一個咬痕")]
    public GameObject EatBlack;
    float T = 0;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(EatBlack, transform.position, Quaternion.Euler(0, 0, 0), transform.parent.transform);
        Rot = Random.Range(0, 360);
        RR = Random.Range(-RotSpeed, RotSpeed);
        transform.rotation = Quaternion.Euler(0, 0, Rot);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(MoveSpeed, 0, 0) * Time.deltaTime);

        Rot += RR * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, 0, Rot);

        T += Time.deltaTime;
        if(T >= EatSpeed)
        {
            int x = Random.Range(0, 3);
            if(x <= 0)
            {
                RR = Random.Range(-RotSpeed, RotSpeed);
            }
            Instantiate(EatBlack, transform.position, Quaternion.Euler(0, 0, 0), transform.parent.transform);
            T = 0;
        }
    }
}
