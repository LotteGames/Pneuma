using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug_Down_Box : MonoBehaviour
{
    [Header("要生成的掉落蟲蟲")]
    public GameObject CreateBug;
    public GameObject myBug;
    [Header("蟲蟲的位置")]
    public GameObject BugPos;
    [Header("生成延遲時間")]
    public float CreateTime;


    // Update is called once per frame
    void Update()
    {
        if(myBug != null)
        {
            myBug.transform.position = Vector2.Lerp(myBug.transform.position, BugPos.transform.position, 0.16f);
        }
    }

    public void BugDown()
    {
        myBug = null;
        StartCoroutine(DelayCreate());
    }

    public IEnumerator DelayCreate()
    {
        yield return new WaitForSeconds(CreateTime);
        myBug = Instantiate(CreateBug, transform.position, Quaternion.Euler(0, 0, 0), transform);
        GetComponent<LineRenderer_Contrl>().Pos[1] = myBug;
    }
}
