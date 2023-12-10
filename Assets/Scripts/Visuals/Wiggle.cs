using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiggle : MonoBehaviour
{
    public Vector3 range = Vector3.one;
    public float frequency = 5;
    public float strength = 1;

    private Vector3 origPos;
    private Vector3 startPos;
    private Vector3 destPos;
    private float movementStartTime;

    void Start () {
        origPos = transform.localPosition;
        startPos = origPos;
        movementStartTime = Time.time;
        destPos = RandomDest ();
    }

    void Update () {
        if (frequency == 0 || strength == 0 || range.sqrMagnitude == 0) {
            transform.localPosition = origPos;
            enabled = false;
            return;
        }

        float u = (Time.time - movementStartTime) * frequency;

        if (u >= 1) {
            transform.localPosition = destPos;
            startPos = destPos;
            movementStartTime = Time.time;
            destPos = RandomDest ();
            return;
        }

        transform.localPosition = Vector3.Lerp (startPos, destPos, u);
    }

    Vector3 RandomDest () {
        return Vector3.Scale (Random.insideUnitSphere, range) * strength;
    }
}