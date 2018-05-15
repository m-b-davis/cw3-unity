using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

    public GameObject GUICanvas, MenuCanvas, camera, gameObjects;

    private bool menuActive = false;

	void Start () {
		
	}
	

	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuActive = !menuActive;
        }

        GUICanvas.SetActive(!menuActive);
        //gameObjects.SetActive(!menuActive);
        MenuCanvas.SetActive(menuActive);

        if (menuActive)
        {

        }

        /*if (menuActive)
            camera.GetComponent<Camera>().cullingMask = 0;
        else
            camera.GetComponent<Camera>().cullingMask = 1;*/
    }
}
