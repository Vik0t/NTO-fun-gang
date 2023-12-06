using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Platform : MonoBehaviour
{
    public float speed;
    public Vector2 moveVector;
    private Rigidbody2D rb;
    private Vector2 startPos;
    private Vector2 towardPoint;
    private bool isForward;

    void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        startPos = transform.position;
        towardPoint = startPos + moveVector;
        isForward = true;
    }

    void FixedUpdate() {
        if (new Vector2(transform.position.x, transform.position.y) == towardPoint) {
            isForward = false;
        }
        else if (new Vector2(transform.position.x, transform.position.y) == startPos) {
            isForward = true;
        }

        if (isForward) rb.MovePosition(Vector2.MoveTowards(transform.position, towardPoint, speed * Time.fixedDeltaTime));
        else rb.MovePosition(Vector2.MoveTowards(transform.position, startPos, speed * Time.fixedDeltaTime));
    }
}