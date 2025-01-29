using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Bullet_Behavioir : MonoBehaviour
{
    public float B_Speed;
    Rigidbody Clone_Rb;
    public GameObject bullet;
    public Transform GunPoint;
    void  Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            var B_clone = Instantiate(bullet,GunPoint.position,GunPoint.rotation);
            Clone_Rb = B_clone.GetComponent<Rigidbody>();

            Clone_Rb.velocity = GunPoint.forward * B_Speed * Time.deltaTime;
        }
    }
}
