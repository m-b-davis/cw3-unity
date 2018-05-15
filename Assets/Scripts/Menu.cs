using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

    public GameObject GUICanvas, MenuCanvas, camera, gameObjects;

    private bool menuActive = false;

    private BlurController blurController;


    void Start () {
        blurController = FindObjectOfType<BlurController>();
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
            blurController.ShowBlur();
        else
            blurController.HideBlur();

        /*if (menuActive)
            camera.GetComponent<Camera>().cullingMask = 0;
        else
            camera.GetComponent<Camera>().cullingMask = 1;*/
    }
}
