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
    public bool withButton;
    private int normalSpeed;
    private bool continueMoving = true;

    void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        startPos = transform.position;
        towardPoint = startPos + moveVector;
        isForward = continueMoving = true;
        if (!withButton) StartCoroutine(Move());
    }

    public void StartMoving() {
        continueMoving = true;
        StartCoroutine(Move());
    }

    public void StopMoving() {
        if (withButton) continueMoving = false;
    }

    IEnumerator Move() {
        while (continueMoving) {
            if (new Vector2(transform.position.x, transform.position.y) == towardPoint) {
                isForward = false;
            }
            else if (new Vector2(transform.position.x, transform.position.y) == startPos) {
                isForward = true;
            }

            if (isForward) rb.MovePosition(Vector2.MoveTowards(transform.position, towardPoint, speed * Time.fixedDeltaTime));
            else rb.MovePosition(Vector2.MoveTowards(transform.position, startPos, speed * Time.fixedDeltaTime));
            yield return null;
        }
    }
}