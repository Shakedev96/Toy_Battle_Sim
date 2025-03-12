using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class Plane : MonoBehaviour
{

    [Header("Plane Settings")]

    [Tooltip("How much Throttle ramps up or down")]

    [SerializeField] public float ThrottleIncrement = 0.1f;

    [Tooltip("Max Engine Thrust when throttle at 100%")]

    [SerializeField] public float MaxThrust = 200f;

    [Tooltip("How Responsive is the plane when rolling, pitching and yawing")]

    [SerializeField] public float Responsiveness = 10f;


    [SerializeField, Space(5), Range(0, 10)]
    
     private float resetWaitTime = 1, waitTime;

    [Tooltip("How much lift is required to get the plane off the gorund.")]
    [SerializeField] private float _lift = 135f; // to check how much force required to lift the off the ground

    private float _throttle;// percentage of max engine thrust being used
    private float roll;// tilting left to right
    private float pitch;//tilting front to back
    private float yaw; // turning left to right

    [SerializeField] Transform propeller;

    [SerializeField] public float AirSpeed;

    private const string _groundLayer = "Ground";



    private float ResponseModifier //value used to tweak the resonsiveness of plane
    {
        get
        {
            return (RB.mass / 10f) * Responsiveness;
        }

    }

   
    Rigidbody RB;

    [Header("Plane Shoot Settings")]
    [SerializeField] TextMeshProUGUI hud;

    [SerializeField] GameObject aimCam;
    [SerializeField] public Image crosshair;
    public bool aimOn , isAiming;
    bool hasCollided = false;

    private void Awake()
    {
        RB = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        RB.useGravity = true;
        waitTime = 0;
    }

    private void HandleInput()
    {
        // roll = Input.GetAxis("Roll");
        // pitch = Input.GetAxis("Pitch");
        // yaw = Input.GetAxis("Yaw");


        // // Handling throttlevalue and clamping it bwt 0 -100

        // if (Input.GetKey(KeyCode.Space))
        // {
        //     _throttle += ThrottleIncrement;
        // }
        // else if(Input.GetKey(KeyCode.C))
        // {
        //     _throttle -= ThrottleIncrement;
        // }

        // _throttle = Mathf.Clamp(_throttle, 0f, 100f);


        // new logic
        roll = Input.GetAxis("Roll");
        pitch = -Input.GetAxis("Pitch"); // Inverted for realistic flight control
        yaw = Input.GetAxis("Yaw");

        if (Input.GetKey(KeyCode.Space))
        {
            _throttle += ThrottleIncrement;
        }
        else if(Input.GetKey(KeyCode.C))
        {
            _throttle -= ThrottleIncrement;
        }

        _throttle = Mathf.Clamp(_throttle, 0f, 100f);
    }

    void Update()
    {
        Aim();   
    }
   

    private void LateUpdate()
    {
        HandleInput();
        propeller.Rotate(Vector3.forward * _throttle);
        UpdateHUD();
    }

    private void FixedUpdate()
    {
        // Read from RB velocity
        AirSpeed = RB.velocity.magnitude;

        if (!hasCollided)
        {
            // Apply Forward Thrust
            RB.AddRelativeForce(MaxThrust * (_throttle / 100f) * Vector3.forward);

            // Apply Rotation Forces
            RB.AddTorque(ResponseModifier * yaw * transform.up);
            RB.AddTorque(pitch * ResponseModifier * transform.right);
            RB.AddTorque(ResponseModifier * roll * -transform.forward);

            // Apply Lift Force based on plane orientation
            Vector3 liftForce = transform.up * _lift * AirSpeed;
            RB.AddForce(liftForce);
        }
        else
        {
            RB.velocity = Vector3.zero;
            hasCollided = false;
        }

        //Aim();

        //Vector3 AirMovement = new(AirSpeed, AirSpeed);

        ////Write back into value
        //RB.velocity = AirMovement;
    }
            // FOR CHECKING COLLISIONS AGAINST OBJECTS
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer(_groundLayer))
        {
            RB.AddForce(MaxThrust * RB.mass * collision.GetContact(0).normal, ForceMode.Impulse);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(_groundLayer))
        {
            Debug.Log("fell down!!");
        }

        else
        {
            waitTime += Time.deltaTime;

            if (waitTime > resetWaitTime)
            {
                hasCollided = true;
                RB.AddForce(MaxThrust * RB.mass * collision.GetContact(0).normal, ForceMode.Impulse);
                waitTime = 0;
            }

            Debug.Log(collision.gameObject.name);
        }

        
    }
    private void UpdateHUD()
    {
        hud.text = "Throttle: " + _throttle.ToString("F0") + "%\n";
        hud.text += "Airspeed: " + (RB.velocity.magnitude * 3.6f).ToString("F0") + " Km/h\n";
        hud.text += "Altitude: " + transform.position.y.ToString("F0") + " m";
    }

    void Aim()
    {   
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
        aimOn = true;
        aimCam.SetActive(true);
        isAiming = true;
        }
        else if(Input.GetKeyUp(KeyCode.Mouse1))
        {
        aimOn = false;
        aimCam.SetActive(false);
        isAiming = false;
        }

    }

    /*
    private void HandleInput()
{
    roll = Input.GetAxis("Roll");
    pitch = -Input.GetAxis("Pitch"); // Inverted for realistic flight control
    yaw = Input.GetAxis("Yaw");

    if (Input.GetKey(KeyCode.Space))
    {
        _throttle += ThrottleIncrement;
    }
    else if(Input.GetKey(KeyCode.C))
    {
        _throttle -= ThrottleIncrement;
    }

    _throttle = Mathf.Clamp(_throttle, 0f, 100f);
}

private void FixedUpdate()
{
    
}

    */
}
