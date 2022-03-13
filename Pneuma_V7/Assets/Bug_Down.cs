using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug_Down : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<CatContrl>() != null)
        {
            collision.GetComponent<CatContrl>().CanJump = true;
            collision.GetComponent<CatContrl>().GetJump();
        }
    }
}
