using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class WormRoll : MonoBehaviour
{
    public float moveSpeed = 2;

    public List<GameObject> pathPoints = new List<GameObject>();

    public Transform worm;

    public Animator animator;

    public SpriteRenderer sp;

    public Color lineColor = Color.yellow, pointColor = new Color(0, 1, 222f / 255, 1);

    public float waitDuration = 1;

    public Ease moveEase = Ease.Linear;
    private void OnDrawGizmos()
    {
        for (int i = 0; i < pathPoints.Count; i++)
        {
            if (pathPoints[i] != null)
            {
                Gizmos.color = pointColor;
                Gizmos.DrawSphere(pathPoints[i].transform.position, 0.1f);

                if (pathPoints[(i + 1 == pathPoints.Count) ? 0 : i + 1] != null)
                {
                    Gizmos.color = lineColor;
                    Gizmos.DrawLine(pathPoints[i].transform.position, pathPoints[(i + 1 == pathPoints.Count) ? 0 : i + 1].transform.position);
                }
            }
        }
    }

    void Start()
    {
        loopingRoutine = StartCoroutine(LoopMoving());

        sp.flipX = !sp.flipX;//這是為了讓在設定時能比較直覺，所以做的動作
        //沒有加這個的話，因為底下的loop，所以角色會變成倒退的移動
    }

    Tween tween;
    Coroutine loopingRoutine;
    IEnumerator LoopMoving()
    {
        for (int i = 0; i < pathPoints.Count; i++)
        {
            animator.SetBool("Walk", true);
            sp.flipX = !sp.flipX;

            Vector3 vector = pathPoints[i].transform.position - worm.position;

            float time = vector.magnitude / moveSpeed;

            tween = worm.DOMove(pathPoints[i].transform.position, time).SetEase(moveEase);

            yield return new WaitForSeconds(time);
            animator.SetBool("Turn",true);
            animator.SetBool("Walk", false);

            yield return new WaitForSeconds(waitDuration);
            animator.SetBool("Walk", true);
            animator.SetBool("Turn", false);
        }
        loopingRoutine = StartCoroutine(LoopMoving());
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        StopCoroutine(loopingRoutine);
        animator.SetTrigger("IsHit");
        tween.Kill();
    }


    public void ResetWorm()
    {
        animator.SetTrigger("IsReset");

        if (loopingRoutine != null)
        {
            StopCoroutine(loopingRoutine);
        }
        loopingRoutine = StartCoroutine(LoopMoving());
    }
    public void SetActive(bool value)
    {
        animator.SetBool("IsCatHere", value);
    }
}
