using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCrashWall : MonoBehaviour
{
    [SerializeField] KeyCode CrashToggle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(CrashToggle))
        {
            this.gameObject.SetActive(false);
        }
        else if (Input.GetKey(CrashToggle))
        {
            this.gameObject.SetActive(true);
        }
    }
}
