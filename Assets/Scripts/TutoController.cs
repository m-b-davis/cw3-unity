using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoController : MonoBehaviour
{

    [SerializeField]
    private GameObject rightTrigger, leftTrigger, leftStick;
    private bool LT, RT, LStr, LStl, LSbr, LSbl;
    private bool[] LSchecks;
    private int LScheck = 0;
    private bool LSmoving = false;
    bool maxDistReached = false;

    private Vector3 LSpos;
    private Transform LStrans;
    private int moveCount = 0;

    private float x, z;
    private float triggerPosY;
    private bool goingUp;

    void Start() {
        LStrans = leftStick.transform;
        LSpos = LStrans.localPosition;
        LT = RT = LStr = LStl = LSbr = LSbl = false;
        LSchecks = new bool[] { LStr, LStl, LSbr, LSbl };
        triggerPosY = leftTrigger.transform.position.y;
    }

    void Update() {

        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        if (x != 0 && z != 0) {
            if (x > 0) {
                if (z > 0)
                    LStr = true;
                else if (z < 0)
                    LSbr = true;
            }
            else if (x < 0) {
                if (z > 0)
                    LStl = true;
                else if (z < 0)
                    LSbl = true;
            }
        }

        if (Input.GetAxis("Right Trigger") == 1) {
            RT = true;
        }
        if (Input.GetAxis("Left Trigger") == 1) {
            LT = true;
        }

        if (!(LT && RT && LStr && LSbr && LSbl && LStl)) {

            if (LT)
                animateTrigger(leftTrigger.transform);
            if (RT)
                animateTrigger(rightTrigger.transform);

            StartCoroutine("animateLeftStick");

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

    IEnumerator animateLeftStick() {
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
    }

    IEnumerator animateLeftStick(Vector3 dir) {
        if (moveCount < 50) { 
            LStrans.Translate(dir * Time.deltaTime * 20);
            moveCount++;
        }
        else if (moveCount < 100) {
            LStrans.Translate(-dir * Time.deltaTime * 20);
            moveCount++;
        } else {
            Debug.Log(LScheck);
            LStrans.localPosition = LSpos;
        }
        yield return null;
    }

    IEnumerator animateLeftStickTR(Vector3 dir)
    {
        LSmoving = true;
        moveStick(dir);
        yield return null;
    }

    IEnumerator animateLeftStickTL(Vector3 dir)
    {
        LSmoving = true;
        moveStick(dir);
        yield return null;
    }

    IEnumerator animateLeftStickBR(Vector3 dir)
    {
        LSmoving = true;
        moveStick(dir);
        yield return null;
    }

    IEnumerator animateLeftStickBL(Vector3 dir)
    {
        LSmoving = true;
        moveStick(dir);
        yield return null;
    }

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
    }
}