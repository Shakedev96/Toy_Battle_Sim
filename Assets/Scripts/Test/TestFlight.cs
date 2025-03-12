using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFlight : MonoBehaviour
{
    [SerializeField] private float flySpeed = 5f;

    [SerializeField] private float yawAmount = 120f;

    [SerializeField] private float Yaw, Pitch, Roll;
    [SerializeField] private float pitchAMT, rollAMT;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.position += transform.forward * flySpeed * Time.deltaTime; // forward movement

        float horizontalInput = Input.GetAxis("Horizontal"); // left right, roll in project settings
        float verticalInput = Input.GetAxis("Vertical");

        // yaw,pitch and roll.
        Yaw += horizontalInput * yawAmount * Time.deltaTime; 
        Pitch = Mathf.Lerp(0 , pitchAMT , Mathf.Abs(verticalInput)) * Mathf.Sign(verticalInput);
        Roll = Mathf.Lerp(0, rollAMT , Mathf.Abs(horizontalInput)) * -Mathf.Sign(horizontalInput);
        

        transform.localRotation = Quaternion.Euler(Vector3.up * Yaw + Vector3.right * Pitch + Vector3.forward * Roll); //rotation of the plane



        
    }

    
}
