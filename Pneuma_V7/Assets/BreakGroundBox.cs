using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakGroundBox : MonoBehaviour
{
    [Header("要生成的石頭")]
    public GameObject BreakRock;
    [Header("生成時間")]
    public float CreateTime;

    Quaternion Rot;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }
    public void SetStart()
    {
        if(transform.childCount == 0)
        {
            GameObject Ins = Instantiate(BreakRock, transform.position, transform.rotation, transform);
            Ins.GetComponent<BreakGround>().Box = GetComponent<BreakGroundBox>();
        }
    }

    // Update is called once per frame
    public void InsNew()
    {
        StartCoroutine(Delay(CreateTime));
    }

    public IEnumerator Delay(float DelayTime)
    {
        yield return new WaitForSeconds(DelayTime);
        GameObject Ins = Instantiate(BreakRock, transform.position, transform.rotation, transform);
        Ins.GetComponent<BreakGround>().Box = GetComponent<BreakGroundBox>();
    }
}
