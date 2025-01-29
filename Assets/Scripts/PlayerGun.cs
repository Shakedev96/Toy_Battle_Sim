using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] private Transform bulletSpawnPT;

    [SerializeField] GameObject bulletPrefab;

    [SerializeField] private float bulletSpeed = 10f;


//    [SerializeField] private float fireRate = 0.5f; // Time between each shot
    //[SerializeField] private float maxRange = 100f; // The maximum range for the raycast

    [SerializeField] private float nextFireTime = 0f;


    // Rigidbody RB;




    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && Time.time > nextFireTime)
        {
            Shoot();
            Debug.Log("Shooting");
        }
            

    }

    private void Shoot()
    {
        // Instantiating the bullet at the bulletSpawn point.Putting it in the bullet variable(data type GameObject)

        GameObject bullet = ObjectPooling.SharedInstance.GetPooledObject();
        if(bullet != null)
        {
            bullet.transform.position = bulletSpawnPT.position;
            bullet.transform.rotation = bulletSpawnPT.rotation;
            bullet.SetActive(true);
        }

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = bulletSpawnPT.forward * bulletSpeed;

        // Destroy the bullet after a certain time
        bullet.gameObject.SetActive(false);

        // Get the Rigidbody component of the bullet and set its velocity


        // Perform a raycast to detect hits
        //RaycastHit hit;
        //if (Physics.Raycast(bulletSpawnPT.position, bulletSpawnPT.forward, out hit, maxRange))
        //{
        //    Debug.Log("Hit: " + hit.collider.name);
        //    Rigidbody rb = bulletPrefab.GetComponent<Rigidbody>();
        //    rb.velocity = hit.transform.right * bulletSpeed;
        //    // Add any additional logic here, such as dealing damage to hit objects
        //}
    }
}


