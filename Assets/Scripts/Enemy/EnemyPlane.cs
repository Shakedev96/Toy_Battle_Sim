using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlane : MonoBehaviour
{
    /* public float speed = 0f;
    private Rigidbody enemyRB;

    [SerializeField] private float _lift = 135f; // to check how much force required to lift the off the ground

    private float _throttle;// percentage of max engine thrust being used
    private float roll;// tilting left to right
    private float pitch;//tilting front to back
    private float yaw; // turning left to right

    [SerializeField] Transform propeller;

     private const string _groundLayer = "Ground";
    // Start is called before the first frame update
    void Start()
    {
        enemyRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // At rest
    /* 
    1. do nothing till the player collides with the patrol collider.
     */



    // During Patrol
    /*
    1. proppeler on
    2. take off from runway
    3. circle a given area 
    4. switch to detect when player reaches a certain distance from the patrol area
    or when the player collides with the detect collider.
     */




    // Detect Player 
    /* 
    1. Find the Player
    2. if player comes withing a certain area switch to attack.    
     */



    // Attack Player

    [Header("Plane Settings")]
    [SerializeField] private float throttleIncrement = 0.1f;
    [SerializeField] private float maxThrust = 200f;
    [SerializeField] private float responsiveness = 10f;
    [SerializeField] private float lift = 135f;
    [SerializeField] private Transform propeller;

    [SerializeField, Range(0, 10)]
    private float patrolRadius = 500f;
    [SerializeField] private float patrolSpeed = 50f;

    private float throttle;
    private float roll;
    private float pitch;
    private float yaw;

    private Rigidbody rb;
    private Vector3 patrolCenter;
    private bool isTakingOff = false;
    private bool isPatrolling = false;
    private float takeOffAltitude = 50f;

    private float ResponseModifier
    {
        get
        {
            return (rb.mass / 10f) * responsiveness;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        patrolCenter = transform.position;
        StartCoroutine(TakeOffSequence());
    }

    private IEnumerator TakeOffSequence()
    {
        // Gradually increase throttle to take off
        isTakingOff = true;
        throttle = 0f;

        while (transform.position.y < takeOffAltitude)
        {
            throttle += throttleIncrement;
            throttle = Mathf.Clamp(throttle, 0f, 100f);
            yield return new WaitForSeconds(0.1f);
        }

        // Start patrolling once takeoff is done
        isTakingOff = false;
        isPatrolling = true;
    }

    private void Update()
    {
        if (isPatrolling)
        {
            Patrol();
        }
    }

    private void FixedUpdate()
    {
        // Read from RB velocity
        if (isTakingOff || isPatrolling)
        {
            rb.AddRelativeForce(maxThrust * throttle * transform.forward);

            rb.AddTorque(ResponseModifier * yaw * transform.up);
            rb.AddTorque(pitch * ResponseModifier * transform.right);
            rb.AddTorque(ResponseModifier * roll * -transform.forward);

            rb.AddRelativeForce(lift * rb.velocity.magnitude * Vector3.up);
        }

        propeller.Rotate(Vector3.forward * throttle);
    }

    private void Patrol()
    {
        // Simple patrolling in a circular area around a central point (patrolCenter)
        Vector3 directionToCenter = patrolCenter - transform.position;
        float distanceToCenter = directionToCenter.magnitude;

        // Adjust yaw to keep the plane circling around the patrol center
        yaw = Mathf.Clamp(Vector3.Cross(transform.forward, directionToCenter.normalized).y, -1f, 1f);

        // Adjust roll to maintain stable circular movement
        roll = Mathf.Clamp(-yaw, -1f, 1f);

        // Set constant pitch to maintain patrol speed
        pitch = 0.2f;

        // Keep the plane within a certain patrol radius
        if (distanceToCenter > patrolRadius)
        {
            pitch = 0.5f;  // Steepen the pitch if the plane is going too far from the center
        }

        // Set throttle to patrol speed
        throttle = patrolSpeed / maxThrust * 100f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Reset velocity if it collides with anything
        rb.velocity = Vector3.zero;
    }
}
    

