using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutoController : MonoBehaviour
{

    [SerializeField]
    private GameObject rightTrigger, leftTrigger, leftStick, leftStickParent, controller;

    private bool LT, RT, LS;
    private bool[] LSchecks;
    private int LScheck = 0;
    private bool LSmoving = false;
    bool maxDistReached = false;

    private Vector3 LSpos;
    private Transform LStrans;
    private bool goingOut = true;

    private float x, z;
    private float triggerPosY;
    private bool goingUp;

    private int moveCount = 0;

    private float startTime, journeyLength;


    void Start() {

        LT = RT = LS = false;
        triggerPosY = leftTrigger.transform.position.y;

    }

    void Update() {


        if (Input.GetAxis("Horizontal") != 0 && Input.GetAxis("Vertical") != 0) {
            LS = true;
        }
        if (Input.GetAxis("Right Trigger") == 1 || Input.GetKeyDown(KeyCode.E)) {
            RT = true;
        }
        if (Input.GetAxis("Left Trigger") == 1 || Input.GetKeyDown(KeyCode.Q)) {
            LT = true;
        }



        if (!(LS && LT && RT))
        {
            if (!LS)
                leftStickParent.transform.Rotate(new Vector3(0, 0, 2));
            else
            {
                Color color = leftStick.GetComponent<Image>().color;
                leftStick.GetComponent<Image>().color = new Color(color.r - 0.01f, color.g - 0.01f, color.b - 0.01f);
                leftStick.transform.localScale = new Vector3(1.1f, 1, 1);
                leftStick.transform.localPosition = Vector3.zero;
            }

            if (!RT)
                animateTrigger(rightTrigger.transform);
            else
            {
                rightTrigger.transform.position = new Vector3(rightTrigger.transform.position.x, triggerPosY, rightTrigger.transform.position.z);
                Color color = rightTrigger.GetComponent<Image>().color;
                rightTrigger.GetComponent<Image>().color = new Color(color.r - 0.01f, color.g - 0.01f, color.b - 0.01f);
            }

            if (!LT)
                animateTrigger(leftTrigger.transform);
            else
            {
                leftTrigger.transform.position = new Vector3(leftTrigger.transform.position.x, triggerPosY, leftTrigger.transform.position.z);
                Color color = leftTrigger.GetComponent<Image>().color;
                leftTrigger.GetComponent<Image>().color = new Color(color.r - 0.01f, color.g - 0.01f, color.b - 0.01f);
            }
        }
        else
        {
            Color color = leftStick.GetComponent<Image>().color;
            leftTrigger.GetComponent<Image>().color = new Color(0, 0, 0, color.a - 0.01f);
            rightTrigger.GetComponent<Image>().color = new Color(0, 0, 0, color.a - 0.01f);
            leftStick.GetComponent<Image>().color = new Color(0, 0, 0, color.a - 0.01f);
            color = controller.GetComponent<Image>().color;
            controller.GetComponent<Image>().color = new Color(color.r, color.g, color.b, color.a - 0.01f);
        }
            
    }

    void animateTrigger(Transform tr) {
        float x = tr.position.x;
        float z = tr.position.z;

        if (tr.position.y >= triggerPosY)
            goingUp = false;
        if (tr.position.y <= triggerPosY - 12)
            goingUp = true;

        if (goingUp)
            tr.Translate(Vector3.up * Time.deltaTime * 20);
        else
            tr.Translate(Vector3.down * Time.deltaTime * 20);
    }

    //LStrans.position = LSpos;

    /*if (moveCount < 1600)
    {

        float time = Time.deltaTime * 10;

        if (moveCount == 0 || moveCount == 200 || moveCount == 400 || moveCount == 600 || moveCount == 800 || moveCount == 1000 || moveCount == 1200 || moveCount == 1400)
            LStrans.position = LSpos;

        if (moveCount < 200)
            LStrans.position = LSpos + new Vector3(time, time, 0.0f);
        else if (moveCount < 400)
            LStrans.position = LSpos + new Vector3(-time, -time, 0.0f);
        else if (moveCount < 600)
            LStrans.position = LSpos + new Vector3(time, -time, 0.0f);
        else if (moveCount < 800)
            LStrans.position = LSpos + new Vector3(-time, time, 0.0f);
        else if (moveCount < 1000)
            LStrans.position = LSpos + new Vector3(-time, time, 0.0f);
        else if (moveCount < 1200)
            LStrans.position = LSpos + new Vector3(time, -time, 0.0f);
        else if (moveCount < 1400)
            LStrans.position = LSpos + new Vector3(-time, -time, 0.0f);
        else if (moveCount < 1600)
            LStrans.position = LSpos + new Vector3(time, time, 0.0f);
    }
    else
        moveCount = 0;

    moveCount++;*/

    /*switch(LScheck) {
        case 0:
            if (goingOut) {
                StartCoroutine(animateLeftStick(LSpos, LSpos + new Vector3(10, 10, 0), 1));
            }
            break;
    }*/
    //StartCoroutine(animateLeftStick(LSpos, LSpos + new Vector3(10, 10, 0), 1));
    //StartCoroutine(animateLeftStick(LSpos + new Vector3(10, 10, 0), LSpos, 2));
    //StartCoroutine(animateLeftStick(new Vector3(-20, 20, 0), 3));
    //StartCoroutine(animateLeftStick(new Vector3(20, -20, 0), 5));
    //StartCoroutine(animateLeftStick(new Vector3(-20, -20, 0), 7));



    //StartCoroutine(animateLeftStick());

    /*switch (LScheck) {
        case 0:
            animateLeftStick(new Vector3(1, 1, 0));
            break;
        case 1:
            animateLeftStick(new Vector3(-1, 1, 0));
            break;
        case 2:
            animateLeftStick(new Vector3(1, -1, 0));
            break;
        case 3:
            animateLeftStick(new Vector3(-1, -1, 0));
            break;
        case 4:
            LScheck = 0;
            break;
    }*/


    /*IEnumerator animateLeftStick() {
        StartCoroutine(animateLeftStickTR( new Vector3(1, 1, 0)));
        yield return new WaitForSeconds(2);
        StopCoroutine("animateLeftStickTR");
        StartCoroutine(animateLeftStickTL( new Vector3(-1, 1, 0)));
        yield return new WaitForSeconds(2);
        StartCoroutine(animateLeftStickBR( new Vector3(1, -1, 0)));
        yield return new WaitForSeconds(2);
        StartCoroutine(animateLeftStickBL( new Vector3(-1, -1, 0)));
        yield return new WaitForSeconds(2);
        yield return null;
    }*/

    /*IEnumerator animateLeftStick(Vector3 start, Vector3 end, float waitTime) {

        if (waitTime > 0) {
            float timer = waitTime;
            waitTime = 0;
            yield return new WaitForSeconds(timer);
        }

        float distCovered = (Time.time - startTime) * 10;
        float fracJourney = distCovered / journeyLength;

        if (distCovered <= journeyLength) {
            Debug.Log(distCovered + "  " + (journeyLength - 1));
            LStrans.position = Vector3.Slerp(start, end, fracJourney);
            if (distCovered >= journeyLength - 1) {
                StopCoroutine(animateLeftStick(start, end, waitTime));
                LScheck++;
                goingOut = !goingOut;
                if (LScheck > 3)
                    LScheck = 0;
            }
        }
    }*/

    /*
    void moveStick(Vector3 dir)
    {
        if (moveCount < 50)
        {
            LStrans.Translate(dir * Time.deltaTime * 20);
            moveCount++;
        }
        else if (moveCount < 100)
        {
            LStrans.Translate(-dir * Time.deltaTime * 20);
            moveCount++;
        }
        else
        {
            Debug.Log(LScheck);
            LStrans.localPosition = LSpos;
        }
    }*/
}