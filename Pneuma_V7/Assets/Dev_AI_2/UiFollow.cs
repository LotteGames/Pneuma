using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiFollow : MonoBehaviour
{
    public GameObject uiObj;
    void Update()
    {
        uiObj.transform.position = transform.position;
    }
}
