using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line_Link : MonoBehaviour
{
    
    [Header("初始位置")]
    public GameObject StartPos;
    [Header("結束位置")]
    public GameObject OverPos;

    LineRenderer Line;

    [Header("攻擊目標")]
    public GameObject LineTarget;

    [Header("目標位置偏移隨機數值")]
    public Vector3 RandomVect;
    [Header("延長速度")]
    public float AttackSpeed;
    // Start is called before the first frame update
    void Start()
    {
        Line = GetComponent<LineRenderer>();
        RandomVect = new Vector3(Random.Range(-RandomVect.x, RandomVect.x), Random.Range(-RandomVect.y, RandomVect.y), 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(LineTarget != null)
        {
            Line.SetPosition(0, OverPos.transform.position);
            Line.SetPosition(1, StartPos.transform.position);

            OverPos.transform.position = Vector3.Lerp(OverPos.transform.position, LineTarget.transform.position + RandomVect,0.2f);
        }

    }
}
