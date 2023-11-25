using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCameraMovement : MonoBehaviour
{
    public float speed;

    void Update () {
        Vector3 velocity = Vector3.zero;
        velocity.x = Input.GetAxis ("Horizontal") * speed;
        velocity.y = Input.GetAxis ("Vertical") * speed;
        transform.Translate (velocity);
    }
}
