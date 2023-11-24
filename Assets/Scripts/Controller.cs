using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    // Input system
    private Gameplay controls = null;
    private Vector2 movement = Vector2.zero;


    private Animator anim;
    private bool IsGrounded;
    private Rigidbody2D rb = null;
    public float PlayerSpeed = 0;
    public float JumpPower = 0;

    private bool LastDeg; // For players' rotation
    public static bool Control; // Variable for cutscenes => Turn off/on movement ability


    void Awake() {
        // Input init
        controls = new Gameplay();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        Control = LastDeg = true;
    }

    private void FixedUpdate()
    {
        if (Control)
        {
            MovePlayer();
            if (!IsGrounded) anim.Play("Jump");
        }
        else anim.Play("Idle");
    }

    // Movement inputs init
    private void OnEnable() {
        controls.Enable();
        controls.Player.Move.performed += OnMovePerformed;
        controls.Player.Move.canceled += OnMoveCanceled;
        controls.Player.Jump.performed += PlayerJump;
    }

    private void OnDisable() {
        controls.Disable();
        controls.Player.Move.performed -= OnMovePerformed;
        controls.Player.Move.canceled -= OnMoveCanceled;
        controls.Player.Jump.performed -= PlayerJump;
    }

    private void OnMovePerformed(InputAction.CallbackContext value) => movement = value.ReadValue<Vector2>();
    private void OnMoveCanceled(InputAction.CallbackContext value) => movement = Vector2.zero;
    
    
    
    void PlayerJump(InputAction.CallbackContext value) {
        if (IsGrounded)  rb.AddForce(transform.up * JumpPower, ForceMode2D.Force);
    }

    private void MovePlayer()
    {
        if (movement.x > 0 && LastDeg) Rotation();
        if (movement.x < 0 && !LastDeg) Rotation();
        if (movement.x != 0) {
            rb.velocity = new Vector2(movement.x * PlayerSpeed * Time.fixedDeltaTime * 10, rb.velocity.y);
            if (IsGrounded) anim.Play("Run");
        }
        else anim.Play("Idle");
    }

    void OnCollisionStay2D(Collision2D other) {
        if (other.gameObject.layer == 3) IsGrounded = true;
    }

    void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.layer == 3) IsGrounded = false;
    }

    private void Rotation()
    {
        LastDeg = !LastDeg;
        transform.Rotate(Vector2.up * 180);
    }
}