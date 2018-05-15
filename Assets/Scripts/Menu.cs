using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

    public GameObject GUICanvas, MenuCanvas, StartCanvas, camera, gameComponents;

    public static bool gameStart = true;
    private bool menuActive = false;

    private BlurController blurController;


    void Start () {
        blurController = FindObjectOfType<BlurController>();
    }
	

	void Update () {

        if (!gameStart)
        {
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
        else
        {
            gameComponents.SetActive(false);
            GUICanvas.SetActive(false);
            MenuCanvas.SetActive(false);
            StartCanvas.SetActive(true);

            if (Input.anyKeyDown)
            {
                gameStart = false;
                StartCanvas.SetActive(false);
                GUICanvas.SetActive(true);
                gameComponents.SetActive(true);
            }
        }
    }
}
