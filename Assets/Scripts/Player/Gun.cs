using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour {
    public GameObject bullet;
    public Transform fireOrigin;
    public float reloadTime;
    private bool isReady = true;
    public GameObject fireSound;

    private Animator anim;

    void Start() {
        anim = GetComponent<Animator>();
    }

    IEnumerator Reload() {
        anim.Play("Weapon.Fire");
        Instantiate(fireSound);
        Instantiate(bullet, fireOrigin.position, Quaternion.FromToRotation (Vector3.right, transform.lossyScale.x * transform.right));
        yield return new WaitForSeconds(reloadTime);
        isReady = true;
    }

    public void PlayerFire(InputAction.CallbackContext value) {
        if (isReady && Controller.control) {
            isReady = false;
            StartCoroutine(Reload());
        }
    }
}