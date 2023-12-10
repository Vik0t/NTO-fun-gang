using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Cinemachine;

public class Controller : MonoBehaviour
{
    // Input system
    private Gameplay controls = null;
    private Vector2 movement = Vector2.zero;


    // Components
    private Animator anim;
    private Rigidbody2D rb = null;
    private Gun gun;
    private MenuDrop menuDrop;
    private ProgrammingPanelOpen programming;
    private CinemachineVirtualCamera cvm;


    // Movement
    private bool isGrounded;
    public float playerSpeed = 0;
    [Tooltip ("How much player speed is reduced in the air")] public float airSpeedMod = 0.56f;
    public float jumpPower = 0;
    public LayerMask groundLayer;
    public List<string> movingTags;
    public static bool control; // Variable for cutscenes => Turn off/on movement ability
    private bool alive;
    private Animator deathAnim;

    public Transform particleOrigin;
    public RaySegment groundRay;
    public RaySegment[] stepRays;

    [Tooltip ("Max time beetween input and jump")] public float bufferingTime = 0.1f;
    public float stepHeight = 0.1f;
    private float jumpPressTime = float.NegativeInfinity;

    
    // Effects && Sounds
    public GameObject GroundEffect;

    void Awake() {
        // Input init
        controls = new Gameplay();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        control = alive = true;
        gun = gameObject.GetComponent<Gun>();
        menuDrop = GameObject.FindGameObjectWithTag("MenuDrop").GetComponent<MenuDrop>();
        programming = GameObject.FindGameObjectWithTag("ProgrammingOpener").GetComponent<ProgrammingPanelOpen>();
        cvm = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        cvm.Follow = gameObject.transform;
        deathAnim = GameObject.FindGameObjectWithTag("DeathAnim").GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (control && alive)
        {
            TryJump ();
            ApplyStep ();
            GroundCheck ();
            MovePlayer ();    
        }
        else {
            anim.SetBool ("IsMoving", false);
            rb.velocity = Vector2.zero;
        }
        anim.SetFloat ("VerticalSpeed", rb.velocity.y);
        anim.SetBool ("IsGrounded", isGrounded);
    }

    private void OnMovePerformed(InputAction.CallbackContext value) => movement = value.ReadValue<Vector2>();
    private void OnMoveCanceled(InputAction.CallbackContext value) => movement = Vector2.zero;
    
    
    
    void TryJump () {
        if (isGrounded && (Time.time - jumpPressTime) < bufferingTime)  {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }
    }

    private void MovePlayer () {
        if (movement.x != 0) {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign (movement.x);
            transform.localScale = scale;
        }
        anim.SetBool ("IsMoving", movement.x != 0);
        rb.velocity = new Vector2(movement.x * playerSpeed * Time.fixedDeltaTime * 10 * (isGrounded ? 1 : airSpeedMod), rb.velocity.y);
    }

    private void GroundCheck () {
        isGrounded = Physics2D.OverlapBox (groundRay.start.position, groundRay.direction, 0, groundLayer) != null;
    }

    public void ApplyStep () {
        float step = CalulateStep ();
        if (step > stepHeight) return;
        transform.Translate (Vector3.up * step);
        if (step > 0) rb.velocity = Vector2.zero;
    }

    private float CalulateStep () {
        float step = 0;
        foreach (RaySegment segment in stepRays) {
            RaycastHit2D hit = segment.Raycast (groundLayer);
            if (hit.collider != null) step = Mathf.Max (step, segment.distance - hit.distance);
        }
        return step;
    }

    void OnDrawGizmos () {
        groundRay.DebugDraw (Color.blue);
        foreach (RaySegment segment in stepRays) {
            segment.DebugDraw (new Color (0.2f, 1f, 0.2f));
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (((groundLayer >> collision.gameObject.layer) & 1) == 1) {
            Instantiate(GroundEffect, particleOrigin.transform.position, Quaternion.identity);
        }
        
        if (movingTags.Contains(collision.gameObject.tag)) {
            gameObject.transform.parent = collision.transform;
        }

        if (collision.gameObject.tag == "Respawn") {
            alive = false;
            StartCoroutine(DeathAnim());
        }
    }

    IEnumerator DeathAnim() {
        deathAnim.Play("Close");
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (movingTags.Contains(collision.gameObject.tag)) {
            gameObject.transform.parent = null;
        }
    }

    void PressJump (InputAction.CallbackContext value) => jumpPressTime = Time.time;
    void ReleaseJump (InputAction.CallbackContext value) => jumpPressTime = float.NegativeInfinity;

    // Movement inputs init
    private void OnEnable() {
        controls.Enable();
        controls.Player.Move.performed += OnMovePerformed;
        controls.Player.Move.canceled += OnMoveCanceled;
        controls.Player.Jump.performed += PressJump;
        controls.Player.Jump.canceled += ReleaseJump;
        controls.Player.Fire.performed += gun.PlayerFire;
        controls.Player.Exit.performed += menuDrop.OpenPanel;
        controls.Player.Program.performed += programming.OpenProgrammingPanel;
    }

    private void OnDisable() {
        controls.Disable();
        controls.Player.Move.performed -= OnMovePerformed;
        controls.Player.Move.canceled -= OnMoveCanceled;
        controls.Player.Jump.performed -= PressJump;
        controls.Player.Jump.canceled -= ReleaseJump;
        controls.Player.Exit.performed -= menuDrop.OpenPanel;
        controls.Player.Program.performed -= programming.OpenProgrammingPanel;
    }
}