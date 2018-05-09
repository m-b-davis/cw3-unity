using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

	public float moveSpeed;
	public float rotateSpeed;
    public float jumpForce;

    private int gridSize = 7;

    private float timer = 0;

    private bool isGrounded = false;
    private bool isMoving = false;

    private int movementIterator = 0;

    private Vector3 jump, startPosition, endPosition, input;
    private Rigidbody rigidBody;



	void Start () {
		this.Spawn();	
		this.rigidBody = GetComponent<Rigidbody> ();
		this.jump = new Vector3 (0.0f, this.jumpForce, 0);
	}

	void Spawn() {
		this.gameObject.transform.position = new Vector3 (4, 0.5f, 4);
	}
	
	void OnCollisionStay() {
		isGrounded = true;
	}
	


	void Update () {

        //DONE Check if "on grid" and readjust to nearest floor cube's x and z if not
        //TODO Add 4-D rotation using the second joystick and angle checker (might need direction indicator in-game)
        //TODO MOVEMENT NEEDS DIRECTIONAL OBSTACLE CHECK AS EXTRA CONDITION

        if ((Input.GetKeyDown(KeyCode.Space) || (Input.GetKeyDown(KeyCode.Joystick1Button0 /*'A' button*/))) && isGrounded) {

			this.rigidBody.AddForce(jump * jumpForce, ForceMode.Impulse);
			isGrounded = false;
		}



                /* LEFT JOYSTICK (MOVEMENT) */

        float x = Input.GetAxis("Horizontal") * Time.deltaTime;
        float z = Input.GetAxis("Vertical") * Time.deltaTime;
        //float xz = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(z, 2)) * MoveSpeed; //diagonal speed

        //float firstVal = 0;
        //float secondVal = 0;
        //float thirdVal = 0;

        Vector3 direction = new Vector3(0, 0, 0);

        Debug.Log(isMoving);

        input = new Vector3(Input.GetAxis("Horizontal"),0 , Input.GetAxis("Vertical"));

        //left joystick angular check
        if (!isMoving) { //if not moving already, get requested direction of movement from input
            if (x >= 0) {
                if (z >= 0)
                    direction = Vector3.right;
                else if (z <= 0)
                    direction = -Vector3.forward;
            } else if (x <= 0) {
                if (z >= 0)
                    direction = Vector3.forward;
                else if (z <= 0)
                    direction = -Vector3.right;
            }
            if (input != Vector3.zero)
                StartCoroutine(move(direction));
        }



                /* RIGHT JOYSTICK (ROTATION) */

        float x2 = Input.GetAxis("Horizontal2") * Time.deltaTime * rotateSpeed;
        float z2 = Input.GetAxis("Vertical2") * Time.deltaTime * rotateSpeed;
        //transform.Rotate(0, z2, 0);

    }

    public IEnumerator move(Vector3 direction)
    {
        isMoving = true;
        Debug.Log("hello");
        startPosition = transform.position;
        endPosition = transform.position + direction;
        timer = 0;

        while (timer < 1f)
        {
            timer += Time.deltaTime * (moveSpeed / gridSize);
            transform.position = Vector3.Lerp(startPosition, endPosition, timer);
            yield return null;
        }

        isMoving = false;
        yield return 0;
    }
}


/*
//left joystick angular check
        if (!isMoving) { //if not moving already, get requested direction of movement from input
            if (x > 0) {
                if (z > 0) {
                    firstVal = xz;
                    secondVal = 0;
                    thirdVal = 0;
                    isMoving = true;
                } else if (z < 0) {
                    firstVal = 0;
                    secondVal = 0;
                    thirdVal = -xz;
                    isMoving = true;
                }
            } else if (x < 0) {
                if (z > 0) {
                    firstVal = 0;
                    secondVal = 0;
                    thirdVal = xz;
                    isMoving = true;
                } else if (z < 0) {
                    firstVal = -xz;
                    secondVal = 0;
                    thirdVal = 0;
                    isMoving = true;
                }
            }
        } else {
            Debug.Log(movementIterator);
            if (movementIterator < 10)
            {
                transform.Translate(firstVal, secondVal, thirdVal);
            } else {
                isMoving = false;
                movementIterator = 0;
            }
            movementIterator++;
        } 
*/
