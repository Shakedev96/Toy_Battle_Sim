using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class TestGun : MonoBehaviour
{
    public Transform firePoint1;
    public Transform firePoint2;
    public bool isShooting;
    public GameObject bulletPrefab;

    public float shootForce, bulletSpeed;

    //public Vector3 lookAheadTarget;

    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {



        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
            isShooting = true;
        }
       /*  else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isShooting = false;
        } */



       /*   // Check if the "F" key is pressed
        if (Input.GetKeyDown(KeyCode.F))
        {
            isShooting = true;
            Debug.Log("Shooting true");
        }
        // Check if the "F" key is released
        if (Input.GetKeyUp(KeyCode.F))
        {
            isShooting = false;
            Debug.Log("No Shoot");
        }

        // If shooting is active and the fire rate interval has passed, shoot
        if (isShooting && Time.time > nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
        // The Magnitude of the planes current velocity gets updated and recorded for bullet trajectory projection
        currVel = rbPlane.velocity.magnitude; */
    }

    void Shoot()
    {
       RaycastHit hit;

       /*  Vector3 shootDirection1 = (lookAheadTarget.position - firePoint1.position).normalized;
        Vector3 shootDirection2 = (lookAheadTarget.position - firePoint2.position).normalized; */

      // For SPawner1

       Physics.Raycast(firePoint1.position , firePoint1.TransformDirection(Vector3.forward) , out hit , 100);
       {
            Debug.DrawRay(firePoint1.position , firePoint1.TransformDirection(Vector3.forward) * hit.distance , Color.red);

            Debug.Log("Raycast hit" + hit);

            //Instantiate(bulletPrefab,firePoint1.position, Quaternion.identity);
            
            
            //Instantiate(bulletPrefab,firePoint1.position, Quaternion.identity);
            Instantiate(bulletPrefab,firePoint1.position, firePoint1.rotation);

            //_bullet1.transform.Translate((hit.point - firePoint1.position).normalized * bulletSpeed);

            //_bullet1.GetComponent<Rigidbody>().AddForce(lookAheadTarget * shootForce, ForceMode.Impulse);

            //_bullet1.transform.position = Vector3.MoveTowards(firePoint1.position , lookAheadTarget.TransformDirection(0,0,1) , bulletSpeed * Time.deltaTime);

            Debug.DrawRay(firePoint1.position, (hit.point - firePoint1.position).normalized * 10f );
       }


        // For Spawner2

        
       Physics.Raycast(firePoint2.position , firePoint2.TransformDirection(Vector3.forward) , out hit , 100);
       {
            Debug.DrawRay(firePoint2.position , firePoint2.TransformDirection(Vector3.forward) * hit.distance , Color.magenta);

            //Instantiate(bulletPrefab,firePoint2.position, Quaternion.identity);
            Instantiate(bulletPrefab,firePoint2.position, firePoint2.rotation);
       }

    }
}



   
