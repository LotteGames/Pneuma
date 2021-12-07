using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandmaRecall : MonoBehaviour
{
    [Header("場景中的貓咪程式")]
    public OldCatContrl Cat;
    public GameObject CatObject;

    [Header("最一開始會噴去哪(以及後面跟隨的位置)")]
    public Vector3 StartPos;
    [Header("跟隨的位置")]
    public Vector3 FallowPos;
    [Header("跟隨的位置的跟隨的位置")]
    public Vector3 FallowPos_2;

    [Header("位置移動速度")]
    public float MoveSpeed;

    [Header("徘徊的機率")]
    public int GoToCat;

    [Header("消失特效")]
    public GameObject BreakAni;

    [Header("換位置的時間")]
    public float DelateTimeMin, DelateTimeMax;
    public float DelateTime;
    float T;
    // Start is called before the first frame update
    void Start()
    {
        GoToCat = 1;
        DelateTime = Random.Range(DelateTimeMin, DelateTimeMax);
        Cat = GameObject.FindObjectOfType<OldCatContrl>();
        CatObject = Cat.gameObject;
        StartPos = transform.position;
        //MoveSpeed = Random.Range(0.01f, 0.05f);
        FallowPos = StartPos + new Vector3(Random.Range(-9, 9), Random.Range(5,10), 0);
        FallowPos_2 = new Vector3(Random.Range(-9, 9), Random.Range(5, 10), 0);
    }

    // Update is called once per frame
    void Update()
    {
        T += Time.deltaTime;
        if(GoToCat < 2)//最多徘徊2次
        {

            FallowPos = Vector3.Lerp(FallowPos, StartPos + FallowPos_2, MoveSpeed);//二次追蹤，會產生不規則弧形
            transform.position = Vector3.Lerp(transform.position, FallowPos, MoveSpeed);//二次追蹤，會產生不規則弧形

            if (T >= DelateTime)
            {
                T = 0;
                DelateTime = Random.Range(DelateTimeMin, DelateTimeMax);

                int X = Random.Range(0, GoToCat);
                if (X > 2)
                {
                    GoToCat = 10;
                    FallowPos_2 = Cat.gameObject.transform.position;
                    MoveSpeed = 0.05f;
                }
                else
                {
                    GoToCat += 1;

                    FallowPos_2 = new Vector3(Random.Range(-8, 8),  Random.Range(-8, 8), 0);
                    FallowPos = CatObject.transform.position + new Vector3(Random.Range(-8, 8), Random.Range(-8, 8), 0);
                }
            }
        }
        else
        {
            MoveSpeed = 0.05f;

            FallowPos = Vector3.Lerp(FallowPos, Cat.gameObject.transform.position, MoveSpeed);//二次追蹤，會產生不規則弧形
            transform.position = Vector3.Lerp(transform.position, FallowPos, MoveSpeed);//二次追蹤，會產生不規則弧形

            float Remove = Vector3.Distance(transform.position, FallowPos);
            if (Remove <= 0.5f)
            {
                GameObject Ani = Instantiate(BreakAni, transform.position, Quaternion.Euler(0, 0, 0));

                Destroy(Ani, Ani.GetComponent<ParticleSystem>().duration + Ani.GetComponent<ParticleSystem>().startLifetime);
                Destroy(gameObject);
            }
        }
       
    }
}
