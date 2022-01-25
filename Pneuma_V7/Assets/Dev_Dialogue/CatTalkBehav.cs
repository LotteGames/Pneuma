using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The TalkBehav for cat
/// </summary>
public class CatTalkBehav : MonoBehaviour
{
    public CatContrl catContrl;
    public Rigidbody2D catRb2d;
    public DialoguesManager dialoguesManager;
    public SpriteRenderer spR;
    private void Start()
    {
        dialoguesManager = FindObjectOfType<DialoguesManager>();
        catContrl = GetComponent<CatContrl>();
        catRb2d = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (dialoguesManager.SpeakingOne != null)
        {
            if (dialoguesManager.SpeakingOne.transform.position.x > transform.position.x)
            {
                spR.flipX = false;
            }
            else if (dialoguesManager.SpeakingOne.transform.position.x < transform.position.x)
            {
                spR.flipX = true;
            }

            catContrl.NowCatAct = CatContrl.CatAct.CatStop;
            catRb2d.velocity = new Vector2(0, catRb2d.velocity.y);
        }
        spR.flipX = false;
    }
    public void SetCatState()
    {
        catContrl.NowCatAct = CatContrl.CatAct.Idle;
    }
}
