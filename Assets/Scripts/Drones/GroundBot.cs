using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class GroundBot : MonoBehaviour
{
    [Header("Specs")]
    public float speed;
    public float jumpForce;
    public float maxLiftableWeight = 10.0f;
    public bool isBeatable = true;
    private int groundLayer;
    private int interactiveLayer;
    private Rigidbody2D rb;
    private Animator anim;
    [Header("Transforms")]
    public Transform origin;
    public Transform pickUpOrigin;
    public Transform putOrigin;
    private int dir;
    private bool alreadyCarryOn = false;
    [Header("Weapon specs")]
    public bool canAttack = true;
    public GameObject bullet;
    public GameObject bulletSound;

    void Start() {
        groundLayer = LayerMask.GetMask("Ground");
        interactiveLayer = LayerMask.GetMask("Interactive");
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        
        if (transform.rotation.y == 0) dir = 1;
        else dir = -1;
    }

    public void StartDoCommands(List<int> commands) {
        StartCoroutine(AcceptCommandList(commands));
    }

    IEnumerator AcceptCommandList(List<int> commands) {
        foreach (int i in commands) {
            switch (i) {
                case (int)BotCommands.Move:
                    yield return Move();
                    break;
                case (int)BotCommands.Rotate:
                    yield return Rotate();
                    break;
                case (int)BotCommands.Jump:
                    yield return Jump();
                    break;
                case (int)BotCommands.Pick:
                    yield return Pick();
                    break;
                case (int)BotCommands.Put:
                    yield return Put();
                    break;
                case (int)BotCommands.Attack:
                    yield return Attack();
                    break;
            }
        }
    }

    IEnumerator Rotate() {
        transform.Rotate(Vector2.up * 180);
        dir *= -1;
        yield return null;
    }

    IEnumerator Move() {
        anim.Play("Run");
        RaycastHit2D hit;

        while (true) {
            if (dir == 1) hit = Physics2D.Raycast(origin.position, Vector2.right, 1, groundLayer); 
            else hit = Physics2D.Raycast(origin.position, Vector2.left, 1, groundLayer); 

            if (hit.collider != null && hit.transform.gameObject.name != gameObject.name) {
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
        GameObject hitObject = hit.transform.gameObject;
        var hitObjectSpecs = hitObject.GetComponent<ObjectConfig>();

        if (hit.collider != null && hitObject.name != gameObject.name) {
            if (hitObjectSpecs.IsPickable && !alreadyCarryOn && hitObjectSpecs.weight <= maxLiftableWeight) {
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
        if (canAttack)
        {
            Instantiate(bulletSound);
            Instantiate(bullet, putOrigin.position, putOrigin.rotation);
            yield return new WaitForSeconds(1f);
        }
    }
}
