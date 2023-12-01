using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace Muratich {
    public enum Commands
    {
        Move,
        Rotate,
        Jump,
        Pick,
        Put,
        Attack
    }

    public class GroundBot : MonoBehaviour
    {
        
        public float speed;
        public float jumpForce;
        private int groundLayer;
        private int interactiveLayer;
        private Rigidbody2D rb;
        private Animator anim;
        public Transform origin;
        public Transform pickUpOrigin;
        public Transform putOrigin;
        private int dir;
        public List<int> commandList;
        private bool alreadyCarryOn = false;
        public GameObject bullet;
        public GameObject bulletSound;

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
                    case (int)Commands.Move:
                        yield return Move();
                        break;
                    case (int)Commands.Rotate:
                        yield return Rotate();
                        break;
                    case (int)Commands.Jump:
                        yield return Jump();
                        break;
                    case (int)Commands.Pick:
                        yield return Pick();
                        break;
                    case (int)Commands.Put:
                        yield return Put();
                        break;
                    case (int)Commands.Attack:
                        yield return Attack();
                        break;
                }
            }
        }

        IEnumerator Rotate() {
            transform.Rotate(Vector2.up * 180);
            dir *= -1;
            rb.velocity = new Vector2(speed * dir * 1.5f * Time.fixedDeltaTime, rb.velocity.y);
            yield return null;
        }

        IEnumerator Move() {
            anim.Play("Run");
            RaycastHit2D hit;

            while (true) {
                if (dir == 1) hit = Physics2D.Raycast(origin.position, Vector2.right, 1, groundLayer); 
                else hit = Physics2D.Raycast(origin.position, Vector2.left, 1, groundLayer); 

                if (hit.collider != null) {
                    anim.Play("Idle");
                    break;
                }
                rb.velocity = new Vector2(speed * dir * Time.fixedDeltaTime, rb.velocity.y);
                yield return null;
            }
        }

        IEnumerator Jump() {
            rb.velocity = new Vector2(jumpForce * dir * 0.5f, jumpForce);
            while (rb.velocity != Vector2.zero) yield return null;
        }

        IEnumerator Pick() {
            RaycastHit2D hit;
            if (dir == 1) hit = Physics2D.Raycast(origin.position, Vector2.right, 1, groundLayer); 
            else hit = Physics2D.Raycast(origin.position, Vector2.left, 1, groundLayer);
            
            if (hit.collider != null) {
                if (hit.transform.gameObject.tag == "Pick" && !alreadyCarryOn) {
                    alreadyCarryOn = true;
                    hit.transform.position = pickUpOrigin.position;
                    hit.transform.parent = pickUpOrigin;
                }
            }
            yield return null;
        }

        IEnumerator Put() {
            if (alreadyCarryOn) {
                Transform child = pickUpOrigin.GetChild(0).gameObject.transform;
                child.position = putOrigin.position;
                child.parent = null;
                alreadyCarryOn = false;
            }
            yield return null;
        }

        IEnumerator Attack() {
            Instantiate(bulletSound);
            Instantiate(bullet, putOrigin.position, putOrigin.rotation);
            yield return new WaitForSeconds(1f);
        }
    }
}