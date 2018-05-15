using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PieceParent : MonoBehaviour {

	private Rigidbody rb;
	private BlockController[] blockControllers;

	private bool dropping = false;
	private float DropSpeed = 0;
	private bool paused = true;
	private bool complete = false;

	private bool indicateDrop = false;
	private float indicatorTimer = 0;
	private bool indicating = false;
	private float indicateInterval = 0.5f;

	void OnPauseGame ()
	{
		paused = true;
	}

	void OnResumeGame ()
	{
		paused = false;
	}

	public bool CanCrush = true;

	public int NumBlocks { get { return blockControllers.Length; } }

	private LevelManager levelManager;

	// Use this for initialization
	void Start () {
		this.paused = true;
		this.InitialiseChildren ();
	}

	public void Begin(float dropSpeed) {
		this.DropSpeed = dropSpeed;
		this.paused = false;
		this.indicateDrop = true;
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
					this.indicateDrop = false;
					this.SetShadowsEnabled (true);
				}
			} else {
				if (CanMove (Vector3.down)) {
					// if slow, find a better method of checking when to fall
					// perhaps only recalculate fall distance on a certain event 
					this.StartDrop ();
				}
			}
		}

		if (indicateDrop) {
			if (indicatorTimer > 0) {
				indicatorTimer -= Time.fixedDeltaTime;
			} else {
				indicating = !indicating;
				indicatorTimer = indicateInterval;

				SetShadowsEnabled (indicating);
			}
		}
	}

	private void SetShadowsEnabled(bool isEnabled) {
		foreach (var controller in blockControllers) {
			controller.SetShadowsEnabled (isEnabled);
		}
	}

	public void StartDrop() {
		foreach (var controller in blockControllers) {
			controller.gameObject.SetActive (true);
		}

		this.dropping = true;
	}

	public void StopDrop() {
		this.dropping = false;
		this.levelManager.HandlePieceFell (this);
		this.CanCrush = false;
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

