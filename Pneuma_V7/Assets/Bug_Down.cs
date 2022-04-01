using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug_Down : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<CatContrl>() != null)
        {
            collision.GetComponent<CatContrl>().CanLong = true;//可以延長
            collision.GetComponent<CatContrl>().CanJump = true;
            collision.GetComponent<CatContrl>().GetJump();
            Down();
        }
    }
    
    public void Down()
    {
        transform.parent.GetComponent<Bug_Down_Box>().BugDown();
        transform.parent.GetComponent<LineRenderer_Contrl>().Pos[1] = transform.parent.gameObject;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 4;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        Destroy(gameObject, 3);
    }
}
