using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_CreatePike : MonoBehaviour
{
    [Header("腳的Pike")]
    public GameObject BonePike;

    [Header("動畫最初偵數")]
    public float time;

    public Animator Ani;
    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<Animator>() != null)
        {
            Ani = GetComponent<Animator>();
            Ani.Update(time);
        }

        Instantiate(BonePike, transform.position, transform.rotation);
    }


}
