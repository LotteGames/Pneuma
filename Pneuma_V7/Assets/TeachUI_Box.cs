using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeachUI_Box : MonoBehaviour
{
    [Header("需要達成的按鍵次數")]
    public int Count;

    [Header("完成指令後出現的東西")]
    public GameObject CoinDoor;

    // Start is called before the first frame update
    void Start()
    {
        Count = this.gameObject.transform.childCount;
    }

    public void GetKey()
    {
        Count--;
        if(Count <= 0)
        {
            CoinDoor.SetActive(true);
            Destroy(gameObject,2);
        }
    }
}
