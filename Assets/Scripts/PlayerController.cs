﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

	private Vector3 direction;

	// Use this for initialization
	void Start () {
		this.name = "player";
		this.Spawn();	
		this.jump = new Vector3 (0.0f, this.jumpForce, 0);
	}
		
	void Spawn() {
		this.gameObject.transform.position = new Vector3 (4, 0.5f, 4);
	}
// 
//	void HitBlock(Collision collision) {
//		var blockController = collision.gameObject.GetComponent<BlockController> ();
//		var parentController = blockController.parent;
//
//		if (parentController.CanMove (this.direction)) {
//			parentController.Move (this.direction, amount: 1);
//		} 
//	}
//
//	void OnCollisionEnter(Collision collision)
//	{
//		if (collision.gameObject.name == "block") {
//			this.HitBlock (collision);
//		}
//		
//		foreach (ContactPoint contact in collision.contacts)
//		{
//			Debug.DrawRay(contact.point, contact.normal, Color.white);
//		}
//	}

	void GetDirection(float x, float z){ 
		var absX = Mathf.Abs (x);
		var absZ = Mathf.Abs (z);

		if (absX > absZ) {
			if (x > 0) {
				Debug.Log ("Right");
				this.direction = Vector3.right;
			} else {
				Debug.Log ("Left");
				this.direction = Vector3.left;
			}
		} else if (absZ > absX) {
			if (z > 0) {
				Debug.Log ("Forward");
				this.direction = Vector3.forward;
			} else {
				Debug.Log ("Back");
				this.direction = Vector3.back;
			}
		}
	}

    void Update()
    {

        //DONE Check if "on grid" and readjust to nearest floor cube's x and z if not
        //TODO Add 4-D rotation using the second joystick and angle checker (might need direction indicator in-game)
        //TODO MOVEMENT NEEDS DIRECTIONAL OBSTACLE CHECK AS EXTRA CONDITION

        if ((Input.GetKeyDown(KeyCode.Space) || (Input.GetKeyDown(KeyCode.Joystick1Button0 /*'A' button*/))) && isGrounded)
        {
            //this.rigidBody.AddForce(jump * jumpForce, ForceMode.Impulse);
            //sisGrounded = false;
        }
			
        /* LEFT JOYSTICK (MOVEMENT) */

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        this.GetDirection(x, z);   
        //float xz = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(z, 2)) * MoveSpeed; //diagonal speed

        //float firstVal = 0;
        //float secondVal = 0;
        //float thirdVal = 0;

        //Vector3 direction = new Vector3(0, 0, 0);

        input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //left joystick angular check
        if (!isMoving)
        { //if not moving already, get requested direction of movement from input

            /*Vector3 moveDirection = new Vector3(0, 0, 0);
            float angle = Mathf.Atan2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Mathf.Rad2Deg;

            if (0 <= angle && angle < 90) {
                moveDirection = Vector3.forward;
            } else if (90 <= angle && angle < 180) {
                moveDirection = Vector3.left;
            } else if (180 <= angle && angle < 270) {
                moveDirection = Vector3.back;
            } else if (270 <= angle && angle < 360) {
                moveDirection = Vector3.right;
            }

            if (input != Vector3.zero)
                StartCoroutine(move(moveDirection));
            */
            if (x != 0 && z != 0) {
                if (x > 0) {
                    if (z > 0)
                        direction = Vector3.right;
                    else if (z < 0)
                        direction = -Vector3.forward;
                } else if (x < 0) {
                    if (z > 0)
                        direction = Vector3.forward;
                    else if (z < 0)
                        direction = -Vector3.right;
                }

				// raycast in target direction
				// if no hit - move that way
				// if block, check if block can move
					// if it can, move that way
					// else check if we can mantle



				if (input != Vector3.zero) {
					TryMove (direction);
				}
            }
        }
			
        /* RIGHT JOYSTICK (ROTATION) */

        float x2 = Input.GetAxis("Horizontal2") * Time.deltaTime * rotateSpeed;
        float z2 = Input.GetAxis("Vertical2") * Time.deltaTime * rotateSpeed;
        //transform.Rotate(0, z2, 0);

    }

	public void TryMove(Vector3 direction) {
		RaycastHit hit;
		var transformDirection = transform.TransformDirection (direction);
		Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), transformDirection * 1000, Color.white);

		int layerMask = 1 << 8;

		if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), transformDirection, out hit, Mathf.Infinity))
		{
			
			Debug.Log ("HIT");			
			Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), transformDirection * hit.distance, Color.red);
			if (hit.distance >= 0.8) {
				// gap 
				// check if block below
				RaycastHit hit2;

				if (Physics.Raycast (transform.position, Vector2.down, out hit2, Mathf.Infinity)) {
					if (hit2.distance <= 0.8) {
						if (hit2.collider.gameObject.name == "block") {
							var block = hit2.collider.gameObject.GetComponent<BlockController> ();
							if (block.SquareEmpty (direction)) {
								 moveAndDrop (direction);
								return;
							}
						}

					}
				}
					
				// if block below ask block if it can move in that direciton
				// if so, movedown
				StartCoroutine (move (direction));
				return;
			} else {
				if (hit.collider.gameObject.name == "block") {
					var block = hit.collider.gameObject.GetComponent<BlockController> ();
					if (block.parent.CanMove (direction)) {
						StartCoroutine (move (direction));
						block.parent.Move (direction, 1);
						// can move and will push block
						return;
					} else if (block.CanMantle ()) {
						// can mantle
						moveAndRise (direction);
						return;
					}
				}
			}
		}
		else
		{
			StartCoroutine (move (direction));
			Debug.DrawRay(transform.position, transformDirection * 1000, Color.white);
		}

	}

	public void moveAndDrop(Vector3 moveDirection)
	{
		Debug.Log ("DROP");
		StartCoroutine(move(moveDirection + new Vector3 (0, -1, 0)));
	}

	public void moveAndRise(Vector3 moveDirection)
	{
		Debug.Log ("DROP");
		StartCoroutine(move(moveDirection + new Vector3 (0, 1, 0)));
	}
		
    public IEnumerator move(Vector3 moveDirection)
    {
		Debug.Log ("MOVE");

        isMoving = true;
        startPosition = transform.position;
        endPosition = transform.position + moveDirection;
        timer = 0;

        while (timer < 1f)
        {
			//transform.Rotate (new Vector3(0, -2, 0));
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
