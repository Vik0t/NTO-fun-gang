using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour {
    public GameObject particle;
    public float bulletSpeed;

    private Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D> ();
        StartCoroutine(DestroyTime(0.8f));
    }
    void Update() {
        rb.velocity = transform.right * bulletSpeed;
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(particle,transform.position, transform.rotation);
        Destroy(gameObject);
    }

    IEnumerator DestroyTime(float t) {
        yield return new WaitForSeconds(t);
        Instantiate(particle, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}