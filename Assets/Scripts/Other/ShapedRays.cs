using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SegmentRay {
    public Transform start;
    public Transform end;

    public Vector3 direction {
        get => (end.position - start.position);
    }

    public float distance {
        get => (direction.magnitude);
    }

    public virtual void Draw () {
        Gizmos.DrawLine (start.position, end.position);
    }

    public virtual bool Raycast (LayerMask layerMask, out RaycastHit2D hit) {
        hit = Physics2D.Raycast (start.position, direction, distance, layerMask);
        return hit.collider != null;
    }

    public virtual bool Raycast (LayerMask layerMask) => Raycast (layerMask, out RaycastHit2D hit);

    public bool Raycast () => Raycast (Physics2D.DefaultRaycastLayers);
}

[System.Serializable]
public class BoxRay : SegmentRay {
    public override void Draw () {
        Gizmos.DrawWireCube (Vector3.Lerp (start.position, end.position, 0.5f), direction);
    }

    public override bool Raycast (LayerMask layerMask) {
        return Physics2D.OverlapArea (start.position, end.position, layerMask) != null;
    }
}
