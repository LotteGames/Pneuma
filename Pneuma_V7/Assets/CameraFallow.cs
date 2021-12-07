using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFallow : MonoBehaviour
{
    public GameObject Hand;
    public GameObject Butt;

    public bool A;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Hand.GetComponent<OldCatContrl>().NowCatAct == OldCatContrl.CatAct.LongLongCat)
        {
            transform.position = Vector3.Lerp(transform.position, Vector3.Lerp(Hand.transform.position, Butt.transform.position, 0.3f) + new Vector3(0, 2, -10), 0.1f);
            A = true;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, Hand.transform.position, 0.1f) + new Vector3(0, 0, -10);
            //transform.position = Hand.transform.position;
            A = false;
        }
    }
}
