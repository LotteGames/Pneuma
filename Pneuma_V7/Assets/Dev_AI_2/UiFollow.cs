using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiFollow : MonoBehaviour
{
    public GameObject uiObj;
     Vector3 vector;

    private void Start()
    {
        vector = uiObj.transform.position - transform.position;
    }
    void Update()
    {
        uiObj.transform.position = transform.position + vector;
    }
}
