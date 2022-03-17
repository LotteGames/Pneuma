using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBullet : MonoBehaviour
{

    private void Start()
    {
        GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0, 1000));
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0, 1000));
        //}
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(1000, 0));
        //}
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(1000, 0));
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Ground" || collision.tag == "Wall")
        {
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Destroy(gameObject, 3);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Wall")
        {
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Destroy(gameObject, 3);
        }
    }
}
