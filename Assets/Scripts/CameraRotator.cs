﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour {

    public static int rotateDir = 0;
    public GameObject player;
    private bool rotating = false;
	public float smoothTime = 0.125f;

	void Update () {

		transform.position = Vector3.Lerp (transform.position, player.transform.position, smoothTime);

        /*if (Input.GetAxisRaw("Left Trigger") != 0)
        {
            if (leftTrigger == false)
            {
                // Call your event function here.
                leftTrigger = true;
            }
        }
        if (Input.GetAxisRaw("Fire1") == 0)
        {
            m_isAxisInUse = false;
        }*/

        if (!rotating)
        {

            //sets rotation when rotation key is pressed
            if ((Input.GetKey(KeyCode.Q) || (Input.GetAxis("Left Trigger")) == 1) && !(Input.GetKey(KeyCode.E) || (Input.GetAxis("Right Trigger")) == 1))
            {
                rotating = true;
                rotateDir += 90;
                StartCoroutine("RotatorTimer");
            }
            else if ((Input.GetKey(KeyCode.E) || (Input.GetAxis("Right Trigger")) == 1) && !(Input.GetKey(KeyCode.Q) || (Input.GetAxis("Left Trigger")) == 1)) {
                rotating = true;
                rotateDir -= 90;
                StartCoroutine("RotatorTimer");
            }

            //fixes "overrotation"
            if (rotateDir > 270)
                rotateDir = 0;
            else if (rotateDir < 0)
                rotateDir = 270;
        }

        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(1, rotateDir, 1), 5 * Time.deltaTime);

    }

    private IEnumerator RotatorTimer() {
        yield return new WaitForSeconds(.33f);
        rotating = false;
    }
}
