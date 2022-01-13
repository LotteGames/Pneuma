using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownMoveGround : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<CatContrl>() != null)
        {
            collision.transform.parent = transform;
            collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
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

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<CatContrl>() != null)
        {
            collision.transform.parent = null;
            collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = collision.gameObject.GetComponent<CatContrl>().CatWeight;
        }
    }
}
