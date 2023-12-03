using UnityEngine;
using System.Collections;
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
    public static bool control; // Variable for cutscenes => Turn off/on movement ability

    
    // Effects && Sounds
    public GameObject GroundEffect;

    void Awake() {
        // Input init
        controls = new Gameplay();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        control = lastDeg = true;
        groundLayer = LayerMask.GetMask("Ground");
        playerSpeedConst = playerSpeed;
        gun = gameObject.GetComponent<Gun>();
        menuDrop = GameObject.FindGameObjectWithTag("MenuDrop").GetComponent<MenuDrop>();
        programming = GameObject.FindGameObjectWithTag("ProgrammingOpener").GetComponent<ProgrammingPanelOpen>();
        cvm = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        cvm.Follow = gameObject.transform;
    }

    private void FixedUpdate()
    {
        if (control)
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
            //rb.AddForce(transform.up * jumpPower, ForceMode2D.Force);
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
        float distance = 0.2f;

        hit1 = Physics2D.Raycast(new Vector2(rayOrigins[0].position.x, rayOrigins[0].position.y), Vector2.down, distance, groundLayer);
        hit2 = Physics2D.Raycast(new Vector2(rayOrigins[1].position.x, rayOrigins[1].position.y), Vector2.down, distance, groundLayer);
        
        Debug.DrawRay(new Vector2(rayOrigins[0].position.x, rayOrigins[0].position.y), Vector2.down, Color.green);
        Debug.DrawRay(new Vector2(rayOrigins[1].position.x, rayOrigins[1].position.y), Vector2.down, Color.green);
        
        if (hit1.collider != null || hit2.collider != null)
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
            Instantiate(GroundEffect, rayOrigins[2].transform.position, Quaternion.identity);
        }
    }
}