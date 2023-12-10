using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlightBot : MonoBehaviour
{
    public float speed;
    public LayerMask groundLayer;
    public float maxLiftableWeight = 10.0f;
    public bool isBeatable;
    private Rigidbody2D rb;
    private Animator anim;
    public Transform origin;
    public Transform fireOrigin;
    public Transform pickUpOrigin;
    public Transform putOrigin;
    private int dir;
    private bool alreadyCarryOn = false;
    public GameObject bullet;
    public GameObject bulletSound;
    public GameObject[] deathEffectsAndSounds;
    private Animator deathAnim;

    private GameObject carriedObject;
    private Rigidbody2D carriedRigidbody;
    public float carryingDistance = 2f;
    public GameObject pickupBeam;

    // Conditions
    private bool isFounded = false;
    private bool isStartedCondition = false;

    void Start() {
        deathAnim = GameObject.FindGameObjectWithTag("DeathAnim").GetComponent<Animator>();
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
            if (isStartedCondition) {
                if (!isFounded && i != (int)BotCommands.BreakIf) continue;
            }
            switch (i) {
                case (int)BotCommands.Move:
                    yield return Move();
                    break;
                case (int)BotCommands.Rotate:
                    yield return Rotate();
                    break;
                case (int)BotCommands.Up:
                    yield return Vertical(true);
                    break;
                case (int)BotCommands.Down:
                    yield return Vertical(false);
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
                case (int)BotCommands.IfEnemy:
                    isStartedCondition = true;
                    yield return Scan("Enemy");
                    break;
                case (int)BotCommands.IfWall:
                    isStartedCondition = true;
                    yield return Scan("Wall");
                    break;
                case (int)BotCommands.IfBox:
                    isStartedCondition = true;
                    yield return Scan("Box");
                    break;
                case (int)BotCommands.IfBot:
                    isStartedCondition = true;
                    yield return Scan("Bot");
                    break;
                case (int)BotCommands.BreakIf:
                    isStartedCondition = false;
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
        RaycastHit2D[] hit;
        bool tauched = false;
        while (true) {
            if (dir == 1) hit = Physics2D.RaycastAll(origin.position, Vector2.right, alreadyCarryOn ? carryingDistance : 1); 
            else hit = Physics2D.RaycastAll(origin.position, Vector2.left, alreadyCarryOn ? carryingDistance : 1); 

            for (int i = 0; i < hit.Length; i++) {
                if (hit[i].transform.gameObject != gameObject && hit[i].transform.gameObject != carriedObject) {
                    tauched = true;
                    break;
                }
                rb.velocity = new Vector2(speed * dir * Time.fixedDeltaTime, rb.velocity.y);
            }
            yield return null;
            if (tauched) break;
        }
        rb.velocity = Vector2.zero;
    }

    IEnumerator Vertical(bool OnUp) {
        rb.velocity = Vector2.zero;
        RaycastHit2D[] hit;
        int verticalDir = 0;
        bool tauched = false;

        if (OnUp) verticalDir = 1;
        else verticalDir = -1;

        while (true) {
            if (OnUp) hit = Physics2D.RaycastAll(origin.position, Vector2.up, 0.35f); 
            else hit = Physics2D.RaycastAll(origin.position, Vector2.down, 0.35f); 

            for (int i = 0; i < hit.Length; i++) {
                if (hit[i].transform.gameObject != gameObject) {
                    tauched = true;
                    break;
                }
                rb.velocity = new Vector2(rb.velocity.x, speed * verticalDir * Time.fixedDeltaTime);
                yield return null;
            }
            if (tauched) break;
        }
        rb.velocity = Vector2.zero;
    }

    IEnumerator Pick() {
        RaycastHit2D[] hit;
        if (dir == 1) hit = Physics2D.RaycastAll(origin.position, Vector2.right, 1); 
        else hit = Physics2D.RaycastAll(origin.position, Vector2.left, 1);
        
        if (hit.Length > 1) {
            for (int i = 0; i < hit.Length; i++){
                GameObject obj = hit[i].transform.gameObject;
                if (obj.GetComponent<ObjectConfig>() != null) {
                    ObjectConfig objConfig = obj.GetComponent<ObjectConfig>();

                    if (objConfig.IsPickable && !alreadyCarryOn && objConfig.weight <= maxLiftableWeight) {
                        alreadyCarryOn = true;
                        pickupBeam.SetActive (true);
                        carriedObject = obj;
                        carriedRigidbody = carriedObject.GetComponent<Rigidbody2D> ();
                        obj.transform.position = pickUpOrigin.position;
                        obj.transform.parent = pickUpOrigin;
                        break;
                    }
                }
            }
        }
        yield return null;
    }

    void Update () {
        if (carriedRigidbody != null) carriedRigidbody.velocity = Vector2.zero;
        if (carriedObject != null) carriedObject.transform.localPosition = Vector3.zero;
        anim.SetFloat ("Speed", Mathf.Abs(rb.velocity.x));
    }

    IEnumerator Put() {
        if (alreadyCarryOn) {
            pickupBeam.SetActive (false);
            Transform child = carriedObject.transform;
            child.position = putOrigin.position;
            child.parent = null;
            alreadyCarryOn = false;
            carriedObject = null;
            carriedRigidbody = null;
        }
        yield return null;
    }

    IEnumerator Attack() {
        Instantiate(bulletSound);
        Instantiate(bullet, fireOrigin.position, fireOrigin.rotation);
        anim.Play ("Weapon.Fire");
        yield return new WaitForSeconds(1f);
    }

    IEnumerator Scan(string type) {
        isFounded = false;
        RaycastHit2D[] hit;
        if (dir == 1) hit = Physics2D.RaycastAll(origin.position, Vector2.right, 1.5f); 
        else hit = Physics2D.RaycastAll(origin.position, Vector2.left, 1.5f);

        if (hit.Length > 1) {
            for (int i = 0; i < hit.Length; i++) {
                GameObject obj = hit[i].transform.gameObject;
                if (obj.name != gameObject.name) {
                    switch (type) {
                        case "Enemy":
                            if (obj.tag == "Respawn") isFounded = true;
                            break;
                        case "Wall":
                            if (obj.layer == 3) isFounded = true;
                            break;
                        case "Box":
                            if (obj.GetComponent<ObjectConfig>() != null) isFounded = true;
                            break;
                        case "Bot":
                            if (obj.GetComponent<GroundBot>() != null || obj.GetComponent<FlightBot>() != null) isFounded = true;
                            break;
                    }
                }
            }
        }
        yield return null;
    }
    
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Respawn") {
            if (isBeatable) StartCoroutine(DeathAnim());
        }
    }

    IEnumerator DeathAnim() {
        foreach (GameObject i in deathEffectsAndSounds) {
            Instantiate(i, transform.position, transform.rotation);
        }
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        deathAnim.Play("Close");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}