using System;
using UnityEngine;


public class FollowCamera: MonoBehaviour
{
	public Transform target;
	public float smoothSpeed = 0.125f;
	public Vector3 offset;
	public Vector3 spawnOffset;

	private float ZoomTimeRemaining = 1f;
	private bool SpawnZoom = true;
	private bool firstFrame = true;

	public FollowCamera ()
	{
	}

	void Start() {


	}

	void FixedUpdate() {
		
		if (firstFrame) {
			transform.position = spawnOffset;
			firstFrame = false;
			return;
		}

		var desiredPosition = target.position + offset;

		if (SpawnZoom) {
			desiredPosition = desiredPosition + (spawnOffset * ZoomTimeRemaining);
			ZoomTimeRemaining -= Time.deltaTime;

			if (ZoomTimeRemaining <= 0)
				SpawnZoom = false;
		}

		var smoothedPosition = Vector3.Lerp (transform.position, desiredPosition, smoothSpeed);

		transform.position = new Vector3 (transform.position.x, smoothedPosition.y, transform.position.z);
	}
}

