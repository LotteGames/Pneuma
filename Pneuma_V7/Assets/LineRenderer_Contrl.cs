using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRenderer_Contrl : MonoBehaviour
{
    [Header("繩索系統")]
    public LineRenderer myLine;

    [Header("要跟隨的頭尾")]
    public GameObject[] Pos;


    // Start is called before the first frame update
    void Start()
    {
        myLine = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        myLine.SetPosition(0, new Vector3(Pos[0].transform.position.x, Pos[0].transform.position.y, 0));
        myLine.SetPosition(1, Vector3.Lerp(Pos[0].transform.position, Pos[1].transform.position, 0.5f));
        myLine.SetPosition(2, new Vector3(Pos[1].transform.position.x, Pos[1].transform.position.y, 0));
    }
}
