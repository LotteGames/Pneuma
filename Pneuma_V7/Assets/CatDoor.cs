using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatDoor : MonoBehaviour
{
    [Header("從左至右的變形")]
    public CatContrl.CatMorph LeftToRight;
    [Header("從右至左的變形")]
    public CatContrl.CatMorph RightToLeft;
    [Header("從左至右的變形提示圖")]
    public GameObject LeftToRightHint;
    [Header("從右至左的變形提示圖")]
    public GameObject RightToLeftHint;

    [Header("提示圖")]
    public Sprite[] Hints;


    GameObject Cat;

    [Header("靠近的提示圖")]
    public Sprite[] CloseHints;
    [Header("靠近的粒子特效")]
    public GameObject CloseOpen_Left;
    public GameObject CloseOpen_Right;
    [Header("靠近多近產生改變")]
    public float CloseRemove;
    private void Start()
    {
        Cat = GameObject.FindObjectOfType<CatContrl>().gameObject;

        if (LeftToRight == CatContrl.CatMorph.NoMorph)
        {
            LeftToRightHint.GetComponent<SpriteRenderer>().sprite = Hints[0];
        }
        if (LeftToRight == CatContrl.CatMorph.Long)
        {
            LeftToRightHint.GetComponent<SpriteRenderer>().sprite = Hints[1];
        }
        if (LeftToRight == CatContrl.CatMorph.Climb)
        {
            LeftToRightHint.GetComponent<SpriteRenderer>().sprite = Hints[2];
        }
        if (LeftToRight == CatContrl.CatMorph.Cloud)
        {
            LeftToRightHint.GetComponent<SpriteRenderer>().sprite = Hints[3];
        }

        if (RightToLeft == CatContrl.CatMorph.NoMorph)
        {
            RightToLeftHint.GetComponent<SpriteRenderer>().sprite = Hints[0];
        }
        if (RightToLeft == CatContrl.CatMorph.Long)
        {
            RightToLeftHint.GetComponent<SpriteRenderer>().sprite = Hints[1];
        }
        if (RightToLeft == CatContrl.CatMorph.Climb)
        {
            RightToLeftHint.GetComponent<SpriteRenderer>().sprite = Hints[2];
        }
        if (RightToLeft == CatContrl.CatMorph.Cloud)
        {
            RightToLeftHint.GetComponent<SpriteRenderer>().sprite = Hints[3];
        }
    }

    void CloseChange_Left()
    {
        if (LeftToRight == CatContrl.CatMorph.NoMorph)
        {
            LeftToRightHint.GetComponent<SpriteRenderer>().sprite = CloseHints[0];
        }
        if (LeftToRight == CatContrl.CatMorph.Long)
        {
            LeftToRightHint.GetComponent<SpriteRenderer>().sprite = CloseHints[1];
        }
        if (LeftToRight == CatContrl.CatMorph.Climb)
        {
            LeftToRightHint.GetComponent<SpriteRenderer>().sprite = CloseHints[2];
        }
        if (LeftToRight == CatContrl.CatMorph.Cloud)
        {
            LeftToRightHint.GetComponent<SpriteRenderer>().sprite = CloseHints[3];
        }


        if (RightToLeft == CatContrl.CatMorph.NoMorph)
        {
            RightToLeftHint.GetComponent<SpriteRenderer>().sprite = Hints[0];
        }
        if (RightToLeft == CatContrl.CatMorph.Long)
        {
            RightToLeftHint.GetComponent<SpriteRenderer>().sprite = Hints[1];
        }
        if (RightToLeft == CatContrl.CatMorph.Climb)
        {
            RightToLeftHint.GetComponent<SpriteRenderer>().sprite = Hints[2];
        }
        if (RightToLeft == CatContrl.CatMorph.Cloud)
        {
            RightToLeftHint.GetComponent<SpriteRenderer>().sprite = Hints[3];
        }
    }
    void CloseChange_Right()
    {
        if (LeftToRight == CatContrl.CatMorph.NoMorph)
        {
            LeftToRightHint.GetComponent<SpriteRenderer>().sprite = Hints[0];
        }
        if (LeftToRight == CatContrl.CatMorph.Long)
        {
            LeftToRightHint.GetComponent<SpriteRenderer>().sprite = Hints[1];
        }
        if (LeftToRight == CatContrl.CatMorph.Climb)
        {
            LeftToRightHint.GetComponent<SpriteRenderer>().sprite = Hints[2];
        }
        if (LeftToRight == CatContrl.CatMorph.Cloud)
        {
            LeftToRightHint.GetComponent<SpriteRenderer>().sprite = Hints[3];
        }


        if (RightToLeft == CatContrl.CatMorph.NoMorph)
        {
            RightToLeftHint.GetComponent<SpriteRenderer>().sprite = CloseHints[0];
        }
        if (RightToLeft == CatContrl.CatMorph.Long)
        {
            RightToLeftHint.GetComponent<SpriteRenderer>().sprite = CloseHints[1];
        }
        if (RightToLeft == CatContrl.CatMorph.Climb)
        {
            RightToLeftHint.GetComponent<SpriteRenderer>().sprite = CloseHints[2];
        }
        if (RightToLeft == CatContrl.CatMorph.Cloud)
        {
            RightToLeftHint.GetComponent<SpriteRenderer>().sprite = CloseHints[3];
        }
    }

    private void Update()
    {
        float Remove = Vector2.Distance(transform.position, Cat.transform.position);
        if (Remove <= CloseRemove)
        {
            if(Cat.transform.position.x < transform.position.x)
            {
                CloseChange_Left();
                CloseOpen_Left.SetActive(true);
                CloseOpen_Right.SetActive(false);
            }
            else if (Cat.transform.position.x >= transform.position.x)
            {
                CloseChange_Right();
                CloseOpen_Left.SetActive(false);
                CloseOpen_Right.SetActive(true);
            }
        }
        else
        {
            Start();
            CloseOpen_Left.SetActive(false);
            CloseOpen_Right.SetActive(false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "CatBody")
        {
            if(collision.GetComponent<CatContrl>() != null)
            {
                if(collision.transform.position.x >= transform.position.x)
                {
                    collision.GetComponent<CatContrl>().NowCatMorph = LeftToRight;
                    collision.GetComponent<Animator>().SetBool("Cloud", false);
                    if(collision.GetComponent<CatContrl>().NowCatMorph == CatContrl.CatMorph.Climb)
                    {
                        collision.GetComponent<Animator>().SetBool("Climb", true);
                        collision.GetComponent<SpriteRenderer>().sortingOrder = 0;
                        collision.GetComponent<CircleCollider2D>().radius = 0.67f;
                    }
                    else
                    {
                        collision.GetComponent<Animator>().SetBool("Climb", false);
                        collision.GetComponent<SpriteRenderer>().sortingOrder = 500;
                        collision.GetComponent<CircleCollider2D>().radius = 0.81f;
                    }
                }
                else
                {
                    collision.GetComponent<CatContrl>().NowCatMorph = RightToLeft;
                    collision.GetComponent<Animator>().SetBool("Cloud", false); 
                    if (collision.GetComponent<CatContrl>().NowCatMorph == CatContrl.CatMorph.Climb)
                    {
                        collision.GetComponent<Animator>().SetBool("Climb", true);
                        collision.GetComponent<SpriteRenderer>().sortingOrder = 0;
                        collision.GetComponent<CircleCollider2D>().radius = 0.67f;
                    }
                    else
                    {
                        collision.GetComponent<Animator>().SetBool("Climb", false);
                        collision.GetComponent<SpriteRenderer>().sortingOrder = 500;
                        collision.GetComponent<CircleCollider2D>().radius = 0.81f;
                    }
                }
            }
        }
    }
}
