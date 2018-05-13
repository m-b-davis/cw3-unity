using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator1 : MonoBehaviour {

	[SerializeField]
    int rotationSpeed;
	[SerializeField]
	private GameObject cameraRotator;
	[SerializeField]
	private GameObject mainCamera;
	[SerializeField]
	private int slerpSpeed;

    private bool camPosSaved = false;
    private Quaternion savedCamPos;
    private float oldMouseX, oldMouseY, newMouseX, newMouseY;
    private Vector3 cursorDelta, intoCube;

	// Use this for initialization
	void Start () {
        //savedCamPos = mainCamera.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
        //gets the position differential of the cursor between frames to calculate rotation of maze
        newMouseX = Input.mousePosition.x;
        newMouseY = Input.mousePosition.y;
        if (Input.GetMouseButton(0))
        {
            if (camPosSaved == false) {
                camPosSaved = true;
            }

			transform.rotation = transform.rotation * Quaternion.Euler (Time.deltaTime * rotationSpeed * -(newMouseY - oldMouseY), 0, 0);
			transform.rotation = Quaternion.Euler (0, Time.deltaTime * rotationSpeed * (newMouseX - oldMouseX), 0) * transform.rotation;
		} else {
            camPosSaved = false;
			transform.localRotation = Quaternion.Slerp (transform.localRotation, Quaternion.Euler (60, 0, 0), slerpSpeed * Time.deltaTime);
        }
        oldMouseX = newMouseX;
        oldMouseY = newMouseY;
    }


}
