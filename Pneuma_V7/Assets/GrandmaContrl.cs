using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandmaContrl : MonoBehaviour
{
    [Header("移動速度")]
    public PlayerSystem playerSystem;

    [Header("移動速度")]
    public float MoveSpeed;
    [Header("跳躍力道")]
    public float JumpPower;


    [Header("可以進行跳躍")]
    public bool CanJump;
    [Header("是否對奶奶進行操作")]
    public bool ContrlMe;
    // Start is called before the first frame update
    public void Init()
    {
        playerSystem = GameObject.Find("PlayerSystem").GetComponent<PlayerSystem>();
    }

    // Update is called once per frame
    public void Main()
    {
        Move();
        Jump();
    }

    void Update()
    {
        LookNowContrl();
    }

    public void Move()
    {      
        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.Translate(1 * MoveSpeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            transform.Translate(1 * MoveSpeed * Time.deltaTime, 0, 0);
        }
    }
    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W) && CanJump == true)
        {
            CanJump = false;
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, JumpPower));
        }
    }

    public void LookNowContrl()
    {
        if(playerSystem.NowContrlPlayer != this.gameObject)
        {
            ContrlMe = true;
            gameObject.layer = LayerMask.NameToLayer("CatBody");
        }
        else
        {
            ContrlMe = false;
            gameObject.layer = LayerMask.NameToLayer("Grandma");
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            CanJump = true;
        }
        if (collision.gameObject.tag == "CatBody")
        {
            CanJump = true;
        }
    }
}
