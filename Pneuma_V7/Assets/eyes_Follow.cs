using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eyes_Follow : MonoBehaviour
{
    [Header("貓咪")]
    public GameObject Cat;
    [Header("瞳孔")]
    public GameObject eye;

    [Header("半徑")]
    public float radius;

    // Start is called before the first frame update
    void Start()
    {
        Cat = GameObject.FindObjectOfType<CatContrl>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        float Remove = Vector2.Distance(Cat.transform.position, transform.position);
        Debug.Log(Remove);
        if (Remove <= radius * 1.5f)
        {
            eye.transform.position = Vector3.Lerp(eye.transform.position, Cat.transform.position, 0.15f);
        }
        else
        {
            eye.transform.position = Vector3.Lerp(eye.transform.position, transform.position + (Cat.transform.position - eye.transform.position).normalized * radius,0.2f);
        }
    }
}
