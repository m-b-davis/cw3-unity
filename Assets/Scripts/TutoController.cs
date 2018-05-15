using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoController : MonoBehaviour {

    [SerializeField]
    private GameObject LS, LSParent;
    private bool lsChecked = false;
	
	void Update () {
        if (!lsChecked)
            LSParent.transform.Rotate(new Vector3(0,0,2));
        if (Input.GetAxis("Horizontal") != 0 && Input.GetAxis("Vertical") != 0) {
            LS.transform.localScale = Vector3.one;
            LS.transform.localPosition = Vector3.zero;
            lsChecked = true;
        }
    }
}
