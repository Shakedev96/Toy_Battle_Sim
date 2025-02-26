using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunshipAI : MonoBehaviour
{
    public enum GunShipState 
    {
        Idle,
        Aim,
        
    }
    [SerializeField] private GunShipState state ;
    [SerializeField] private Transform target, bulletSpawnOrigin1, bulletSpawnOrigin2, cannon1, cannon2;
    [SerializeField] private float trackSpeed, delayShoot, detectRange, bulletSpeed;
    //[SerializeField] private float minXRot = (180 - 360), maxXRot;
    [SerializeField] private GameObject enemyBulletPrefab;
    [SerializeField] private LayerMask targetLayer;

    [SerializeField] private bool detectTarget, aimTarget,shootTarget;
    
    void Start()
    {
        detectTarget = false;
        aimTarget = false;
        shootTarget= false;
    }
    void FixedUpdate()
    {
        UpdateState();
        Detect();

    }

    void UpdateState()
    {
        switch(state)
        {
            case GunShipState.Idle:
                delayShoot = 5;
                shootTarget = false;
                aimTarget = false;
                Debug.Log("In idle");
                if(detectTarget)
                {
                    state = GunShipState.Aim;
                    
                }
                else
                {
                    state = GunShipState.Idle;
                }
                break;

            case GunShipState.Aim:
            
                Aim();
                delayShoot -= Time.deltaTime;
                shootTarget = true;
                if(!detectTarget)
                {
                    state = GunShipState.Idle;
                    
                    
                }
                else if(delayShoot <= 0 && shootTarget)
                {
                    Shoot();
                }
                break;

            
                
        }
    }
    public void Detect()
    {
        
       detectTarget = Physics.CheckSphere(transform.position,detectRange,targetLayer);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = detectTarget ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }

    private void Aim()
    {
        aimTarget = true;
        
        Vector3 direction = target.position - transform.position; 

        if (direction.sqrMagnitude > 0.01f) 
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, trackSpeed * Time.deltaTime);
        }

    }

    void Shoot()
    {
        
        
        // for cannon1
        var enemybullet1 = Instantiate(enemyBulletPrefab,bulletSpawnOrigin1.position,bulletSpawnOrigin1.rotation);
        enemybullet1.GetComponent<Rigidbody>().velocity = bulletSpawnOrigin1.up * bulletSpeed;

        // for cannon2
        var enemybullet2 = Instantiate(enemyBulletPrefab, bulletSpawnOrigin2.position, bulletSpawnOrigin2.rotation);

        enemybullet2.GetComponent<Rigidbody>().velocity = bulletSpawnOrigin2.up * bulletSpeed;
    }

}



