using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Muratich {
public class GroundBot : MonoBehaviour
    {
        public float speed;
        private bool OnMove;
        private int GroundLayer;
        private Rigidbody2D rb;
        private Animator anim;
        public Transform origin;

        void Start() {
            GroundLayer = LayerMask.GetMask("Ground");
            rb = gameObject.GetComponent<Rigidbody2D>();
            anim = gameObject.GetComponent<Animator>();
            MoveTowards();
        }

        public void MoveTowards() {
            RaycastHit2D hit1;
            RaycastHit2D hit2;
            
            anim.Play("Run");
            hit1 = Physics2D.Raycast(origin.position, Vector2.left, 1, GroundLayer);
            hit2 = Physics2D.Raycast(origin.position, Vector2.right, 1, GroundLayer);
            StartCoroutine(ToMove(hit1, hit2));
        }

        IEnumerator ToMove(RaycastHit2D h1, RaycastHit2D h2) {
            h1 = Physics2D.Raycast(origin.position, Vector2.left, 1, GroundLayer);
            h2 = Physics2D.Raycast(origin.position, Vector2.right, 1, GroundLayer);
            rb.velocity = new Vector2(speed, rb.velocity.y);
            yield return new WaitForSeconds(0.1f);

            if (h1 != null && h2 != null) {
                Debug.Log("No");
                anim.Play("Idle");
                StopCoroutine(ToMove(h1,h2));
            }
            else {
                Debug.Log("Yes");
                StartCoroutine(ToMove(h1,h2));
            }
        }
    }
}