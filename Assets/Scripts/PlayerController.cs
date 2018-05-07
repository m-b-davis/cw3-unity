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



	void Start () {
		this.Spawn();	
		this.rigidBody = GetComponent<Rigidbody> ();
		this.jump = new Vector3 (0.0f, this.jumpForce, 0);
	}



	void Spawn() {
		this.gameObject.transform.position = new Vector3 (4, 0.5f, 4);
	}
	
    	

	void OnCollisionStay()
	{
		isGrounded = true;
	}
	


	void Update () {

        /* THIS ENTIRE METHOD NEEDS TO BE SURROUNDED WITH A CHECK TO SEE IF IT'S NOT */
        /* ALREADY MOVING, AND THAT ITS POSITION IS A ROUND NUMBER (I.E. IT'S ON THE GRID) */
        //TODO Check if "on grid" and readjust to nearest floor cube's x and z if not
        //TODO Add 4-D rotation using the second joystick and angle checker (might need direction indicator in-game)

        if ((Input.GetKeyDown(KeyCode.Space)) && isGrounded){

			this.rigidBody.AddForce(jump * jumpForce, ForceMode.Impulse);
			isGrounded = false;
		}

        //left joystick
        var x = Input.GetAxis("Horizontal") * Time.deltaTime;
        var z = Input.GetAxis("Vertical") * Time.deltaTime;

        //right joystick
        var x2 = Input.GetAxis("Horizontal2") * Time.deltaTime * RotateSpeed;
        var z2 = Input.GetAxis("Vertical2") * Time.deltaTime * RotateSpeed;

        Debug.Log(x + " " + z);

        float xz = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(z, 2)) * MoveSpeed; //diagonal speed

        //left joystick angular check
        if (x > 0.001f) {
            if (z > 0.001f) 
                transform.Translate(xz, 0, 0);
            else if (z < -0.001f)
                transform.Translate(0, 0, -xz);
        } else if (x < -0.001f) {
            if (z > 0.001f)
                transform.Translate(0, 0, xz);
            else if (z < -0.001f)
                transform.Translate(-xz, 0, 0);
        }
        
        //transform.Rotate(0, z2, 0);

    }
}
