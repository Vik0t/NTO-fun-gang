using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Muratich {
public class GroundBot : MonoBehaviour
    {
        public float speed;
        private int groundLayer;
        private Rigidbody2D rb;
        private Animator anim;
        public Transform origin;
        private int dir;
        public List<string> commandList;

        void Start() {
            groundLayer = LayerMask.GetMask("Ground");
            rb = gameObject.GetComponent<Rigidbody2D>();
            anim = gameObject.GetComponent<Animator>();
            
            if (transform.rotation.y == 0) dir = 1;
            else dir = -1;

            //AcceptCommandList(commandList);
            StartCoroutine(MainStart());
        }

        IEnumerator MainStart() {
            yield return Move();
            yield return Rotate();
            yield return Move();
        }

        public void ToAcceptCommandList(List<string> commands) {
            StartCoroutine(AcceptCommandList(commandList));
        }

        IEnumerator AcceptCommandList(List<string> commands) {
            for (int i = 0; i < commands.Count; i++) {
                switch (commands[i]) {
                    case "Move":
                        yield return Move();
                        break;
                    case "Rotate":
                        yield return Rotate();
                        break;
                }
            }
        }

        IEnumerator Rotate() {
            transform.Rotate(Vector2.up * 180);
            dir *= -1;
            rb.velocity = new Vector2(speed * dir * 1, rb.velocity.y);
            yield return null;
        }

        IEnumerator Move() {
            anim.Play("Run");
            RaycastHit2D hit;

            while (true) {
                hit = Physics2D.Raycast(Vector2.left, Vector2.right, 1, groundLayer); 
                if (dir == -1) hit = Physics2D.Raycast(Vector2.left, Vector2.right, 1, groundLayer); 
                else if (dir == 1) hit = Physics2D.Raycast(Vector2.right, Vector2.right, 1, groundLayer); 
                
                if (hit.collider != null) {
                    anim.Play("Idle"); 
                    Debug.Log("Broken");
                    break;
                }
                rb.velocity = new Vector2(speed * dir, rb.velocity.y);
                yield return null;
            }
        }
    }
}