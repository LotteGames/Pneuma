using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSonic : MonoBehaviour
{
    [Header("破壞時的特效")]
    public GameObject Sonic;
    [Header("吼叫點")]
    public GameObject Pos;
    [Header("是否震動?")]
    public bool IsShock;

    private void Start()
    {
        ShockOver();
    }
    public void Sonicing()
    {
        if (Sonic != null)
        {
            GameObject mysonic = Instantiate(Sonic, Pos.transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(mysonic, 2);
            if(IsShock == true)
            {
                Shock(1.2f);
            }
        }
    }

    public void Shock(float Time)
    {
        GameObject.FindObjectOfType<CameraActivate>().CurrentCamArea_Shake.Shake(3, 1, Time);
    }

    public void ShockStart()
    {
        GameObject.FindObjectOfType<CameraActivate>().CurrentCamArea_Shake.Shake(2, 1);
    }
    public void ShockOver()
    {
        GameObject.FindObjectOfType<CameraActivate>().CurrentCamArea_Shake.Shake(0, 0);
    }
}
