using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurVelocity : MonoBehaviour
{
    public Material material;
    public Vector3 lastPos, vector;
    void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
        lastPos = transform.position;
    }

    void Update()
    {
        vector = transform.position - lastPos;
        material.SetVector("_Velocity", new Vector4(vector.x, vector.y, 0, 0));


        lastPos = transform.position;
    }
}
