using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastGun : MonoBehaviour
{

    
    public float damage;
    public float range;

    public GameObject gunObject;

     public float rayDisplayTime = 0.5f; // Duration for which the ray will be visible
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            PewPew();
        }
    }

    void PewPew()
    {

        RaycastHit hit;
        Vector3 rayOrigin = gunObject.transform.position;
        Vector3 rayDirection = gunObject.transform.forward;


        if (Physics.Raycast(rayOrigin, rayDirection, out hit, range))
        {
            Debug.Log(hit.transform.name);
            StartCoroutine(ShowRay(rayOrigin, rayDirection * hit.distance));
        }
        else
        {
            StartCoroutine(ShowRay(rayOrigin, rayDirection * range));
        }
    }

    IEnumerator ShowRay(Vector3 origin, Vector3 direction)
    {
        float startTime = Time.time;
        while (Time.time < startTime + rayDisplayTime)
        {
            Debug.DrawRay(origin, direction, Color.red);
            yield return null;
        }
    }
}
