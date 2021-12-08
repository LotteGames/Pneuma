using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudyMove : MonoBehaviour
{
    
    [Header("移動速度")]
    public float MoveSpeed;
    [Header("邊界值")]
    public Vector2 MiniPosX;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(transform.localPosition.x);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(-1, 0, 0) * Time.deltaTime * MoveSpeed;
        if(transform.localPosition.x <= MiniPosX.x)
        {
            transform.localPosition = new Vector3(-MiniPosX.x, MiniPosX.y, 0);
        }
    }
}
