using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug_Bullet : MonoBehaviour
{
    [Header("子彈的出發點")]
    public GameObject Pos;

    [Header("要發射的子彈")]
    public GameObject Bullet;

    [Header("發射的力道")]
    public float ShootPower;

    [Header("發射的時間(最大最小隨機)")]
    public float ShootTimeMin, ShootTimeMax;
    [Header("發射的時間")]
    public float ShootTime;

    [Header("可以發射")]
    public bool CanShoot;
    float T;
    // Start is called before the first frame update
    void Start()
    {
        CanShoot = false;
        ShootTime = Random.Range(ShootTimeMin, ShootTimeMax);
    }

    // Update is called once per frame
    void Update()
    {
        if(CanShoot)
        T += Time.deltaTime;
        if (T >= ShootTime)
        {
            GameObject B = Instantiate(Bullet, Pos.transform.position, Pos.transform.rotation);
            B.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(ShootPower, 0));
            ShootTime = Random.Range(ShootTimeMin, ShootTimeMax);
            T = 0;
        }
    }

    public void Shoot()
    {
        GameObject B = Instantiate(Bullet, Pos.transform.position, Pos.transform.rotation);
        B.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(ShootPower, 0));
    }
    public void YouCanShoot()
    {
        CanShoot = true;
    }
    public void YouCantShoot()
    {
        CanShoot = false;
    }
}
