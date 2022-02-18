using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchGround : MonoBehaviour
{
    [Header("可以站上面")]
    public bool CanIn;

    [Header("貓咪是否在上面")]
    public bool CatOn;

    [Header("是否是陷阱")]
    public bool IsPike;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<CatContrl>() != null)
        {
            if (IsPike == false && CanIn == true)
            {
                collision.transform.parent = transform;
                collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                CatOn = true;
            }
        }
    }

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (collision.gameObject.GetComponent<CatContrl>() != null)
    //    {
    //        collision.transform.parent = transform.parent;
    //        collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
    //        CatOn = true;
    //    }
    //}

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<CatContrl>() != null)
        {
            if (IsPike == false)
            {
                collision.transform.parent = null;
                collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = collision.gameObject.GetComponent<CatContrl>().CatWeight;
                //GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                CatOn = false;
            }
        }
    }
}
