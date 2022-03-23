using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCircle : MonoBehaviour
{

    [Header("玩家在範圍內")]
    public bool PlayerIn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<CatContrl>() != null)
        {
            PlayerIn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<CatContrl>() != null)
        {
            PlayerIn = false;
            collision.transform.parent = null;
        }
    }
}
