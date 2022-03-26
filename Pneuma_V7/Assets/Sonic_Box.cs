using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sonic_Box : MonoBehaviour
{
    private void Start()
    {
        ShockOver();
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
    public void Des()
    {
        Destroy(gameObject);
    }
}
