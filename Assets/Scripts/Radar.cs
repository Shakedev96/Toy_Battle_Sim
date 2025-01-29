using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Radar : MonoBehaviour
{

    
    public Transform player; // The player's transform
    public float radarRadius; // The radius of the radar in world units
    public float radarScreenRadius ; // The radius of the radar on the screen (in pixels)
    public LayerMask targetLayer; // Layer mask for targets to appear on radar
    public RectTransform radarUI; // The UI element representing the radar

    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Find all targets within the radar range
        Collider[] targets = Physics.OverlapSphere(player.position, radarRadius, targetLayer);

        foreach (Collider targetCollider in targets)
        {
            Transform target = targetCollider.transform;
            Vector3 screenPos = WorldToRadarPosition(target.position);
            if (IsOutOfRadar(screenPos))
            {
                screenPos = SnapToRadarEdge(screenPos);
            }

            // Update the target's icon position on the radar UI
            UpdateTargetIconPosition(target, screenPos);
        }
    }
    Vector3 WorldToRadarPosition(Vector3 worldPos)
    {
        Vector3 direction = worldPos - player.position;
        direction.y = 0; // Ignore vertical difference
        direction = player.InverseTransformDirection(direction);
        direction.Normalize();
        return direction * radarScreenRadius;


    }

    bool IsOutOfRadar(Vector3 screenPos)
    {
        return screenPos.magnitude > radarScreenRadius;
    }

    Vector3 SnapToRadarEdge(Vector3 screenPos)
    {
        return screenPos.normalized * radarScreenRadius;
    }



    void UpdateTargetIconPosition(Transform target, Vector3 screenPos)
    {
        // Assuming the target icon is a child of the target
        RectTransform targetIcon = target.GetComponentInChildren<RectTransform>();
        if (targetIcon != null)
        {
            targetIcon.anchoredPosition = new Vector2(screenPos.x, screenPos.z);
        }
    }



}

    

     

   

/*

        public Transform player; // The player's transform
    public float radarRadius = 100f; // The radius of the radar in world units
    public float radarScreenRadius = 100f; // The radius of the radar on the screen (in pixels)
    public LayerMask targetLayer; // Layer mask for targets to appear on radar
    public RectTransform radarUI; // The UI element representing the radar

    void Update()
    {
        // Find all targets within the radar range
        Collider[] targets = Physics.OverlapSphere(player.position, radarRadius, targetLayer);

        foreach (Transform target in targets)
        {
            Vector3 screenPos = WorldToRadarPosition(target.position);
            if (IsOutOfRadar(screenPos))
            {
                screenPos = SnapToRadarEdge(screenPos);
            }

            // Update the target's icon position on the radar UI
            UpdateTargetIconPosition(target, screenPos);
        }
    }

    Vector3 WorldToRadarPosition(Vector3 worldPos)
    {
        Vector3 direction = worldPos - player.position;
        direction.y = 0; // Ignore vertical difference
        direction = player.InverseTransformDirection(direction);
        direction.Normalize();
        return direction * radarScreenRadius;
    }

    bool IsOutOfRadar(Vector3 screenPos)
    {
        return screenPos.magnitude > radarScreenRadius;
    }

    Vector3 SnapToRadarEdge(Vector3 screenPos)
    {
        return screenPos.normalized * radarScreenRadius;
    }

    void UpdateTargetIconPosition(Transform target, Vector3 screenPos)
    {
        // Assuming the target icon is a child of the target
        RectTransform targetIcon = target.GetComponentInChildren<RectTransform>();
        if (targetIcon != null)
        {
            targetIcon.anchoredPosition = new Vector2(screenPos.x, screenPos.z);
        }
    

*/
