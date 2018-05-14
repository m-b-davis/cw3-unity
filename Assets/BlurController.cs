﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlurController : MonoBehaviour {

	private float blurTimer = 0;
	private float currentBlur = 0;
	public float BlurAmount = 3;

	private float blurTime = 0;
	private float blurStart = 0;
	private float blurTarget = 0;
	private RawImage rend;

	public void ShowBlur(float time = 3f) {
		blurTarget = BlurAmount;
		blurStart = 0;
		blurTimer = time;
		blurTime = time;
	}

	public void HideBlur(float time = 3f) {
		blurTarget = 0;
		blurStart = BlurAmount;
		blurTimer = time;
		blurTime = time;
	}

	// Use this for initialization
	void Start () {
		this.rend = GetComponent<RawImage> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (blurTimer > 0f) {
			blurTimer -= Time.deltaTime;
			currentBlur = Mathf.Lerp (blurStart, blurTarget, blurTime - blurTimer);

			rend.material.SetFloat ("_Size", currentBlur);
		}
	}
}