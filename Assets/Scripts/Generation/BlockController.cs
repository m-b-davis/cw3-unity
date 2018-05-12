using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockController : MonoBehaviour {

	public PieceParent parent;

	public void Initialise(PieceParent parent) {
		this.parent = parent;
	}
		
	// Use this for initialization
	void Start () {
		this.name = "block";
	}
	
	// Update is called once per frame
	void Update () {

	}

	public bool CanMantle() {
		return this.CanMove (Vector3.up);
	}

	public bool SquareEmpty(Vector3 direction, bool ignoreSelf = false) {
		RaycastHit hit;
		var transformDirection = transform.TransformDirection (direction);
	
		if (Physics.Raycast(transform.position, transformDirection, out hit, Mathf.Infinity))
		{
			if (ignoreSelf) {
				if (hit.collider.gameObject.name == "block") {
					if (hit.collider.gameObject.GetComponent<BlockController> ().parent == this.parent) {
						return true;
					}
				}
			}

			// ignore player 
			if (hit.collider.gameObject.name == "player") {
				return true;
			}

			return hit.distance >= 0.6;
		}

		return true;
	}
	
	public bool CanMove(Vector3 direction) {
		return SquareEmpty (direction, ignoreSelf: true);
	}

	public bool CannotMove() {
		var directions = new Vector3[]{ Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
		return directions.All (direction => !this.CanMove (direction));
	}


	public void Move(Vector3 direction) {

	}
}
