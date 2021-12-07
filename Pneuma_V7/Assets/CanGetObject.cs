using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanGetObject : MonoBehaviour
{
    [Header("初始位置")]
    public Vector2 StartPos;
    [Header("物件的名稱")]
    public string ObjectName;
    [Header("原始重量")]
    public float Weight;

    /// <summary>
    /// 給予最一開始物件的重量和設定
    /// </summary>
    public void SetStart(Vector3 NewPos)
    {
        transform.tag = "Untagged";
        transform.position = NewPos;
        GetComponent<Collider2D>().isTrigger = false;
        GetComponent<Rigidbody2D>().gravityScale = Weight;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        StartCoroutine(DelayToOpenCollider());
    }

    /// <summary>
    /// 給予被貓咪獲得後的數值
    /// </summary>
    public void SetGet()
    {
        GetComponent<Collider2D>().isTrigger = true;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }

    public IEnumerator DelayToOpenCollider()
    {
        yield return new WaitForSeconds(3);
        transform.tag = "Props";
    }
}
