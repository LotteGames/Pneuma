using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownPike_Bug : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "CatBody" || collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Pike")
        {
            GetComponent<Collider2D>().isTrigger = true;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1300));
            transform.rotation = Quaternion.Euler(0, 0, 180);
            Destroy(gameObject, 8);
        }
    }


}
