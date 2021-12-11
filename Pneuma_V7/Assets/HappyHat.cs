using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappyHat : MonoBehaviour
{
   [Header("帽帽跟隨的貓咪")]
    public GameObject Cat;

    Rigidbody2D hat_Rb;
    // Start is called before the first frame update
    void Start()
    {
        hat_Rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Cat.transform.position.x, transform.position.y, 0);
        transform.rotation = Quaternion.Euler(transform.rotation.x, Cat.transform.rotation.y - 180, transform.rotation.x);
    }
}
