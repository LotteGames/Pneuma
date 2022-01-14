using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletInfo : MonoBehaviour
{
    public Rigidbody2D rb2D;

    public float speed = 3;
    public void SetDir(float degree)
    {
        transform.rotation = Quaternion.Euler(0, 0, degree);
    }
    public void SetVelocity(Vector3 vector)
    {
        Debug.LogError("vector : " + vector.ToString());
        vector = vector.normalized;
        Debug.LogError("velocity : " + new Vector2(vector.x, vector.y) * speed);
        rb2D.velocity = new Vector2(vector.x, vector.y) * speed;
    }
}