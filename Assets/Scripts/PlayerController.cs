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
		this.Spawn ();	
		this.rigidBody = GetComponent<Rigidbody> ();
		this.rigidBody.freezeRotation = true;
		this.jump = new Vector3 (0.0f, this.jumpForce, 0);
	}

	void Spawn() {
		this.gameObject.transform.position = new Vector3 (0, 1.2f, 0);
	}
		

	void OnCollisionStay()
	{
		isGrounded = true;
	}

	void HitBlock(Collision collision) {
		var blockController = collision.gameObject.GetComponent<BlockController> ();
		var parentController = blockController.parent;

		// Calculate Angle Between the collision point and the player
		Vector3 dir = collision.contacts[0].point - transform.position;
//		Debug.Log ("Before rounding hitblock:");
//		Debug.Log (dir.x);
//		Debug.Log (dir.y);
//		Debug.Log (dir.z);
//
//		dir.x = Mathf.Round(dir.x / 90) * 90;
//		dir.y = Mathf.Round(dir.y / 90) * 90;
//		dir.z = Mathf.Round(dir.z / 90) * 90;
//
//		Debug.Log ("Direction hitblock:");
//		Debug.Log (dir.x);
//		Debug.Log (dir.y);
//		Debug.Log (dir.z);

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
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space) && isGrounded){

			this.rigidBody.AddForce(jump * jumpForce, ForceMode.Acceleration);
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
