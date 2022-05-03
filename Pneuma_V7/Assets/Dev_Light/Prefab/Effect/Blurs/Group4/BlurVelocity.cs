using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurVelocity : MonoBehaviour
{

    public Transform targetTransform;
    public Material material;
    public Vector3 lastPos, vector;
    void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
        lastPos = targetTransform.position;
    }
public bool useY=true,useHalfY=true;

    void Update()
    {
        vector = targetTransform.position - lastPos;
        material.SetVector("_Velocity", new Vector4(vector.x,(useY?vector.y:0)*(useHalfY?.5f:1) , 0, 0));

Debug.LogError("vector: "+vector);

        lastPos = targetTransform.position;
    }
}
