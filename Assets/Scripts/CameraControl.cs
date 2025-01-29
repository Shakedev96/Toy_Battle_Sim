using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour
{
    [Tooltip("An array of transforms that represent the camera position")]

    [SerializeField] private GameObject aimCam;
    [SerializeField] bool _aimCamOn = false;

    [SerializeField] private Vector3 aimCamOffset; // Offset from the cockpit transform

    [SerializeField] private Transform cockpitTransform; // The transform inside the cockpit where the aimCam should be posit
    [SerializeField] public Image crosshair;
    [SerializeField] private List<CinemachineVirtualCamera> _cams;

    // [Tooltip("The speed at which the camera follows the plane")]

    // [SerializeField] float speed;

    [SerializeField]
    private int index = 1;
    private Vector3 Target;
    [SerializeField]
    bool camSwitch = false;

    
    private void Start()
    {
        aimCam.SetActive(false);
        crosshair.gameObject.SetActive(false);


        _cams[0].m_Priority = _cams.Count;

        for (int i = 1; i < _cams.Count; i++)
        {
            _cams[i].Priority = _cams.Count - 1;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            PovCam();
            Debug.Log("Aim ON");
            _aimCamOn = true;

            if(_aimCamOn == true)
            {
                aimCam.transform.position = cockpitTransform.position + aimCamOffset;
                aimCam.transform.rotation = cockpitTransform.rotation;
            }
        }
        else if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            aimCam.SetActive(false);
            crosshair.gameObject.SetActive(false);
            _aimCamOn = false;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            camSwitch = true;
        }

        if (camSwitch)
        {
            index++;
            int relativeIndex = index % _cams.Count;
            _cams[relativeIndex].Priority += 2;
            Debug.Log("Cam Switch. Rel index at " + relativeIndex);
            camSwitch = false;
        }

        /* //if (_cams.Count >= 2)
        //{
        //    // setting the target to the relevant POV
        //    if (index % 2 == 0)
        //    {
                
        //    }

        //    else
        //    {
        //        _cams[1].Priority -= 2;
        //        _cams[0].Priority += 2;
        //        Debug.Log("Came Switch Back");
        //        camSwitch = false;
        //    }

        //    //switch (key)
        //    //{
        //    //    case KeyCode.P:
                    
                    
        //    //        break;

        //    //    case KeyCode.O:
                    
        //    //        break;
        //    //    case KeyCode.Alpha3:
        //    //        index = 2;
        //    //        break;
        //    //    case KeyCode.Alpha4:
        //    //        index = 3;
        //    //        break;

        //    //    default:
        //    //        break;
        //    //}
        //}

        

        //Target = Povs[index].position; */
    }
    private void PovCam()
    {
        aimCam.SetActive(true);
        crosshair.gameObject.SetActive(true);

        
    }

    private void FixedUpdate()
    {
        //moving the camera to the desired position/orientation, its in fixedupdate to avoid jitters.

        //transform.position = Vector3.MoveTowards(transform.position, Target, Time.deltaTime);
        //transform.position = Povs[index].forward;
    }
}
