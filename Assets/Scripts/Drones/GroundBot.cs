using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Muratich {
public class GroundBot : MonoBehaviour
    {
        public float speed;
        private int groundLayer;
        private int interactiveLayer;
        private Rigidbody2D rb;
        private Animator anim;
        public Transform origin;
        public Transform PickUpOrigin;
        private int dir;
        public List<int> commandList;
        public int jumpForce;
        private bool alreadyCarryOn = false;

        void Start() {
            groundLayer = LayerMask.GetMask("Ground");
            interactiveLayer = LayerMask.GetMask("Interactive");
            rb = gameObject.GetComponent<Rigidbody2D>();
            anim = gameObject.GetComponent<Animator>();
            
            if (transform.rotation.y == 0) dir = 1;
            else dir = -1;

            StartDoCommand(commandList);
        }

        public void StartDoCommand(List<int> commands) {
            StartCoroutine(AcceptCommandList(commandList));
        }

        IEnumerator AcceptCommandList(List<int> commands) {
            for (int i = 0; i < commands.Count; i++) {
                switch (commands[i]) {
                    case 0:
                        yield return Move();
                        break;
                    case 1:
                        yield return Rotate();
                        break;
                    case 2:
                        yield return Jump();
                        break;
                    case 3:
                        yield return Pick();
                        break;
                    case 4:
                        yield return Put();
                        break;
                    case 5:
                        yield return Attack();
                        break;
                }
            }
        }

        IEnumerator Rotate() {
            transform.Rotate(Vector2.up * 180);
            dir *= -1;
            rb.velocity = new Vector2(speed * dir * 1.5f, rb.velocity.y);
            yield return null;
        }

        IEnumerator Move() {
            anim.Play("Run");
            RaycastHit2D hit;

            while (true) {
                if (dir == 1) hit = Physics2D.Raycast(origin.position, Vector2.right, 0.5f, groundLayer); 
                else hit = Physics2D.Raycast(origin.position, Vector2.left, 0.5f); 

                if (hit.collider != null) {
                    anim.Play("Idle"); 
                    Debug.Log("Ended");
                    break;
                }
                rb.velocity = new Vector2(speed * dir, rb.velocity.y);
                yield return null;
            }
        }

        IEnumerator Jump() {
            rb.velocity = new Vector2(speed * dir * 1.5f, jumpForce);
            yield return null;
        }

        IEnumerator Pick() {
            RaycastHit2D hit;
            if (dir == 1) hit = Physics2D.Raycast(origin.position, Vector2.right, 0.5f, groundLayer); 
            else hit = Physics2D.Raycast(origin.position, Vector2.left, 0.5f, groundLayer);

            if (hit.collider.transform.gameObject.tag == "Pick" && !alreadyCarryOn) {
                alreadyCarryOn = false;
                hit.transform.position = PickUpOrigin.transform.position;
                hit.transform.parent = PickUpOrigin.transform;
            }
            yield return null;
        }

        IEnumerator Put() {
            yield return null;
        }

        IEnumerator Attack() {
            yield return null;
        }
    }
}