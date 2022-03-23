using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Bone : MonoBehaviour
{
    [Header("偵測範圍")]
    public BossCircle Circle;

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
        if(collision.GetComponent<CatContrl>() != null && Circle.PlayerIn == true)
        {
            Vector2 direction = collision.transform.position - transform.position;

            direction.Normalize();
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            collision.transform.rotation = Quaternion.Euler(0, 0, targetAngle - 90);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<CatContrl>() != null && Circle.PlayerIn == true)
        {
            Vector2 direction = collision.transform.position - transform.position;

            direction.Normalize();
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            collision.transform.rotation = Quaternion.Euler(0, 0, targetAngle - 90);
        }
    }
}
