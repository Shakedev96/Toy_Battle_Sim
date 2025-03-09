using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDetectScript : MonoBehaviour
{
    [SerializeField] private float detectionRange = 10;
    public bool targetDetected;
    GunshipAI gunshipAI;
    private SphereCollider sphereCollider;
    [SerializeField] private LayerMask targetLayer;
    private static string playerTag = "Player";

    // Start is called before the first frame update
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        gunshipAI = GetComponentInParent<GunshipAI>();
        
    }

    // Update is called once per frame
    void Update()
    {
        EditRange();
    }
    
    void EditRange()
    {
        sphereCollider.radius = gunshipAI.detectRange;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == targetLayer || other.gameObject.CompareTag(playerTag))
        {
            targetDetected = true;
            Debug.Log("ITRUDER ALERT");
        }
        
        
    }

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == targetLayer || other.gameObject.CompareTag(playerTag))
        {
            targetDetected = true;
            Debug.Log("INTRUDER TO BE SHOT DOWN");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == targetLayer || other.gameObject.CompareTag(playerTag))
        {
            targetDetected = false;
            Debug.Log("TARGET HAS LEFT THE BUILDING");
        }
    }
}
