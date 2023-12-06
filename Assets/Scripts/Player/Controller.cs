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
    private float playerSpeedConst = 0;
    public float jumpPower = 0;
    private int groundLayer = 3;
    public Transform[] rayOrigins;
    private bool lastDeg;
    public List<string> movingTags;
    public static bool control; // Variable for cutscenes => Turn off/on movement ability
    private bool alive;
    private Animator deathAnim;

    
    // Effects && Sounds
    public GameObject GroundEffect;

    void Awake() {
        // Input init
        controls = new Gameplay();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        control = lastDeg = alive = true;
        groundLayer = LayerMask.GetMask("Ground");
        playerSpeedConst = playerSpeed;
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
            GroundCheck();
            MovePlayer();
            if (!isGrounded) anim.Play("Jump");
        }
        else {
            anim.Play("Idle");
            rb.velocity = Vector2.zero;
        }
    }

    // Movement inputs init
    private void OnEnable() {
        controls.Enable();
        controls.Player.Move.performed += OnMovePerformed;
        controls.Player.Move.canceled += OnMoveCanceled;
        controls.Player.Jump.performed += PlayerJump;
        controls.Player.Fire.performed += gun.PlayerFire;
        controls.Player.Exit.performed += menuDrop.OpenPanel;
        controls.Player.Program.performed += programming.OpenProgrammingPanel;
    }

    private void OnDisable() {
        controls.Disable();
        controls.Player.Move.performed -= OnMovePerformed;
        controls.Player.Move.canceled -= OnMoveCanceled;
        controls.Player.Jump.performed -= PlayerJump;
        controls.Player.Exit.performed -= menuDrop.OpenPanel;
        controls.Player.Program.performed -= programming.OpenProgrammingPanel;
    }

    private void OnMovePerformed(InputAction.CallbackContext value) => movement = value.ReadValue<Vector2>();
    private void OnMoveCanceled(InputAction.CallbackContext value) => movement = Vector2.zero;
    
    
    
    void PlayerJump(InputAction.CallbackContext value) {
        if (isGrounded && control)  {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }
    }

    private void MovePlayer()
    {
        if (movement.x > 0 && lastDeg) Rotation();
        if (movement.x < 0 && !lastDeg) Rotation();
        if (movement.x != 0) {
            rb.velocity = new Vector2(movement.x * playerSpeed * Time.fixedDeltaTime * 10, rb.velocity.y);
            if (isGrounded) anim.Play("Run");
        }
        else {
            rb.velocity = new Vector2(0, rb.velocity.y);
            if (isGrounded) anim.Play("Idle");
        }
    }

    private void GroundCheck()
    {
        RaycastHit2D hit1;
        RaycastHit2D hit2;
        RaycastHit2D hit3;
        float distance = 0.2f;

        hit1 = Physics2D.Raycast(new Vector2(rayOrigins[0].position.x, rayOrigins[0].position.y), Vector2.down, distance, groundLayer);
        hit2 = Physics2D.Raycast(new Vector2(rayOrigins[1].position.x, rayOrigins[1].position.y), Vector2.down, distance, groundLayer);
        hit3 = Physics2D.Raycast(new Vector2(rayOrigins[2].position.x, rayOrigins[2].position.y), Vector2.down, distance, groundLayer);
        
        if (hit1.collider != null || hit2.collider != null || hit3.collider != null)
        {
            isGrounded = true;
            playerSpeed = playerSpeedConst;
        }
        else 
        {
            isGrounded = false;
            rb.velocity = new Vector2(0, rb.velocity.y);
            playerSpeed = playerSpeedConst / 1.8f;
        }
    }

    private void Rotation()
    {
        lastDeg = !lastDeg;
        transform.Rotate(Vector2.up * 180);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == 3)
        {
            rb.velocity = Vector2.zero;
            Instantiate(GroundEffect, rayOrigins[3].transform.position, Quaternion.identity);
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
}