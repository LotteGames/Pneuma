using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSonic : MonoBehaviour
{
    [Header("破壞時的特效")]
    public GameObject Sonic;
    [Header("吼叫點")]
    public GameObject Pos;
    public void Sonicing()
    {
        if (Sonic != null)
        {
            GameObject mysonic = Instantiate(Sonic, Pos.transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(mysonic, 2);
        }
    }
}
