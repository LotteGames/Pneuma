using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBugAni : MonoBehaviour
{

    public bool Run;
    public float Speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Run == true)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(Speed , GetComponent<Rigidbody2D>().velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<CatContrl>() != null)
        {
            Run = true;
            Destroy(gameObject, 3f);
        }
    }
}
