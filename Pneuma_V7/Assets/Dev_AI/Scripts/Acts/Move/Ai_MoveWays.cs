using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai_MoveWays : MonoBehaviour
{
    public Rigidbody2D rb2D;
    public void ResetVelocity()
    {
        rb2D.velocity = Vector2.zero;
    }
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
    public void Walk(float speed)
    {
        Vector2 originVelocity = rb2D.velocity;

        rb2D.velocity = new Vector2(speed, originVelocity.y);
    }
    public void Teleporation(Vector3 targetPos)
    {
        transform.position = targetPos;
    }
    public void Jump(float speed, float jumpVelocity, Vector3 targetPos)
    {
        rb2D.velocity = new Vector2(((targetPos.x > transform.position.x) ? 1 : -1) * speed, jumpVelocity);
    }
}
