using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug_MovePike : MonoBehaviour
{
    [Header("初始跳起來的力道")]
    public float StartPower;
    [Header("旋轉速度")]
    public float RotSpeed;
    public float nowRot;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, StartPower));
        if(GameObject.FindObjectOfType<CatContrl>().transform.position.x <= transform.position.x)
        {
            RotSpeed = RotSpeed * 1;
        }
        else
        {
            RotSpeed = RotSpeed * -1;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Collider2D>().isTrigger == true)
        {
            if(GetComponent<Rigidbody2D>().velocity.y <= -0.01f)
            {
                GetComponent<Collider2D>().isTrigger = false;
            }
        }

        nowRot += RotSpeed * Time.deltaTime;
        //GetComponent<Rigidbody2D>().AddTorque(RotSpeed);
        GetComponent<Rigidbody2D>().MoveRotation(nowRot);

        //if (Input.GetKeyDown(KeyCode.Y))
        //{
        //    GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        //    GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 800));
        //}
    }

    public void BugDie(float UpPower)
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, UpPower));
        Destroy(gameObject, 3.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GetComponent<Collider2D>().enabled == true)
        {
            if (collision.gameObject.GetComponent<CatContrl>() != null)
            {
                BugDie(800);
            }

            if (collision.gameObject.tag == "Pike")
            {
                BugDie(600);
            }
        }
    }
}
