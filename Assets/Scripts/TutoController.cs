using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoController : MonoBehaviour {

    [SerializeField]
    private GameObject RightTrigger, LeftTrigger, LeftStick;
    private bool LT, RT, LStr, LStl, LSbr, LSbl;

    private float x, z;

    // Use this for initialization
    void Start () {
        LT = RT = LStr = LStl = LSbr = LSbl = false;
    }

    // Update is called once per frame
    void Update()
    {

        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        if (x != 0 && z != 0)
        {
            if (x > 0)
            {
                if (z > 0)
                    ;
                else if (z < 0)
                    ;
            }
            else if (x < 0)
            {
                if (z > 0)
                    ;
                else if (z < 0)
                    ;
            }
        }

        if (Input.GetAxis("Right Trigger") == 1) {
            RT = true;
        }
        if (Input.GetAxis("Left Trigger") == 1)
        {
            LT = true;
        }
    }
}
