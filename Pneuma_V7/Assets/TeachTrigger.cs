using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeachTrigger : MonoBehaviour
{
    public GameObject OpenTeach;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<CatContrl>() != null)
        {
            collision.GetComponent<CatContrl>().CatActStop();
            collision.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            OpenTeach.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
