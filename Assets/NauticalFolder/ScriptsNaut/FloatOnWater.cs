using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatOnWater : MonoBehaviour
{
    // public Porperties
    public float airDrag = 1;
    public float waterDrag = 10;
    public Transform[] floatPoints;


    //used components
    protected Rigidbody boatRB;
    protected Waves waves;


    //WaterLine
    protected float waterLine;
    protected Vector3[] waterLinePoints;


    // help Vectors
    protected Vector3 centerOffset;

    public Vector3 center {get {return transform.position + centerOffset; } }

    // Start is called before the first frame update
    void Awake()
    {
        waves = FindObjectOfType<Waves>();
        boatRB = GetComponent<Rigidbody>();
        boatRB.useGravity = false;


        //compute center
        waterLinePoints = new Vector3[floatPoints.Length];
        for(int i = 0; i < floatPoints.Length; i++)
        {
            waterLinePoints[i] = floatPoints[i].position;
        }
        centerOffset = PhysicsHelper.GetCenter(waterLinePoints) - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(floatPoints == null)
        {
            return;
        }
        for(int i = 0; i < floatPoints.Length; i++ )
        {
            if(floatPoints[i] == null)
            {
                continue;
            }
            if(waves != null)
            {
                //draw cube
                Gizmos.color = Color.green;
                Gizmos.DrawCube(waterLinePoints[i],Vector3.one * 0.3f);
            }

            //draw Sphere
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(floatPoints[i].position, 0.1f);
        }

        if(Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(new Vector3(center.x, waterLine,center.z), Vector3.one * 1f);
        }
    }
}
