using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calculate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.LogError("Value : " + 2.0f * Mathf.Atan(24 * 0.5f / 10f) * Mathf.Rad2Deg);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
