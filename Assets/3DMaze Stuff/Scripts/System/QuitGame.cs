﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour {

    public class Quit : MonoBehaviour {
        void Update() {
            if (Input.GetKey("escape"))
                Application.Quit();
        }
    }
}
