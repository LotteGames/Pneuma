using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormShoot : MonoBehaviour
{
    CatContrl cat;

    public GameObject bullet;

    public bool isDetectCat = false;

    public SpriteRenderer sp;

    public Transform shootPoint;

    public Animator animator;

    void Start()
    {
        cat = FindObjectOfType<CatContrl>();
    }

    public void Shoot()
    {
        Vector3 vector = shootPoint.position - transform.position;

        float degree = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;

        BulletInfo bulletInfo = Instantiate(bullet, shootPoint.position, Quaternion.identity).GetComponent<BulletInfo>();

        bulletInfo.SetDir(degree);

        bulletInfo.SetVelocity(vector);
    }

    public void MoveDir()
    {
        if (isDetectCat)
        {
            Vector3 vector = cat.transform.position - transform.position;

            float degree = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, degree);

            animator.SetTrigger("IsShoot");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CatContrl>())
        {
            animator.SetTrigger("IsHit");
        }
    }

    public void ResetWorm()
    {
        animator.SetTrigger("IsReset");
    }

    public void SetActive(bool value)
    {
        animator.SetBool("IsCatHere", value);
    }
}
