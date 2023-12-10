using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPhysics : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.GetComponent<Platform>() != null) {
            transform.SetParent(collision.transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.GetComponent<Platform>() != null) {
            transform.parent = null;
        }
    }
}
