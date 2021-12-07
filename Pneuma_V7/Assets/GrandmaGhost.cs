using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandmaGhost : MonoBehaviour
{
    [Header("想要的東西")]
    public string NeedObject;
    [Header("想要的東西(提示)")]
    public GameObject NeedObjectPictrue;

    [Header("回贈的東西")]
    public GameObject ForPlayerObject;

    [Header("給完東西是否會消失")]
    public bool IsDestroy;

    [Header("給完東西是否有下一個想要的東西")]
    public bool IsNext;
    public GameObject NextGrandma;

    public void TouchGrandma(CatContrl Cat)
    {
        if(Cat.GetObject != null)
        {
            if (Cat.GetObject.GetComponent<CanGetObject>().ObjectName == NeedObject)
            {
                GameObject For = Instantiate(ForPlayerObject, transform.position + new Vector3(0,4,0), Quaternion.Euler(0, 0, 0));
                Destroy(Cat.GetObject);
                For.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1000 * For.GetComponent<Rigidbody2D>().gravityScale));
                if (IsDestroy == true)
                {
                    Destroy(gameObject);
                }
                else if (IsNext == true)
                {
                    NextGrandma.SetActive(true);
                    Destroy(gameObject);
                }
            }
            else
            {
                StartCoroutine(DelayPictrue());
            }
        }
        else
        {
            StartCoroutine(DelayPictrue());
        }
    }


  
    //public void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.transform.tag == "Cat" && collision.transform.GetComponent<CatContrl>() == true)
    //    {
    //        if (collision.transform.GetComponent<CatContrl>().GetObject == NeedObject)
    //        {
    //            GameObject For = Instantiate(ForPlayerObject, transform.position, Quaternion.Euler(0, 0, 0));
    //            For.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 200 * For.GetComponent<Rigidbody2D>().gravityScale));
    //            if (IsDestroy == true)
    //            {
    //                Destroy(gameObject);
    //            }
    //            else if (IsNext == true)
    //            {
    //                NextGrandma.SetActive(true);
    //                Destroy(gameObject);
    //            }
    //        }
    //        else
    //        {
    //            StartCoroutine(DelayPictrue());
    //        }
    //    }
    //}

    public IEnumerator DelayPictrue()
    {
        NeedObjectPictrue.SetActive(true);
        yield return new WaitForSeconds(3);
        NeedObjectPictrue.SetActive(false);
    }
}
