using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantEffect : MonoBehaviour
{
    public GameObject obj;
    public Transform point;
    public float time;

    void Start() {
        StartCoroutine(Instant());
    }

    IEnumerator Instant() {
        yield return new WaitForSeconds(time);
        Instantiate(obj, point.position, point.rotation);
        StartCoroutine(Instant());
    }
}
