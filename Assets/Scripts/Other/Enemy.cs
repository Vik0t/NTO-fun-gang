using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Char")]
    [SerializeField] private int speed = 150;
    [SerializeField] private int lookRange = 10;
    [SerializeField] private float reloadTime = 1.5f;
    [SerializeField] private bool OnlyNamprolom = false;
    [SerializeField] private bool isStayig = false;
    [SerializeField] private bool isFlying = false;
    
    [Header("Bullet")]
    public GameObject bullet;
    public GameObject bulletSound;
    public Transform bulletPos;
    public GameObject deathParticle;

    private Animator anim;
    private Rigidbody2D rb;
    private int dir;
    private bool isReadyToFire = true;

    public Transform origin;

    void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        
        if (isFlying) rb.gravityScale = 0;
        else rb.gravityScale = 1;

        if (transform.rotation.y == 0) dir = 1;
        else dir = -1;
    }

    void FixedUpdate() {
        if (OnlyNamprolom) Move();
        else {
            RaycastHit2D[] hitLeft;
            RaycastHit2D[] hitRight;
            hitLeft = Physics2D.RaycastAll(origin.position, Vector2.left, lookRange); 
            hitRight = Physics2D.RaycastAll(origin.position, Vector2.right, lookRange);
            
            if (hitLeft.Length > 1) {
                for (int i = 0; i < hitLeft.Length; i++) {
                    GameObject leftObj = hitLeft[i].transform.gameObject;
                    if (leftObj.GetComponent<Controller>() != null || leftObj.GetComponent<GroundBot>() != null || leftObj.GetComponent<FlightBot>() != null) {
                        if (dir == 1) Rotate();
                        if (!isStayig) Move();
                        if (isReadyToFire) Fire();
                    }
                }                
            }
            if (hitRight.Length > 1) {
                for (int i = 0; i < hitRight.Length; i++) {
                    GameObject rightObj = hitRight[i].transform.gameObject;
                    if (rightObj.GetComponent<Controller>() != null || rightObj.gameObject.GetComponent<GroundBot>() != null || rightObj.GetComponent<FlightBot>() != null) {
                        if (dir == -1) Rotate();
                        if (!isStayig) Move();
                        if (isReadyToFire) Fire();
                    }
                }
            }
        }
    }

    private void Rotate() {
        transform.Rotate(Vector2.up * 180);
        dir *= -1;
    }

    private void Move() {
        rb.velocity = new Vector2(speed * dir * Time.fixedDeltaTime, rb.velocity.y); 
    }

    public void Fire() {
        isReadyToFire = false;
        StartCoroutine(Reload());
        Instantiate(bullet, bulletPos.position, bulletPos.transform.rotation);
        Instantiate(bulletSound);
    }

    IEnumerator Reload() {
        yield return new WaitForSeconds(reloadTime);
        isReadyToFire = true;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "PlayerBullet") {
            Instantiate(deathParticle, origin.position, origin.rotation);
            Destroy(gameObject);
        }
    }
}
