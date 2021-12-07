using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveDev : MonoBehaviour
{
    Rigidbody2D rb2D;

    SpriteRenderer spriteRenderer;

    public float gravity = 24, jumpVelocity = 12, speed = 3;

    public ContactFilter2D filter2D;
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public LayerMask layerMask;
    public float HorizontalInput
    {
        get
        {
            return
                (
                ((Input.GetKey(KeyCode.RightArrow) && Physics2D.Raycast(transform.position + new Vector3(0, 0.5f), Vector2.right, 0.55f, layerMask) == false) ? 1 : 0) +

                ((Input.GetKey(KeyCode.LeftArrow) && Physics2D.Raycast(transform.position + new Vector3(0, 0.5f), Vector2.left, 0.55f, layerMask) == false) ? -1 : 0));
        }
    }
    public bool IsGround
    {
        get
        {
            bool value = rb2D.IsTouching(filter2D);

            if (value)
            {
                //Debug.Log("Touch");
            }
            return value;
        }
    }

    private void Update()
    {
        //Debug.Log((Input.GetKey(KeyCode.RightArrow) && Physics2D.Raycast(transform.position + new Vector3(0, 0.5f), Vector2.right, 0.55f, layerMask) == false));
        //Debug.Log((Input.GetKey(KeyCode.LeftArrow) && Physics2D.Raycast(transform.position + new Vector3(0, 0.5f), Vector2.left, 0.55f, layerMask) == false));

        if (IsGround)
        {
            Jump();
        }
    }
    void FixedUpdate()
    {
        Move();

        rb2D.AddForce(Vector2.down * gravity);
    }
    public void Move()
    {
        Vector2 velocity = rb2D.velocity;

        rb2D.velocity = new Vector2(HorizontalInput * speed, velocity.y);
    }


    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 velocity = rb2D.velocity;

            rb2D.velocity = new Vector2(velocity.x, jumpVelocity);
        }
    }
}
