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

	public void Move(Vector3 direction, float amount) {
	}

	public bool CanMove(Vector3 direction) {
		RaycastHit hit;
		var transformDirection = transform.TransformDirection (direction);

		// Does the ray intersect any objects excluding the player layer
		if (Physics.Raycast(transform.position, transformDirection, out hit, Mathf.Infinity))
		{
			
//			Debug.DrawRay(transform.position, transformDirection * hit.distance, Color.red);
//			Debug.Log("Did Hit");

			if (hit.collider.gameObject.name == "block") {
				if (hit.collider.gameObject.GetComponent<BlockController> ().parent == this.parent){
					return true;
				}
			}
				
			return hit.distance >= 0.6;
		}
		else
		{
//			Debug.DrawRay(transform.position, transformDirection * 1000, Color.white);
//			Debug.Log("Did not Hit");
		}

		return true;
	}

	public bool CannotMove() {
		var directions = new Vector3[]{ Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
		return directions.All (direction => !this.CanMove (direction));
	}


	public void Move(Vector3 direction) {

	}
}
