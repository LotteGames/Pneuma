using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

[ExecuteAlways]
public class CameraDisplay : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private Collider2D collider2D_poly;

    public void SetDisplay(SizeValue sizeValue)
    {
        transform.localScale = new Vector3(sizeValue.Size * 2 * Camera.main.aspect, sizeValue.Size * 2, 1);
    }

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
