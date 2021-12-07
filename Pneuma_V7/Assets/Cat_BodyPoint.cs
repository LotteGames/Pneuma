using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat_BodyPoint : MonoBehaviour
{
    public GameObject[] BodyPoint; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < BodyPoint.Length; i++)
        {
            BodyPoint[i].transform.position = transform.position;
        }
    }
}
