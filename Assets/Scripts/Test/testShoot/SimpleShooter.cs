using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SimpleShoot : MonoBehaviour
{   
    [Header("Bullet Settings")]
    [SerializeField] private Transform spawnPt1;

    [SerializeField] private Transform spawnPt2;

    [SerializeField] float speedIncrement;

     public bool aimOn , isShooting;

    [SerializeField] GameObject bulletPreFab;

    [SerializeField] private float bulletSpeed;

    [Header("References of Objects")]
    
    [SerializeField] Plane playerPlane;
    [SerializeField] bool planeMove;

        // Start is called before the first frame update
    
    void Start()
    {
        playerPlane = FindObjectOfType<Plane>();
    }

    // Update is called once per frame
    void Update()
    {   
       if(playerPlane.AirSpeed >= 10)
       {
            planeMove = true;

       }
       else
       {
        planeMove = false;
       }

        if(Input.GetKeyDown(KeyCode.Mouse0) /* && (playerPlane.isAiming == true) */ )
        {
            Shoot();
            
        }
       
    }

    public void Shoot()
    {   
       isShooting = true;
       if(planeMove == true)
       {
            bulletSpeed = playerPlane.AirSpeed * speedIncrement;

       }
       else if (planeMove == false)
       {
             bulletSpeed *= speedIncrement;
       }
       
        if (bulletPreFab != null)
        {
            // Shoot from spawn point 1
            var bullet1 = Instantiate(bulletPreFab, spawnPt1.position, spawnPt1.rotation);
            if (bullet1.TryGetComponent<Rigidbody>(out var rb1))
            {
                rb1.velocity = spawnPt1.forward * bulletSpeed * Time.deltaTime;

                //rb1.velocity = rb1.AddForce()
            }
            else
            {
                Debug.LogError("Bullet prefab does not have a Rigidbody component.");
            }

            // Shoot from spawn point 2
            var bullet2 = Instantiate(bulletPreFab, spawnPt2.position, spawnPt2.rotation);
            if (bullet2.TryGetComponent<Rigidbody>(out var rb2))
            {
                rb2.velocity = spawnPt2.forward * bulletSpeed * Time.deltaTime;
            }
            else
            {
                Debug.LogError("Bullet prefab does not have a Rigidbody component.");
            }
        }
        else
        {
            Debug.LogError("Bullet prefab is not assigned.");
        }
    }


}

   

