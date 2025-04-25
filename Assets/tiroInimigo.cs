using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tiroInimigo : MonoBehaviour
{
    public GameObject bulletInimigoPrefab;
    public Transform bulletInimigoSpawn;
    public float bulletInimigoVida = 2.0f;
    public float bulletInimigoSpeed = 6.0f;
    public float visao = 5.0f;
    public string inimigoTag = "Enemy";

    public float fireRate = 0.5f;
    private GameObject closestEnemy;
    private Transform alvo;
    private float nextFire = 0.0f;

    private bool isFiring = false;

    void Update()
    {
        closestEnemy = FindClosestEnemy();

        if (closestEnemy != null)
        {
            alvo = closestEnemy.transform;
            float distancia = Vector3.Distance(transform.position, alvo.position);

            if (distancia <= visao)
            {
                if (!isFiring)
                {
                    InvokeRepeating("FireInimigo", 0.2f, fireRate);
                    isFiring = true;
                }
            }
            else
            {
                CancelInvoke("FireInimigo");
                isFiring = false;
            }
        }
        else
        {
            CancelInvoke("FireInimigo");
            isFiring = false;
        }
    }

    void FireInimigo()
{
    if (Time.time < nextFire) return; // ← agora estamos usando
    nextFire = Time.time + fireRate;

    if (bulletInimigoPrefab == null || bulletInimigoSpawn == null) return;

    GameObject bullet = Instantiate(bulletInimigoPrefab, bulletInimigoSpawn.position, bulletInimigoSpawn.rotation);

    Rigidbody rb = bullet.GetComponent<Rigidbody>();
    if (rb != null)
    {
        rb.linearVelocity = bullet.transform.forward * bulletInimigoSpeed;
    }

    Destroy(bullet, bulletInimigoVida);
}

    GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(inimigoTag);
        GameObject closest = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            if (enemy == this.gameObject) continue; // Ignora a si mesmo

            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < shortestDistance)
            {
                shortestDistance = dist;
                closest = enemy;
            }
        }

        return closest;
    }




    void OnCollisionEnter(Collision collision)
    {
        // Verifique se o objeto com que colidiu tem a tag "Enemy"
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Destruir o NPC
            Destroy(collision.gameObject);

            // Opcional: destruir a própria bala após o impacto
            Destroy(gameObject);
        }
    }


    void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                // Destruir o NPC
                Destroy(other.gameObject);

                // Opcional: destruir a própria bala após o impacto
                Destroy(gameObject);
            }
        }



}
