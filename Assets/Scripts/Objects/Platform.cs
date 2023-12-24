using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent (typeof (Rigidbody2D))]
public class Platform : MonoBehaviour
{
    public float speed;
    public List<Vector2> points;
    public bool loop = false;
    private Rigidbody2D rb;

    private int direction = 1;
    private int pointIndex = 0;
    private Vector2 fromPoint;
    private Vector2 toPoint;

    private float movementStartTime;
    private float movementDuration;
    private float localTime;

    private Vector2 lastPos;

    public Vector2 velocity {
        get => ((Vector2)transform.position - lastPos) / Time.fixedDeltaTime;
    }

    void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        if (points.Count > 0) {
            fromPoint = toPoint = points[0];
            SetPosition (fromPoint);
        }
        lastPos = transform.position;
    }

    public void FixedUpdate () {
        lastPos = transform.position;
        localTime += Time.fixedDeltaTime;
        float progress = (localTime - movementStartTime) / movementDuration;
        if (fromPoint != toPoint) rb.MovePosition (Vector2.Lerp (fromPoint, toPoint, progress));
        if (progress < 1) return;
        UpdatePoints ();
        movementStartTime = localTime;
        movementDuration = Vector3.Distance (fromPoint, toPoint) / speed;
    }

    private void UpdatePoints () {
        if (points.Count < 2) return;
        fromPoint = points[pointIndex];

        pointIndex += direction;
        if (pointIndex < 0 || pointIndex >= points.Count) {
            if (loop) {
                pointIndex %= points.Count;
                if (pointIndex < 0) pointIndex += points.Count; //C# сука
            } else {
                direction = -direction;
                pointIndex += direction * 2;
            }
        }

        toPoint = points[pointIndex];
    }

    private void SetPosition (Vector2 pos) {
        Vector3 position = pos;
        position.z = transform.position.z;
        transform.position = position;
    }

    public void StartMoving () {
        enabled = true;
    }

    public void StopMoving () {
        enabled = false;
    }

    private void OnDrawGizmos () {
        if (points.Count < 1) return;
        Gizmos.color = Color.green;

        Vector3 prevPoint = points[loop ? points.Count - 1 : 0];
        
        foreach (Vector3 point in points) {
            Gizmos.DrawWireSphere (point, 0.25f);
            Gizmos.DrawLine (prevPoint, point);
            prevPoint = point;
        }
    }
}