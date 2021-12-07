using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBall : MonoBehaviour
{
    [Header("花朵的代號")]
    public string Number;
    [Header("自動追尋貓咪")]
    public OldCatContrl Cat;

    [Header("是否是一次性的")]
    public bool IsOne;
    [Header("重生的冷卻時間")]
    public float DelateTime;
    [Header("碰觸後的圖片")]
    public Sprite TouchPictrue;
    // Start is called before the first frame update
    void Start()
    {
        Cat = GameObject.FindObjectOfType<OldCatContrl>();

    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    //private void OnMouseEnter()
    //{
    //    CatGetBall();
    //}

    public void OnMouseLeft()
    {
        if (Cat.RedBall == this.gameObject)
        {
            Cat.RedBall = null;
        }
    }
    public void CatTouch()
    {
        GetComponent<Animator>().Play("RedBall_Touch");
        PlayerPrefs.SetString(Number, "T");
    }

    //public void CatGetBall()
    //{
    //    Cat.RedBall = this.gameObject;
    //    PlayerPrefs.SetString(Number, "T");
    //}

    public void OpenAgain()
    {
        StartCoroutine(DelayOpen());
    }

    public IEnumerator DelayOpen()
    {
        yield return new WaitForSeconds(DelateTime);
        GetComponent<Animator>().SetBool("Break", false);
    }
}
