using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBallGo : MonoBehaviour
{
    [Header("最終所在")]
    public GameObject GoToWhere;
    [Header("初始點")]
    public Vector3 StartPos;
    [Header("目標點")]
    public Vector3 OverPos;


    [Header("中間點")]
    public Vector3 Pos;

    [Header("攻擊速度")]
    public float Speed;

    Vector3 P1;
    Vector3 P2;
    float T;
    void Start()
    {
        //Speed = Random.Range(1, 1.6f);
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        StartPos = transform.position;
        OverPos = GoToWhere.transform.position;
        float Remove = Vector3.Distance(transform.position, GoToWhere.transform.position);
        Pos = transform.TransformPoint(new Vector3(0, Remove, 0));
    }

    void Update()
    {
        T += Time.deltaTime * Speed;
        P1 = Vector2.Lerp(StartPos, P2, T);
        P2 = Vector2.Lerp(Pos, OverPos, T);
        transform.position = P1;

        Vector3 P = Vector2.Lerp(P2, OverPos, 0.5f);

        Vector3 direction = P - transform.position;
        direction.z = 0f;
        direction.Normalize();

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, targetAngle - 90);

        if (T >= 0.95f)
        {
            gameObject.SetActive(false);
        }
    }
}
