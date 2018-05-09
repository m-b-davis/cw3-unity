using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

	public float MoveSpeed;
	public float RotateSpeed;

	private bool isGrounded = false;
	private Rigidbody rigidBody;

	public Vector3 jump;
	public float jumpForce = 10.0f;

	private Vector3 Direction;

	// Use this for initialization

	void Start () {
		this.Spawn();	
		this.rigidBody = GetComponent<Rigidbody> ();
		this.rigidBody.freezeRotation = true;
		this.jump = new Vector3 (0.0f, this.jumpForce, 0);
	}



	void Spawn() {
		this.gameObject.transform.position = new Vector3 (4, 0.5f, 4);
	}

	void OnCollisionStay()
	{
		isGrounded = true;
	}

	void HitBlock(Collision collision) {
		var blockController = collision.gameObject.GetComponent<BlockController> ();
		var parentController = blockController.parent;

		if (parentController.CanMove (this.Direction)) {
			parentController.Move (this.Direction, amount: 1);
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.name == "block") {
			this.HitBlock (collision);
		}
		
		foreach (ContactPoint contact in collision.contacts)
		{
			Debug.DrawRay(contact.point, contact.normal, Color.white);
		}
	}

	void GetDirection(float x, float z){ 
		var absX = Mathf.Abs (x);
		var absZ = Mathf.Abs (z);

		if (absX > absZ) {
			if (x > 0) {
				Debug.Log ("Right");
				this.Direction = Vector3.right;
			} else {
				Debug.Log ("Left");
				this.Direction = Vector3.left;
			}
		} else if (absZ > absX) {
			if (z > 0) {
				Debug.Log ("Forward");
				this.Direction = Vector3.forward;
			} else {
				Debug.Log ("Back");
				this.Direction = Vector3.back;
			}
		}
	}

	void Update () {

		/* THIS ENTIRE METHOD NEEDS TO BE SURROUNDED WITH A CHECK TO SEE IF IT'S NOT */
		/* ALREADY MOVING, AND THAT ITS POSITION IS A ROUND NUMBER (I.E. IT'S ON THE GRID) */
		//TODO Check if "on grid" and readjust to nearest floor cube's x and z if not
		//TODO Add 4-D rotation using the second joystick and angle checker (might need direction indicator in-game)

		if ((Input.GetKeyDown (KeyCode.Space)) && isGrounded) {

			this.rigidBody.AddForce (jump * jumpForce, ForceMode.Acceleration);
			isGrounded = false;
		}

		var inputX = Input.GetAxis ("Horizontal");
		var inputZ = Input.GetAxis ("Vertical");

		this.GetDirection (inputX, inputZ);

		transform.Translate (
			x: inputX * Time.deltaTime * MoveSpeed,
			y: 0,
			z: inputZ * Time.deltaTime * MoveSpeed
		);
	}
}


//=======
//        //left joystick
//        var x = Input.GetAxis("Horizontal") * Time.deltaTime;
//        var z = Input.GetAxis("Vertical") * Time.deltaTime;
//
//        //right joystick
//        var x2 = Input.GetAxis("Horizontal2") * Time.deltaTime * RotateSpeed;
//        var z2 = Input.GetAxis("Vertical2") * Time.deltaTime * RotateSpeed;
//
//        Debug.Log(x + " " + z);
//
//        float xz = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(z, 2)) * MoveSpeed; //diagonal speed
//
//        //left joystick angular check
//        if (x > 0) {
//            if (z > 0) 
//                transform.Translate(xz, 0, 0);
//            else if (z < 0)
//                transform.Translate(0, 0, -xz);
//        } else if (x < 0) {
//            if (z > 0)
//                transform.Translate(0, 0, xz);
//            else if (z < 0)
//                transform.Translate(-xz, 0, 0);
//        }
//        
//        //transform.Rotate(0, z2, 0);
//
//    }

