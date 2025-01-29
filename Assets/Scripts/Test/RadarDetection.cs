using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarDetection : MonoBehaviour
{
    public Transform player; // The player's transform
    public Transform rotatingObject; // The object that rotates around the player to cast raycasts
    public float radarRadius; // The radius of the radar in world units
    public float radarScreenRadius; // The radius of the radar on the screen (in pixels)
    public LayerMask targetLayer; // Layer mask for targets to appear on radar
    public RectTransform radarUI; // The UI element representing the radar
    public GameObject targetIconPrefab; // Prefab for the target icon
    public float heightOffset;

    private Dictionary<Transform, RectTransform> targetIcons = new Dictionary<Transform, RectTransform>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the object to cast raycasts in all directions
        rotatingObject.Rotate(Vector3.right, 360 * Time.deltaTime);

        // Detect targets in all directions using raycasts
        DetectTargets();
    }

    void DetectTargets()
    {
        RaycastHit hit;
        float angleStep = 360f / 360; // Assuming one raycast per degree

        for (float angle = 0; angle < 360; angle += angleStep)
        {
            Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward; // Only rotate on the Y axis
            Vector3 rayOrigin = rotatingObject.position + Vector3.up * heightOffset; // Raise the ray origin slightly to avoid ground collision

            if (Physics.Raycast(rayOrigin, direction, out hit, radarRadius, targetLayer))
            {
                Transform target = hit.transform;
                Vector3 screenPos = WorldToRadarPosition(target.position);
                if (IsOutOfRadar(screenPos))
                {
                    screenPos = SnapToRadarEdge(screenPos);
                }

                // Update or create the target's icon position on the radar UI
                UpdateTargetIconPosition(target, screenPos);
            }
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
        if (!targetIcons.ContainsKey(target))
        {
            // Instantiate a new target icon
            GameObject iconGO = Instantiate(targetIconPrefab, radarUI);
            RectTransform iconRect = iconGO.GetComponent<RectTransform>();
            targetIcons[target] = iconRect;
        }

        RectTransform targetIcon = targetIcons[target];
        targetIcon.anchoredPosition = new Vector2(screenPos.x, screenPos.z);
    }

    void LateUpdate()
    {
        // Clean up target icons for targets that are no longer detected
        List<Transform> toRemove = new List<Transform>();
        foreach (var kvp in targetIcons)
        {
            if (Vector3.Distance(player.position, kvp.Key.position) > radarRadius)
            {
                toRemove.Add(kvp.Key);
            }
        }
        foreach (var target in toRemove)
        {
            Destroy(targetIcons[target].gameObject);
            targetIcons.Remove(target);
        }
    }
}
