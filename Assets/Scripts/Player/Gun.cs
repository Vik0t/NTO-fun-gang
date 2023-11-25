using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Muratich {
    public class Gun : MonoBehaviour {
        public GameObject bullet;
        public Transform fireOrigin;
        public float ReloadTime;
        private Animator pistolAnim;
        public GameObject pistol;
        private bool IsReady = true;
        public GameObject FireSound;

        void Start() {
            pistolAnim = pistol.gameObject.GetComponent<Animator>();
        }
    
        IEnumerator Reload() {
            pistolAnim.Play("Fire");
            Instantiate(FireSound);
            Instantiate(bullet, new Vector2(fireOrigin.position.x, fireOrigin.position.y) , gameObject.transform.rotation);
            yield return new WaitForSeconds(ReloadTime);
            pistolAnim.Play("Idle");
            IsReady = true;
        }

        public void PlayerFire(InputAction.CallbackContext value) {
            if (IsReady && Controller.Control) {
                IsReady = false;
                StartCoroutine(Reload());
            }
        }
    }
}
