using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

[ExecuteAlways]
public class LimitedSpace : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private Collider2D collider2D_poly;

    public void SetColor(Color color)
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        spriteRenderer.color = color;
    }
    public void SetCollider(bool value)
    {
        if (collider2D_poly == null)
        {
            collider2D_poly = GetComponent<Collider2D>();
        }
        collider2D_poly.enabled = value;
    }
}
