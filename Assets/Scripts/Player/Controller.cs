using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Muratich;

namespace Muratich {
    public class Controller : MonoBehaviour
    {
        // Input system
        private Gameplay controls = null;
        private Vector2 movement = Vector2.zero;


        private Animator anim;
        private bool IsGrounded;
        private Rigidbody2D rb = null;
        public float PlayerSpeed = 0;
        private float PlayerSpeedConst = 0;
        public float JumpPower = 0;
        private int groundLayer = 3;
        public Transform[] RayOrigins;
        private Gun gun;
        private MenuDrop menuDrop;

        private bool LastDeg; // For players' rotation
        public static bool Control; // Variable for cutscenes => Turn off/on movement ability


        void Awake() {
            // Input init
            controls = new Gameplay();
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            Control = LastDeg = true;
            groundLayer = LayerMask.GetMask("Ground");
            PlayerSpeedConst = PlayerSpeed;
            gun = gameObject.GetComponent<Gun>();
            menuDrop = GameObject.FindGameObjectWithTag("MenuDrop").GetComponent<MenuDrop>();
        }

        private void FixedUpdate()
        {
            if (Control)
            {
                GroundCheck();
                MovePlayer();
                if (!IsGrounded) anim.Play("Jump");
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
        }

        private void OnDisable() {
            controls.Disable();
            controls.Player.Move.performed -= OnMovePerformed;
            controls.Player.Move.canceled -= OnMoveCanceled;
            controls.Player.Jump.performed -= PlayerJump;
            controls.Player.Exit.performed -= menuDrop.OpenPanel;
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
            else {
                if (IsGrounded) anim.Play("Idle");
            }
        }

        private void GroundCheck()
        {
            RaycastHit2D hit1;
            RaycastHit2D hit2;
            float distance = 0.15f;

            hit1 = Physics2D.Raycast(new Vector2(RayOrigins[0].position.x, RayOrigins[0].position.y), Vector2.down, distance, groundLayer);
            hit2 = Physics2D.Raycast(new Vector2(RayOrigins[1].position.x, RayOrigins[1].position.y), Vector2.down, distance, groundLayer);
            Debug.DrawRay(new Vector2(RayOrigins[0].position.x, RayOrigins[0].position.y), Vector2.down, Color.green);
            Debug.DrawRay(new Vector2(RayOrigins[1].position.x, RayOrigins[1].position.y), Vector2.down, Color.green);
            if (hit1.collider != null || hit2.collider != null)
            {
                IsGrounded = true;
                rb.sharedMaterial.friction = 5;
                PlayerSpeed = PlayerSpeedConst;
            }
            else 
            {
                IsGrounded = false;
                rb.sharedMaterial.friction = 0;
                PlayerSpeed = PlayerSpeedConst / 1.7f;
            }
        }

        private void Rotation()
        {
            LastDeg = !LastDeg;
            transform.Rotate(Vector2.up * 180);
        }
    }
}