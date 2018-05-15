using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class BlockController : MonoBehaviour {

	public PieceParent parent;

	public void Initialise(PieceParent parent) {
		this.parent = parent;
	}
		
	// Use this for initialization
	void Start () {
		this.name = "block";
		this.gameObject.SetActive (false);
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

			var squareEmpty = hit.distance >= 0.6;

			// ignore player if going downwards (or crush player)
			if (hit.collider.gameObject.name == "player") {
				if (direction == Vector3.down) {
					if (!squareEmpty && parent.CanCrush) {
						FindObjectOfType<LevelManager> ().PlayerCrushed ();
					}
					return squareEmpty;
				}
				return true;
			}

			return squareEmpty;
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

	public void SetShadowsEnabled(bool enabled) {
		var shadowMode = enabled ? ShadowCastingMode.On : ShadowCastingMode.Off;

		GetComponentsInChildren<Renderer> ().ToList ().ForEach (renderer => 
			renderer.shadowCastingMode = shadowMode
		);
	}


	public void Move(Vector3 direction) {

	}
}
