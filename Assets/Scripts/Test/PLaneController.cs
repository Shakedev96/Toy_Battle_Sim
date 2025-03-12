using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PLaneController : MonoBehaviour
{
    [Header("Plane Settings")]
    [SerializeField] public float ThrottleIncrement = 0.1f;
    [SerializeField] public float MaxThrust = 5000f;
    [SerializeField] public float Responsiveness = 10f;
    [SerializeField] private float _lift = 135f;
    [SerializeField] private float drag = 0.01f;
    [SerializeField] private float angularDrag = 0.5f;

    [Header("Ground Check Settings")]
    
    [SerializeField] private LayerMask groundLayer; // Assign a layer for the ground

    [SerializeField] private Transform propeller;
    [SerializeField] private TextMeshProUGUI hud;

    private float _throttle;
    private float _targetThrottle;
    private float roll, pitch, yaw;
    private float smoothness = 5f; 
    public bool isGrounded;

    private Rigidbody RB;

    private float ResponseModifier => (RB.mass / 10f) * Responsiveness;

    private void Awake()
    {
        RB = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        RB.useGravity = true;
        RB.drag = drag;
        RB.angularDrag = angularDrag;
    }

    private void HandleInput()
    {
        _throttle = Mathf.Lerp(_throttle, _targetThrottle, Time.deltaTime * smoothness);

        roll = Mathf.Lerp(roll, Input.GetAxis("Roll"), Time.deltaTime * smoothness);
        pitch = Mathf.Lerp(pitch, -Input.GetAxis("Pitch"), Time.deltaTime * smoothness); 
        yaw = Mathf.Lerp(yaw, Input.GetAxis("Yaw"), Time.deltaTime * smoothness);

        if (Input.GetKey(KeyCode.Space))
        {
            _targetThrottle += ThrottleIncrement * Time.deltaTime * 10f;
        }
        else if (Input.GetKey(KeyCode.C))
        {
            _targetThrottle -= ThrottleIncrement * Time.deltaTime * 10f;
        }

        _targetThrottle = Mathf.Clamp(_targetThrottle, 0f, 100f);
    }

    private void FixedUpdate()
    {
        ApplyPhysics();
    }

    private void ApplyPhysics()
    {
        // Check if the plane is grounded using CheckSphere
        isGrounded = Physics.Raycast(transform.position, -transform.up, 1f,groundLayer );

        // Apply thrust if throttle is greater than zero
        if (_throttle > 0)
        {
            float currentThrust = MaxThrust * (_throttle / 100f);
            RB.AddRelativeForce(Vector3.forward * currentThrust);
        }

        // Apply rotation forces
        RB.AddTorque(ResponseModifier * yaw * transform.up);
        RB.AddTorque(ResponseModifier * pitch * transform.right);
        RB.AddTorque(ResponseModifier * roll * -transform.forward);

        // Apply lift force only if the plane is NOT grounded
        if (!isGrounded)
        {
            Vector3 liftForce = transform.up * _lift * RB.velocity.magnitude;
            RB.AddForce(liftForce);
        }

        // Rotate the propeller
        propeller.Rotate(Vector3.forward * _throttle * 10f);
    }

    private void Update()
    {
        HandleInput();
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        hud.text = "Throttle: " + _throttle.ToString("F0") + "%\n";
        hud.text += "Airspeed: " + (RB.velocity.magnitude * 3.6f).ToString("F0") + " Km/h\n";
        hud.text += "Altitude: " + transform.position.y.ToString("F0") + " m";
    }
}
