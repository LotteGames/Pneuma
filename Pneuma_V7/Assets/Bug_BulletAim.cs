using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug_BulletAim : MonoBehaviour
{
    [Header("貓咪所在地")]
    public GameObject Cat;

    [Header("子彈的出發點")]
    public GameObject Pos;

    [Header("要發射的子彈")]
    public GameObject Bullet;

    [Header("發射的力道")]
    public float ShootPower;

    [Header("發射冷卻時間")]
    public float Shoot_CD;

    [Header("多接近會攻擊")]
    public float ShootRemove;

    float T;
    // Start is called before the first frame update
    void Start()
    {
        Cat = GameObject.FindObjectOfType<CatContrl>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        T += Time.deltaTime;

        float Remove = transform.position.y - Cat.transform.position.y;



        if (Remove >= 0 && Remove <= ShootRemove)
        {
            if (T >= Shoot_CD)
            {
                GetComponent<Animator>().SetBool("Attack", true);
                T = 0;
            }
        }
    
    }

    public void Shoot()
    {
        GetComponent<Animator>().SetBool("Attack", false);

        float Remove = transform.position.y - Cat.transform.position.y;
        if (Remove >= -3 && Remove <= ShootRemove * 2f)
        {
            Vector2 Path = Cat.transform.position - transform.position;
            Path.Normalize();

            GameObject B = Instantiate(Bullet, Pos.transform.position, Quaternion.Euler(0, 0, 0));
            B.GetComponent<Rigidbody2D>().AddRelativeForce(Path * ShootPower);
        }
    }
}
