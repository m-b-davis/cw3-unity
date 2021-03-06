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

    public AudioSource audio;

    private Vector3 jump, startPosition, endPosition, input;

	private Vector3 direction;

	private LevelManager LevelManager;

	public int SpawnX;
	public int SpawnZ;

	public bool FreezeInput = false;

	// Use this for initialization
	void Start () {
		this.LevelManager = FindObjectOfType<LevelManager> ();
		this.name = "player";
		this.Spawn();	
		this.jump = new Vector3 (0.0f, this.jumpForce, 0);
	}
		
	void Spawn() {
		this.gameObject.transform.position = new Vector3 (SpawnX, 0.5f, SpawnZ);
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
				this.direction = Vector3.right;
			} else {
				this.direction = Vector3.left;
			}
		} else if (absZ > absX) {
			if (z > 0) {
				this.direction = Vector3.forward;
			} else {
				this.direction = Vector3.back;
			}
		}
	}

    void Update()
    {
		if (FreezeInput)
			return;

        //DONE Check if "on grid" and readjust to nearest floor cube's x and z if not
        //TODO Add 4-D rotation using the second joystick and angle checker (might need direction indicator in-game)
        //DONE MOVEMENT NEEDS DIRECTIONAL OBSTACLE CHECK AS EXTRA CONDITION

        //fixes any rotational issues for raycasting
        transform.rotation = new Quaternion(0,0,0,0);

        if ((Input.GetKeyDown(KeyCode.Space) || (Input.GetKeyDown(KeyCode.Joystick1Button0 /*'A' button*/))) && isGrounded)
        {
            //this.rigidBody.AddForce(jump * jumpForce, ForceMode.Impulse);
            //sisGrounded = false;
        }
			
        /* LEFT JOYSTICK (MOVEMENT) */

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        this.GetDirection(x, z);   

        input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //left joystick angular check
        if (!isMoving)
        { //if not moving already, get requested direction of movement from input

            float newX = x;
            float newZ = z;

            switch (CameraRotator.rotateDir)
            {
                case 0:
                    newX = x;
                    newZ = z;
                    break;
                case 90:
                    newX = z;
                    newZ = -x;
                    break;
                case 180:
                    newX = -x;
                    newZ = -z;
                    break;
                case 270:
                    newX = -z;
                    newZ = x;
                    break;
            }

            if (newX != 0 && newZ != 0) {
                if (newX > 0) {
                    if (newZ > 0)
                        direction = Vector3.right;
                    else if (newZ < 0)
                        direction = -Vector3.forward;
                } else if (newX < 0) {
                    if (newZ > 0)
                        direction = Vector3.forward;
                    else if (newZ < 0)
                        direction = -Vector3.right;
                }
					

				if (input != Vector3.zero) {
					TryMove (direction);
				}
            }
        }
			
        /* RIGHT JOYSTICK (ROTATION) */

        //float x2 = Input.GetAxis("Horizontal2") * Time.deltaTime * rotateSpeed;
        //float z2 = Input.GetAxis("Vertical2") * Time.deltaTime * rotateSpeed;
        //transform.Rotate(0, z2, 0);

    }

	public void TryMove (Vector3 direction) {
		RaycastHit hitFloor;
		var transformDirection = transform.TransformDirection (direction);
		Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), transformDirection * 1000, Color.red);

		if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), transformDirection, out hitFloor, Mathf.Infinity))
		{
			Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), transformDirection * hitFloor.distance, Color.red, 1);
			if (hitFloor.distance >= 0.8) {
				// gap 
				// check if block below
				RaycastHit hitBlockDown;


				Debug.DrawRay(transform.position + direction, Vector3.down, Color.red, 2);

				if (Physics.Raycast (transform.position + direction.normalized + new Vector3(0, 0.1f, 0), Vector3.down, out hitBlockDown, Mathf.Infinity)) {
					Debug.DrawRay(transform.position + direction, Vector3.down * hitBlockDown.distance, Color.green, 2);

					if (hitBlockDown.distance > 0.6) {
						moveAndDrop (direction, Mathf.RoundToInt (hitBlockDown.distance));
						return;
					} else {
						StartCoroutine (move (direction));
						return;
					}
				}
				// if block below ask block if it can move in that direciton
				// if so, movedown
				StartCoroutine (move (direction));
				return;
			} else {
				if (hitFloor.collider.gameObject.name == "block") {
					var block = hitFloor.collider.gameObject.GetComponent<BlockController> ();
					if (block.parent.CanMove (direction)) {
                        audio.Play();
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


	void OnPauseGame ()
	{
		FreezeInput = true;
	}

	void OnResumeGame ()
	{
		FreezeInput = false;
	}


	public void moveAndDrop(Vector3 moveDirection, int amount)
	{
		Debug.Log ("DROP");
		StartCoroutine(move(moveDirection + new Vector3 (0, -amount, 0)));
	}

	public void moveAndRise(Vector3 moveDirection)
	{
		Debug.Log ("DROP");
		StartCoroutine(move(moveDirection + new Vector3 (0, 1, 0)));
	}
		
    public IEnumerator move(Vector3 moveDirection)
    {
		this.isMoving = true;
		Debug.Log ("MOVE");

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

		//fixes any offset issues
		transform.position.Set (Mathf.RoundToInt (transform.position.z), transform.position.y, Mathf.RoundToInt (transform.position.z));

        isMoving = false;
		this.CheckForFailure ();
        yield return 0;
    }

	private void CheckForFailure() {

		var directions = new Vector3[]{ Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
		var heights = new float[] { 0.5f, 1.5f };


		foreach (var direction in directions) {
			foreach (var height in heights) {
				RaycastHit hit;
				if (Physics.Raycast (transform.position + new Vector3 (0, height, 0), direction, out hit, Mathf.Infinity)) {
					if (hit.distance > 0.8) {
						// Can move
						return;
					}
				}
			}
		}

		// If we get here, player has no possible moves
		this.PlayerDied(CauseOfDeath.Checkmate);
	}

	private void PlayerDied(CauseOfDeath cause) {
		this.LevelManager.PlayerDied (cause);
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
