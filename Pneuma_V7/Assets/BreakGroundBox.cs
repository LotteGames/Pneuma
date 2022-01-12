using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakGroundBox : MonoBehaviour
{
    [Header("要生成的石頭")]
    public GameObject BreakRock;
    [Header("生成時間")]
    public float CreateTime;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }

    // Update is called once per frame
    public void InsNew()
    {
        StartCoroutine(Delay(CreateTime));
    }

    public IEnumerator Delay(float DelayTime)
    {
        yield return new WaitForSeconds(DelayTime);
        GameObject Ins = Instantiate(BreakRock, transform.position, Quaternion.Euler(0, 0, 0), transform);
        Ins.GetComponent<BreakGround>().Box = GetComponent<BreakGroundBox>();
    }
}