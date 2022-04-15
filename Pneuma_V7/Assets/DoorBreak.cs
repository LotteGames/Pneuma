using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBreak : MonoBehaviour
{
    [Header("破壞時的特效")]
    public GameObject BreakAni;
    [Header("是否打開否個物件")]
    public bool Active;
    [Header("打開的物件")]
    public GameObject Door;
    public void Breaking()
    {
        if (Active == true)
        {
            Door.SetActive(true);
        }

        if (BreakAni != null)
        {
            Instantiate(BreakAni, transform.position, Quaternion.Euler(-90, 0, 0));
        }
    }

}
