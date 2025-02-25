using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunshipAI : MonoBehaviour
{
    [SerializeField] private Transform target;
    private Vector3 lastPos;
    [SerializeField] private float trackSpeed;
    [SerializeField] private float minXRot,maxXRot,minYRot,maxYRot;
    // Start is called before the first frame update
    void Start()
    {
        lastPos = target.position;
    }

    // Update is called once per frame
    void Update()
    {
        Track();
    }

    private void Track()
    {
        Vector3 direction = target.position - lastPos;
        direction.z = 0;// removing tracking along the z axis

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        Vector3 targetRot = targetRotation.eulerAngles;

        targetRot.x = Mathf.Clamp(targetRot.x, minXRot, maxXRot);
        targetRot.y = Mathf.Clamp(targetRot.y, minYRot, maxYRot);
        Quaternion clampedRotation = Quaternion.Euler(targetRot);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, clampedRotation, trackSpeed * Time.deltaTime);
        


    }
}
/* 
states for the gunship
1.idle/rest
2. detect
3. track or track and shoot.
4. shoot
*/