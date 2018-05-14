using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDeleteAnim : MonoBehaviour {

    private bool kill = false;

    private Vector3 init;
    private Vector3 final = new Vector3(0,0,0);
    private float timeScale = 0.5f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        init = transform.localScale;

        if (Input.GetKeyDown(KeyCode.Space)) {
            kill = !kill;
        }
        if (kill) {
            StartCoroutine("LerpSize");
        }
	}

    IEnumerator LerpSize()
    {
        transform.localScale = Vector3.Lerp(init, final, Time.deltaTime * timeScale);
        yield break;
    }
}
