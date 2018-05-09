﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PieceParent : MonoBehaviour {

	private Rigidbody rb;
	private BlockController[] blockControllers;

	private bool dropping = false;
	private int DropSpeed = 10;
	private bool paused = true;
	private bool complete = false;

	private LevelManager levelManager;

	// Use this for initialization
	void Start () {
		this.paused = true;
		this.InitialiseChildren ();
	}

	public void Begin() {
		this.paused = false;
	}
		
	public int GetRowIndex() {
		float yPos = blockControllers [0].transform.position.y;
		return Mathf.RoundToInt (yPos);
	}

	public void RowComplete() {
		this.complete = true;
	}
		
	private void InitialiseChildren() {
		this.blockControllers = this.GetComponentsInChildren<BlockController> ();
	
		foreach (var controller in blockControllers) {
			controller.Initialise (this);
		}
	}

	public void SetLevelManager(LevelManager manager) {
		this.levelManager = manager;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (paused)
			return;

		if (complete) {
			transform.Translate (0, Time.deltaTime * DropSpeed * -1, 0);
		} else {
			if (dropping) {
				if (CanMove (Vector3.down)) {
					transform.Translate (0, Time.deltaTime * DropSpeed * -1, 0);
				} else {
					this.StopDrop ();
				}
			} else {
				if (CanMove (Vector3.down)) {
					// if slow, find a better method of checking when to fall
					// perhaps only recalculate fall distance on a certain event 
					this.StartDrop ();
				}
			}
		}
	}

	public void StartDrop() {
		this.dropping = true;
	}

	public void StopDrop() {
		this.dropping = false;
		this.levelManager.HandlePieceFell (this);
	}

	public bool CanMove(Vector3 direction) {
		foreach (var controller in blockControllers) {
			if (!controller.CanMove (direction)) {
				return false;
			}
		}

		return true;
	}

	public bool CannotMove() {
		foreach (var controller in blockControllers) {
			if (!controller.CannotMove ()) {
				return false;
			}
		}

		return true;
	}
		
	public void Move(Vector3 direction, float amount) {

		transform.Translate (direction * amount);
	}
}

