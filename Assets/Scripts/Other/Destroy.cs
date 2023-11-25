using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public float time;

    void Start() => StartCoroutine(DestroyCor());

    IEnumerator DestroyCor() {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
