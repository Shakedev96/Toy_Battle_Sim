using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{

    public RawImage compImage;

    public Transform player;
    // public Transform playerTransform;
    // Vector3 dir;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        compImage.uvRect = new Rect (player.localEulerAngles.y/360f,0f,1f,1f);
        // dir.z = playerTransform.eulerAngles.y;
        // transform.localEulerAngles = dir;
    }
}
