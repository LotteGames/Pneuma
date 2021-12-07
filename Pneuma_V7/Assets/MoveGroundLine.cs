using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGroundLine : MonoBehaviour
{
    [Header("繩索兩端")]
    public GameObject PosA;
    public GameObject PosB;

    public LineRenderer Line;

    // Start is called before the first frame update
    void Start()
    {
        Line = GetComponent<LineRenderer>();
        Line.SetPosition(0, PosA.transform.position);
        Line.SetPosition(1, PosB.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        Line.SetPosition(0, PosA.transform.position);
        Line.SetPosition(1, PosB.transform.position);
    }
}
