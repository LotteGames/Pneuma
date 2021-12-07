using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDownPike : MonoBehaviour
{
    [Header("掉落位置")]
    public Vector2 StartPos;
    [Header("要掉落的位置")]
    public GameObject DownPos;

    [Header("要掉落的物件")]
    public GameObject DownPike;
    private GameObject NowPike;
    [Header("幾秒後再次生成陷阱")]
    public float TimeCreatePike;
    [Header("陷阱重量")]
    public float DownPikegravity;
    // Start is called before the first frame update
    void Start()
    {
        StartPos = DownPos.transform.position;
        NowPike = Instantiate(DownPike, StartPos, Quaternion.Euler(0, 0, 0));
        NowPike.GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    public void Down()
    {
        if(NowPike != null)
        {
            NowPike.GetComponent<Rigidbody2D>().gravityScale = DownPikegravity;
            NowPike = null;
            StartCoroutine(CreatePike());
        }
    }

    public IEnumerator CreatePike()
    {
        yield return new WaitForSeconds(TimeCreatePike);
        NowPike = Instantiate(DownPike, StartPos, Quaternion.Euler(0, 0, 0));
        NowPike.GetComponent<Rigidbody2D>().gravityScale = 0;
    }
}
