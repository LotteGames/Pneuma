using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyToDoor : MonoBehaviour
{
    [Header("是否開啟")]
    public bool IsTouch;

    [Header("是否是按一次永久性的")]
    public bool IsOne;

    [Header("是否需要道具")]
    public bool IsNeedProps;
    public string NeedPropsName;

    [Header("已經獲得")]
    public bool IsOver;

    [Header("需要道具的提示圖")]
    public GameObject NeedPropsPictrue;

    //public GameObject myDoor;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "CatBody")
        {
            if(IsNeedProps == false)
            {
                IsTouch = true;
            }
            else
            {
                CatContrl Cat = GameObject.FindObjectOfType<CatContrl>();

                if (Cat.GetObject != null)
                {
                    if (NeedPropsName == Cat.GetObject.GetComponent<CanGetObject>().ObjectName)
                    {
                        IsTouch = true;
                        GameObject MyNeed = Cat.GetObject;
                        Cat.GetObject = null;
                        MyNeed.transform.position = transform.position;
                        MyNeed.GetComponent<Rigidbody2D>().gravityScale = 0;
                        IsOver = true;
                        if (GetComponent<Animator>() != null)
                        {
                            GetComponent<Animator>().Play("KeyTouch");
                        }
                    }
                    else
                    {
                        //給需要的物件的提示(之後做成動畫)
                        if(IsOver == false)
                        {
                            StartCoroutine(DelayPictrue());
                        }
                    }
                }
                else
                {
                    //給需要的物件的提示(之後做成動畫)
                    if (IsOver == false)
                    {
                        StartCoroutine(DelayPictrue());
                    }
                }
       
            }
        }
    }

    public IEnumerator DelayPictrue()
    {
        if(GetComponent<Animator>() != null)
        {
            GetComponent<Animator>().Play("KeyTouch");
        }
        NeedPropsPictrue.SetActive(true);
        yield return new WaitForSeconds(3);
        NeedPropsPictrue.SetActive(false);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "CatBody" && IsOne == false)
        {
            IsTouch = false;
        }
    }
}
