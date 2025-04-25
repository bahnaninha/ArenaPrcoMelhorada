using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tiroBullet : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVida = 2.0f;
    public float bulletSpeed = 6.0f;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Fire();
        }
    }

    void Fire()
    {
        // Cria um Bullet a partir de BulletPrefab
        var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        // Adiciona velocidade a Bullet
        bullet.GetComponent<Rigidbody>().linearVelocity = bullet.transform.forward * bulletSpeed;

        // Destruir Bullet depois de n segundos
        Destroy(bullet, bulletVida);
    }
}
