using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownPike : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Pike")
        {
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
            GetComponent<Rigidbody2D>().AddForce(new Vector3(0, GetComponent<Rigidbody2D>().gravityScale * 400, 0));
            Destroy(gameObject, 4);
        }
    }
}
