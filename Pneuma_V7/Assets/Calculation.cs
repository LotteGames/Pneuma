using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calculation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Lens.FieldOfView = (2.0f * Mathf.Atan(12f / 10f) * Mathf.Rad2Deg);
        Debug.LogError("frustumHeight : " + (2.0f * 10 * Mathf.Tan(95 * 0.5f * Mathf.Deg2Rad)));


        Debug.LogError("frustumHeight : " + (2.0f * 10 * Mathf.Tan(61.92752f * 0.5f * Mathf.Deg2Rad)));

        Debug.LogError("fov : " + (2.0f * Mathf.Atan(12f  / 10f) * Mathf.Rad2Deg));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
