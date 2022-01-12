using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineContrl : MonoBehaviour
{
    [Header("頭")]
    public GameObject Hand;

    [Header("是否有身體")]
    public bool HaveBody;
    [Header("身體")]
    public GameObject[] Body;

    [Header("屁股")]
    public GameObject Butt;

    [Header("繩索系統")]
    public LineRenderer myLine;

    [Header("繩索系統")]
    public Material M_1;
    public Material M_2;
    [Header("繩索寬度")]
    public float width;
    [Header("碰撞物件位置")]
    public List<Vector2> ColliderPos;

    [Header("碰撞點")]
    public PolygonCollider2D LineCollider;

    [Header("中央點的所在(0~1)")]
    public float ConectWhere;

    // Start is called before the first frame update
    void Start()
    {
        LineCollider = GetComponent<PolygonCollider2D>();
        //for (int i = 0; i < BodyCollider.Count; i++)
        //{
        //    BodyCollider[i] = Instantiate(BodyCollider_Create);
        //    //Debug.Log("X分之" + i + " = " + ((float)(i + 1) / (float)ColliderPos.Count));
        //}
        Butt = GameObject.FindObjectOfType<CatContrl>().gameObject;
        myLine = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Line();

        if (Hand.transform.position.x >= Butt.transform.position.x)
        {
            myLine.material = new Material(M_1);
        }
        else
        {
            myLine.material = new Material(M_2);
        }

        //LineCollider();
    }


    public void Line()
    {
        //for (int i = 0; i < ColliderPos.Count; i++)
        //{
        //    float B = (ColliderPos.Count - 1) / 2;
        //    B = B / i;
        //    myLine.SetPosition(i, ColliderPos[i] + new Vector2(0, B));
        //}
        myLine.SetPosition(0, new Vector3(Hand.transform.position.x, Hand.transform.position.y, 0));
        myLine.SetPosition(1, Vector3.Lerp(Hand.transform.position, Butt.transform.position, ConectWhere));
        myLine.SetPosition(2, new Vector3(Butt.transform.position.x, Butt.transform.position.y, 0));

        ColliderPos = findPoint();
        LineCollider.SetPath(0, ColliderPos.ConvertAll(p => (Vector2)transform.InverseTransformPoint(p)));

        //ColliderPos[0] = new Vector3(Hand.transform.position.x, Hand.transform.position.y, 0);
        //ColliderPos[1] = new Vector3(Butt.transform.position.x, Butt.transform.position.y, 0);

        //ColliderPos[2] = new Vector3(Hand.transform.position.x, Hand.transform.position.y, 0);
        //ColliderPos[3] = new Vector3(Butt.transform.position.x, Butt.transform.position.y, 0);

        /* => (Vector2)transform.InverseTransformPoint(ColliderPos);*/
        //myLine.SetPosition(2, new Vector3(Butt.transform.position.x, Butt.transform.position.y, 0));
        //myLine.SetPosition(2, Hand.transform.position);
    }

    private List<Vector2> findPoint()
    {
        Vector3[] position = new Vector3[myLine.positionCount];
        myLine.GetPositions(position);

        float m = (position[1].y - position[0].y) / (position[1].x - position[0].x);
        float deltaX = (width / 2.4f) * (m / Mathf.Pow(m * m + 1, 0.5f));
        float deltaY = (width / 2.4f) * (1 / Mathf.Pow(1 + m * m, 0.5f));

        float n = (position[2].y - position[0].y) / (position[2].x - position[0].x);
        float deltax = (width / 2f) * (n / Mathf.Pow(n * n + 1, 0.5f));
        float deltay = (width / 2f) * (1 / Mathf.Pow(1 + n * n, 0.5f));

        Vector3[] offsets = new Vector3[4];
        offsets[0] = new Vector3(-deltax, deltay);
        offsets[1] = new Vector3(deltaX, -deltaY);//中
        offsets[2] = new Vector3(-deltaX, deltaY);//中
        offsets[3] = new Vector3(deltax, -deltay);

        List<Vector2> colliderPostion = new List<Vector2>
        {
            position[0] +  offsets[0],
            position[1] +  offsets[2],
            position[2] +  offsets[0],
            position[2] +  offsets[3],
            position[1] +  offsets[1],
            position[0] +  offsets[3],

        };
        return colliderPostion;
    }
}
