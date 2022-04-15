using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongPos : MonoBehaviour
{
    [Header("貓咪")]
    public GameObject Cat;

    // Start is called before the first frame update
    void Start()
    {
        Cat = GameObject.FindObjectOfType<CatContrl>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Turn();
    }
    void Turn()
    {
        Vector3 worldPos = Cat.transform.position;
        Vector3 direction = worldPos - transform.position;


        direction.z = 0f;
        direction.Normalize();
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, targetAngle), TurnSpeed * Time.deltaTime);

        //GetComponent<Rigidbody2D>().MoveRotation(Quaternion.Euler(0, 0, targetAngle - TurnRun));
        transform.rotation = Quaternion.Euler(0, 0, targetAngle + 90);

    }
}
