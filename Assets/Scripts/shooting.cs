using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting : MonoBehaviour
{

    [Header("Attributes")]

    public float shootRange = 15f;
    public float shootingRate = 1f;
    public float shootCountdown = 0f;

    public GameObject bulletPrefab;
    public Transform shootPoint;

    private Transform target;
    

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("updateTarget", 0f, 0.5f);
    }

    void updateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach(GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if(distanceToEnemy <= shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }

            if (nearestEnemy != null && shortestDistance <= shootRange)
            {
                target = nearestEnemy.transform;
            }
            else
            {
                target = null;
            }


        }



    }

    // Update is called once per frame
    void Update()
    {
        
        if (target == null)
            return;

        if (shootCountdown <= 0f)
        {
            transform.LookAt(target);
            shoot();
            shootCountdown = 1f / shootingRate;
        }

        shootCountdown -= Time.deltaTime;

    }

    void shoot()
    {
      
        GameObject bulletGo =  (GameObject)Instantiate(bulletPrefab, shootPoint.position,shootPoint.rotation);
        bullet bullet = bulletGo.GetComponent<bullet>();

        if (bullet != null)
            bullet.Seek(target);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootRange);
    }



}
