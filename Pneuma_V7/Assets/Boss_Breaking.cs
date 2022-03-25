using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Breaking : MonoBehaviour
{
    [Header("破壞時的特效")]
    public GameObject BreakAni;
    
    public void Breaking()
    {
        if(BreakAni != null)
        {
            Instantiate(BreakAni, transform.position, Quaternion.Euler(-90, 0, 0));
        }
    }
}
