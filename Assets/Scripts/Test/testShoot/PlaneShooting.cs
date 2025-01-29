using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaneShooting : MonoBehaviour
{

    [SerializeField] GameObject aimCam;
    [SerializeField] public Image crosshair;

    public bool aimOn;


    // Start is called before the first frame update
    void Start()
    {
        aimCam.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            PovShoot();
            aimOn = true;
        }
        else{
            aimCam.SetActive(false);
            crosshair.gameObject.SetActive(false);
            aimOn = false;
        }
    }

    private void PovShoot()
    {
        aimCam.SetActive(true);
        crosshair.gameObject.SetActive(true);
    }
}
