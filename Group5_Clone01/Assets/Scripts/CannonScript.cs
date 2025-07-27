using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CannonScript : MonoBehaviour
{
    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private float projectileSpeed;
    [SerializeField]
    private float SecondsToDestroy;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float secsBeforeFirstShot;
    [SerializeField]
    private float intervalBetweenShots;

    void Start()
    {
        
         InvokeRepeating("Fire", secsBeforeFirstShot, intervalBetweenShots);  
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            // Make this object look at the target's position
            transform.LookAt(target.position);
        }
        /*
        if (Input.GetKeyDown(KeyCode.Z))
        {

            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.velocity = firePoint.forward * projectileSpeed;
            Destroy(projectile, SecondsToDestroy);
        }
        */
    }

    void Fire()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = firePoint.forward * projectileSpeed;
        Destroy(projectile, SecondsToDestroy);
    }

}
