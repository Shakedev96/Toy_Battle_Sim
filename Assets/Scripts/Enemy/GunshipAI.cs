using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunshipAI : MonoBehaviour
{
    public enum GunShipState 
    {
        Idle,
        Aim,
        Shoot
    }

    [SerializeField] private GunShipState state ;
    [SerializeField] private Transform target;
    [SerializeField] private float trackSpeed, delayShoot, detectRange;
    //[SerializeField] private float minXRot = (180 - 360), maxXRot;
    [SerializeField] private LayerMask targetLayer;

    [SerializeField] private bool stateChange, detectTarget, aimTarget,shootTarget;
    
    void Start()
    {
        detectTarget = false;
        aimTarget = false;
        shootTarget= false;
    }
    void Update()
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
                aimTarget = false;
                shootTarget = false;
                detectTarget = false;
                if(detectTarget)
                {
                    state = GunShipState.Aim;
                    delayShoot --;
                }
                break;

            case GunShipState.Aim:
                Aim();
                if(!detectTarget)
                {
                    state = GunShipState.Idle;
                }
                else if(delayShoot == 0)
                {
                    state = GunShipState.Shoot;
                }
                break;

            case GunShipState.Shoot:
                Shoot();
                break;
        }
    }
    public bool Detect()
    {
        
        detectTarget = false;

        // Check if any colliders within the sphere overlap with the target layer
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectRange, targetLayer);
        
        if (hitColliders.Length > 0)
        {
            Debug.Log("Detected: " + hitColliders[0].name);
            detectTarget = true;
            return true;
        }
        
        return false;
        

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = detectTarget ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }

    private void Aim()
    {
        aimTarget = true;
        
        Vector3 direction = target.position - transform.position; // Calculate direction to the target

        if (direction.sqrMagnitude > 0.01f) // Prevent jittering if the target is too close
        {
            // Calculate the target rotation that looks in the direction of the target
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        

            // Smoothly rotate the sphere towards the target
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, trackSpeed * Time.deltaTime);
        }



    }
    private void Shoot()
    {
        shootTarget = true;
    }
}



