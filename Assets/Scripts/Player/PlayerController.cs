using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   public float moveSpeed;
   public float jumpSpeed;

   public bool isGrounded;
   public Transform groundCheck;
   public LayerMask whatIsGround;
   public Vector2 groundBoxSize = new Vector2(0.8f, 0.2f);

   private InputManager _input;
   private Rigidbody2D _rb;

   private void Start()
   {
      _input = GetComponent<InputManager>();
      _rb = GetComponent<Rigidbody2D>();
   }

   private void FixedUpdate()
   {
      _rb.linearVelocityX = _input.horizontal * moveSpeed;
   }

   private void Update()
   {
      isGrounded = Physics2D.OverlapBox(groundCheck.position, groundBoxSize,0f, whatIsGround);
      if (isGrounded && _input.jump)
      {
         _rb.gravityScale = 0;
         _rb.linearVelocityY = jumpSpeed;
      }
   }

   private void OnDrawGizmos()
   {
      Gizmos.color = Color.yellow;
      Gizmos.DrawWireCube(groundCheck.position, groundBoxSize);
   }
}
