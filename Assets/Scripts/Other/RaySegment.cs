using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct RaySegment {
    public Transform start;
    public Transform end;

    public Vector3 direction {
        get => (end.position - start.position);
    }

    public float distance {
        get => (direction.magnitude);
    }

    public void DebugDraw (Color color) {
        Debug.DrawLine (start.position, end.position, color);
    }

    public void DebugDraw () => DebugDraw (Color.white);

    public RaycastHit2D Raycast (LayerMask layerMask) {
        return Physics2D.Raycast (start.position, direction, distance, layerMask);
    }

    public RaycastHit2D Raycast () => Raycast (Physics2D.DefaultRaycastLayers);
}
