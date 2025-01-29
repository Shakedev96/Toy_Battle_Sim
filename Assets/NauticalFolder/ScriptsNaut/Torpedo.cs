using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torpedo : MonoBehaviour
{
    [SerializeField] public float launchSpeed;
    private Rigidbody torpRB;

    private const string _bogeyLayer = "Bogey";

    [SerializeField] public ParticleSystem torpTrail;
    [SerializeField] public ParticleSystem torpExplode;
    
    // Start is called before the first frame update
    void Start()
    {
        torpRB = GetComponent<Rigidbody>();

        torpRB.AddForce(transform.up * launchSpeed,ForceMode.Impulse);
        torpTrail.Play();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer(_bogeyLayer))
        {
            torpTrail.Stop();
            
            Destroy(other.gameObject);

            torpExplode.Play();
            Destroy(gameObject);

            
        }
    }




}
