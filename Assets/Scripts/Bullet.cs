using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    [SerializeField] private float life = 3f;
    [SerializeField] public float shootSpeed;

    [SerializeField] const string _bogey = "Bogey";

    //[SerializeField] private Transform plane;
    
    private Rigidbody bulletRB;

    private void Start()
    {
        bulletRB = GetComponent<Rigidbody>();

        //bulletRB.AddForce(transform.forward * shootSpeed, ForceMode.Impulse);
        bulletRB.AddRelativeForce(Vector3.forward * shootSpeed, ForceMode.Impulse);
        //bulletRB.velocity = plane.transform.forward * shootSpeed;

        //bulletRB.AddForce(lookAhead.transform.position * shootSpeed, ForceMode.Impulse);
    }

    private void Awake()
    {
        Destroy(gameObject, life);
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag(_bogey))
        {
            Destroy(gameObject,1f);
        }
    }

}
