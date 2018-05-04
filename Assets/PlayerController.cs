using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

	public float MoveSpeed;
	public float RotateSpeed;

	private bool isGrounded = false;
	private Rigidbody rigidBody;

	public Vector3 jump;
	public float jumpForce = 10.0f;

	// Use this for initialization
	void Start () {
		this.Spawn ();	
		this.rigidBody = GetComponent<Rigidbody> ();
		this.jump = new Vector3 (0.0f, this.jumpForce, 0);
	}

	void Spawn() {
		this.gameObject.transform.position = new Vector3 (0, 1.2f, 0);
	}
		

	void OnCollisionStay()
	{
		isGrounded = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space) && isGrounded){

			this.rigidBody.AddForce(jump * jumpForce, ForceMode.Impulse);
			isGrounded = false;
		}

		var x = Input.GetAxis("Horizontal") * Time.deltaTime * RotateSpeed;
		var z = Input.GetAxis("Vertical") * Time.deltaTime * MoveSpeed;

		transform.Rotate(0, x, 0);
		transform.Translate(0, 0, z);
	}
}
