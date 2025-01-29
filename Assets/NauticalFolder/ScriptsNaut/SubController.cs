using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class SubController : MonoBehaviour
{

    [Header("Speed Settings")]
    [SerializeField] public float speedOffset;

    [SerializeField , Tooltip("Increases the top Speed of the Submarine going Forwards")] public float maxForwardSpeed;

    [SerializeField , Tooltip("Increases the top Speed of the Submarine goin Backwards")] public float maxBackwardSpeed;

    [SerializeField] private float currentSpeed;

    [SerializeField] private float minSpeed;

    [SerializeField, Tooltip("Speed at which the Submarine turns")] private float turnSpeed;

    [SerializeField] private float riseSpeed;


    [Header("Torpedo settings")]

    [SerializeField] private GameObject torpSpawnPT;

    [SerializeField] private GameObject torpPrefab;
    
   
    private Rigidbody playerRB;
    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();   
    }

    
    void FixedUpdate()
    {
        
        // Forward and Backward Movement of Submarine
        
        if(Input.GetKey(KeyCode.W))
        {
            currentSpeed += speedOffset;
        }
        else if(Input.GetKey(KeyCode.S))
        {
            currentSpeed -= speedOffset;
        }
        else if(Mathf.Abs(currentSpeed) <= minSpeed)
        {
            currentSpeed = 0;
        }

        currentSpeed = Mathf.Clamp(currentSpeed, -maxBackwardSpeed,maxForwardSpeed);
        playerRB.AddForce(transform.forward * currentSpeed);


        
        //For Turning the Sub
        if(Input.GetKey(KeyCode.D))
        {
            playerRB.AddTorque(transform.up * turnSpeed);
            Debug.Log("Turing Right");
        }

        

        else if(Input.GetKey(KeyCode.A))
        {
            playerRB.AddTorque(transform.up * -turnSpeed);
            Debug.Log("Turing Left");
            
        }
        

        //subROT = Mathf.Clamp(subROT, -turnRate, turnRate);
        
        //playerRB.AddTorque(Vector3.up * subROT);


        // FOR RISING AND LOWERING THE SUB

         if(Input.GetKey(KeyCode.LeftShift))
        {
            playerRB.AddForce(transform.up * riseSpeed);
            Debug.Log("Turing Right");
        }

        

        else if(Input.GetKey(KeyCode.LeftCommand))
        {
            playerRB.AddForce(transform.up * -riseSpeed);
            Debug.Log("Turing Left");
            
        }


    }
    
    // Update is called once per frame
    void Update()
    {
        LaunchTorp();// Shooting the torpedo
    }

    void LaunchTorp()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(torpPrefab, torpSpawnPT.transform.position, torpSpawnPT.transform.rotation);
        }
    }
}
