using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialKeyPress : MonoBehaviour {

	/*[SerializeField]
	private KeyCode myKey;
	private Renderer rend;
    [SerializeField]
    private string action;
    private Transform tr;
    private float xp, yp, zp, xr, yr, zr, xs, ys, zs = 0;
    private LevelController levelController;

	void Start() {
        levelController = GameObject.Find("Level Controller").GetComponent<LevelController>();

        if (levelController.GetGameMode() != LevelController.GameMode.Tutorial) {
            Destroy(gameObject);
        }
        if (levelController.GetLevelCount() == 0) {
            if (!(myKey == KeyCode.W || myKey == KeyCode.A || myKey == KeyCode.S || myKey == KeyCode.D)) {
                Destroy(gameObject);
            }
        }
        if (levelController.GetLevelCount() == 1) {
            if (!(myKey == KeyCode.Mouse0 || myKey == KeyCode.Q || myKey == KeyCode.E)) {
                Destroy(gameObject);
            }
        }
        if (levelController.GetLevelCount() > 1) {
            Destroy(gameObject);
        }
        tr = GetComponent<Transform>();
        rend = GetComponent<Renderer>();
	}


	void Update () {
		if (Input.GetKeyDown(myKey) || Input.GetKeyDown("space")) {
            if (action == "+zpos") {
                zp = 0.02f;
            } else if (action == "-zpos") {
                zp = -0.03f;
            } else if (action == "+xpos") {
                xp = 0.03f;
            } else if (action == "-xpos") {
                xp = -0.03f;
            } else if (action == "+ypos") {
                yp = 0.03f;
            } else if (action == "-ypos") {
                yp = -0.03f;
            } else if (action == "+yrot") {
                yr = 2f;
                xs = 0.004f;
                ys = 0.004f;
            } else if (action == "-yrot") {
                yr = -2f;
                xs = 0.004f;
                ys = 0.004f;
            } else if (action == "+size") {
                xs = 0.004f;
                ys = 0.004f;
            }

            Fade ();
		}
	}

	void Fade() {

        tr.position = new Vector3(tr.position.x + xp, tr.position.y + yp, tr.position.z + zp);
        tr.eulerAngles = new Vector3(tr.eulerAngles.x + xr, tr.eulerAngles.y + yr, tr.eulerAngles.z + zr);
        tr.localScale = new Vector3(tr.localScale.x + xs, tr.localScale.y + ys, tr.localScale.z + zs);

        if (rend.material.color.a > 0) {
			Color color = rend.material.color;
			color.a -= 0.01f;
			rend.material.color = color;
			Invoke ("Fade", 0.01f);
		} else if (rend.material.color.a <= 0) {
			Destroy (gameObject);
		}
	}*/
}
