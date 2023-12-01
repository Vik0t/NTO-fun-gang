using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Muratich {
    public class Gun : MonoBehaviour {
        public GameObject bullet;
        public Transform fireOrigin;
        public float reloadTime;
        private Animator pistolAnim;
        public GameObject pistol;
        private bool isReady = true;
        public GameObject fireSound;

        void Start() {
            pistolAnim = pistol.gameObject.GetComponent<Animator>();
        }
    
        IEnumerator Reload() {
            pistolAnim.Play("Fire");
            Instantiate(fireSound);
            Instantiate(bullet, fireOrigin.position, gameObject.transform.rotation);
            yield return new WaitForSeconds(reloadTime);
            pistolAnim.Play("Idle");
            isReady = true;
        }

        public void PlayerFire(InputAction.CallbackContext value) {
            if (isReady && Controller.control) {
                isReady = false;
                StartCoroutine(Reload());
            }
        }
    }
}
