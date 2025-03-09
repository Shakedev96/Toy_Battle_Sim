using System.Collections;
using UnityEngine;

public class GunshipAI : MonoBehaviour
{
    public enum GunShipState
    {
        Idle,
        Aim
        
    }

    private Animator anim;
    [Header("Gunship States")]
    [SerializeField] private GunShipState state;
    [SerializeField] private Transform target, bulletSpawnOrigin1, bulletSpawnOrigin2;
    public float trackSpeed = 5f, detectRange = 15f, bulletSpeed = 20f;
    [SerializeField] private float delayShoot = 5f, timeBetweenShots = 0.5f; // Shooting delays
    [SerializeField] private GameObject enemyBulletPrefab;
    [SerializeField] private LayerMask targetLayer;

    [Header("Detection")]
    public AIDetectScript detectScript;
    [SerializeField] private bool detectTargets;

    [Header("Cannon/Shooting")]
    public bool fireFromFirstCannon = true; // Track which cannon fires next
    private Coroutine shootingCoroutine;
    //[SerializeField] private Transform Cannon1;
    //[SerializeField] private float recoil = 0.2f;
    //[SerializeField] private Vector3 ogCannonPos;


    void Awake()
    {
        //ogCannonPos = Cannon1.position;
    }
    void Start()
    {
        state = GunShipState.Idle;
        detectScript = GetComponentInChildren<AIDetectScript>();
        anim = GetComponent<Animator>();
        
    }

    void FixedUpdate()
    {
        UpdateState();
    }


    private void UpdateState()
    {
        switch (state)
        {
            case GunShipState.Idle:
                if (detectScript.targetDetected)
                {
                    state = GunShipState.Aim;
                }
                break;

            case GunShipState.Aim:
                AimAtTarget();
                delayShoot -= Time.deltaTime;

                if (!detectScript.targetDetected)
                {
                    StopShooting();
                    state = GunShipState.Idle;
                    
                }
                else if (delayShoot <= 0 )//&& )
                {
                    //state = GunShipState.Shooting;
                    StartShooting();
                }
                break;

            // case GunShipState.Shooting:
            //     if (!detectTarget)
            //     {
            //         state = GunShipState.Idle;
            //         StopShooting();
            //     }
            //     break;
        }
    }

    private void AimAtTarget()
    {
        if (target == null) return;

        Vector3 direction = target.position - transform.position;
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, trackSpeed * Time.deltaTime);
        }

        
    }

    private void StartShooting()
    {
        if (shootingCoroutine == null)
        {
            shootingCoroutine = StartCoroutine(FireWithDelay());
        }
    }

    private void StopShooting()
    {
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
        }
        delayShoot = 5f; // Reset delay shoot time when stopping
    }

    private IEnumerator FireWithDelay()
    {
        while (detectScript.targetDetected)
        {
            Transform firePoint = fireFromFirstCannon ? bulletSpawnOrigin1 : bulletSpawnOrigin2;
            string cannonAnim = fireFromFirstCannon ? "Cannon1" : "Cannon2";

             // Play the recoil animation
            anim.SetTrigger(cannonAnim);
            

            GameObject bullet =  Instantiate(enemyBulletPrefab, firePoint.position, firePoint.rotation); 
            // for object pooling
            //GameObject bullet = ObjectPooling.SharedInstance.GetPooledObject();
            // if(bullet != null)
            // {
            //     bullet.transform.position = firePoint.transform.position;
            //     bullet.transform.rotation = firePoint.transform.rotation;
            //     bullet.SetActive(true);
            // }
            bullet.GetComponent<Rigidbody>().velocity = firePoint.up * bulletSpeed;
            //bullet.GetComponent<Rigidbody>().AddForce(Vector3.forward * bulletSpeed, ForceMode.Impulse); // another method using rigidbody, does not work up to the mark.

            fireFromFirstCannon = !fireFromFirstCannon; // Alternate cannons

            yield return new WaitForSeconds(timeBetweenShots);
        }

        shootingCoroutine = null; // Reset coroutine reference when stopped
    }

    // void CannonMove()
    // {
    //     Vector3 targetPos = Cannon1.position - (Cannon1.forward * recoil);
    //     if(fireFromFirstCannon)
    //     {
    //         Cannon1.position = Vector3.Lerp(ogCannonPos, targetPos,timeBetweenShots);
    //     }
    //     else
    //     {
    //         Cannon1.position = ogCannonPos;
    //     }
    // }
    
}

/*
void CannonMove()
{
    if (fireFromFirstCannon)
    {
        StartCoroutine(MoveCannonBack());
    }
}

private IEnumerator MoveCannonBack()
{
    Vector3 targetPos = ogCannonPos - Cannon1.forward * recoil;
    float elapsedTime = 0f;
    float duration = 0.1f; // Adjust for smoother recoil effect

    // Move the cannon back (recoil effect)
    while (elapsedTime < duration)
    {
        Cannon1.position = Vector3.Lerp(ogCannonPos, targetPos, elapsedTime / duration);
        elapsedTime += Time.deltaTime;
        yield return null;
    }
    Cannon1.position = targetPos;

    // Wait before returning to original position
    yield return new WaitForSeconds(0.1f);

    // Move cannon back to its original position
    elapsedTime = 0f;
    while (elapsedTime < duration)
    {
        Cannon1.position = Vector3.Lerp(targetPos, ogCannonPos, elapsedTime / duration);
        elapsedTime += Time.deltaTime;
        yield return null;
    }
    Cannon1.position = ogCannonPos;
}


*/
