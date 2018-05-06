using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRotator : MonoBehaviour {

	[SerializeField]
	int slerpSpeed;
	[SerializeField]
	private int maxTilt;
    [SerializeField]
    private GameObject rotateAudioPrefab;
    private GameObject ball;
	private Quaternion currentFaceDirection = Quaternion.identity;
    private int viewDirection = 0;
    private float verticalTilt = 0;
    private float horizontalTilt = 0;
    private Rigidbody ballRigidbody;
    
    void Start() {
        ball = GameObject.FindGameObjectWithTag("Ball");
        ballRigidbody = ball.GetComponent<Rigidbody>();
    }

    void Update() {

        //gets value of view direction when rotating with Q and E (O-3)
        if (Input.GetKeyDown(KeyCode.Q)) {
            viewDirection--;
            Instantiate(rotateAudioPrefab, transform.position, transform.rotation);
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            viewDirection++;
            Instantiate(rotateAudioPrefab, transform.position, transform.rotation);
        }
        if (viewDirection > 3)
            viewDirection = 0;
        if (viewDirection < 0)
            viewDirection = 3;

        //sets the axies of tilt depending on the Y orientation of the maze (view direction)
        switch (viewDirection) {
            case 0:
                verticalTilt = Input.GetAxis("Vertical") * maxTilt;
                horizontalTilt = -Input.GetAxis("Horizontal") * maxTilt;
                break;
            case 1:
                verticalTilt = Input.GetAxis("Horizontal") * maxTilt;
                horizontalTilt = Input.GetAxis("Vertical") * maxTilt;
                break;
            case 2:
                verticalTilt = -Input.GetAxis("Vertical") * maxTilt;
                horizontalTilt = Input.GetAxis("Horizontal") * maxTilt;
                break;
            case 3:
                verticalTilt = -Input.GetAxis("Horizontal") * maxTilt;
                horizontalTilt = -Input.GetAxis("Vertical") * maxTilt;
                break;
        }

        //rotate from center when CheckFace triggered
        transform.localRotation = Quaternion.Slerp (transform.localRotation, currentFaceDirection, slerpSpeed * Time.deltaTime);
        //rotate from top using tilt values
        transform.parent.localRotation = Quaternion.Slerp (transform.parent.localRotation, Quaternion.Euler (verticalTilt, 90*viewDirection, horizontalTilt), slerpSpeed * Time.deltaTime);

	}

	public void SetCurrentFace (GameObject face) {
        ballRigidbody.useGravity = false;
        Invoke("DisableKinematicBall", 0.3f);
        if (face.CompareTag ("Right")) {
			currentFaceDirection = Quaternion.Euler (0, 0, 90) * currentFaceDirection;
		} else if (face.CompareTag ("Left")) {
			currentFaceDirection = Quaternion.Euler (0, 0, -90) * currentFaceDirection;
		} else if (face.CompareTag ("Front")) {
			currentFaceDirection = Quaternion.Euler (90, 0, 0) * currentFaceDirection;
		} else if (face.CompareTag ("Back")) {
			currentFaceDirection = Quaternion.Euler (-90, 0, 0) * currentFaceDirection;
		}
	}

    void DisableKinematicBall() {
        ballRigidbody.useGravity = true;
    }

}
