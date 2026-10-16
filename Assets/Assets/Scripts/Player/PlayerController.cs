using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
   [Header("Movement")]
   public float maxVelocityX = 6f;
   public float maxVelocityY = 16f;
   public float acceleration = 1f;
   public float groundFriction = 0.3f;
   public float airFriction = 0.003f;
   private Vector2 _currentVelocity;
   private float _moveSpeed;
   private int moveDirection;
   private bool _canMove;
   
   [Header("Jumping")] 
   public float jumpForce = 10f;
   public float coyoteTime = 0.15f;
   private float _fallMultiplier = 2.5f;

   public float jumpTimeCounter;
   public float jumpTime = 0.25f;

   private float _coyoteTimeCounter;
   private bool _isCoyoteTime;
   private bool _isJumping;
   
   [Header("Collision")]
   public bool isGrounded;
   public Transform groundCheck;
   public LayerMask whatIsGround;
   public Vector2 groundBoxSize = new Vector2(0.8f, 0.2f);
   
   [Header("Health")]
   public int playerHealth = 10;
   public float damageCooldown = 1f;
   private float _damageCooldownTimer;
   private bool _isDead;
   private float _deathTime;
   

   [Header("Audio")] 
   public AudioClip deathSound;
   public AudioClip[] jumpSounds;
   public AudioClip[] hurtSounds;
   public AudioClip[] walkSounds;
   

   [Header("Components")]
   private InputManager _input;
   private Rigidbody2D _rb;
   private AudioSource _audioSource;
   private Animator _animator;

   private void Start()
   {
      _input = GetComponent<InputManager>();
      _rb = GetComponent<Rigidbody2D>();
      _audioSource = GetComponent<AudioSource>();
      _animator = GetComponent<Animator>();
      
      _isDead = false;
      _canMove = true;
   }

   private void Update()
   {
      UpdateAnimation();
      Flip();
      Attack();
      if (_canMove)
      {
         UpdateJumping();
      }
      
      isGrounded = Physics2D.OverlapBox(groundCheck.position, groundBoxSize,0f, whatIsGround);
      
      
      if (isGrounded && _rb.linearVelocityY < 0f)
      {
         _isJumping = false;
      }
   }
   private void FixedUpdate()
   {
      if(_canMove)
      {
         UpdateMovement();
      }
   }

   private void UpdateAnimation()
   {
      if (_deathTime > Time.time) return;
      if (_isDead)
      {
         _animator.Play("Player_Death");
      }
      else if (isGrounded && _input.moveVector.x != 0f)
      {
         _animator.Play("Player_Walk");
      }
      else if (_input.moveVector.x == 0f)
      {
         _animator.Play("Player_Idle");
      }
      else if (!isGrounded && _isJumping)
      {
         _animator.Play("Player_Jump");
      }
   }

   private void Attack()
   {
      if (!Physics2D.OverlapCircle(groundCheck.position, 0.2f, LayerMask.GetMask("Enemy"))) return;
      
      var enemyColliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f, LayerMask.GetMask("Enemy"));

      foreach (var enemy in enemyColliders)
      {
         Destroy(enemy.gameObject);
      }
      _rb.linearVelocityY = jumpForce/1.3f;
   }
   
   #region Movement

   private void Flip()
   {
      if (_input.moveVector.x > 0)
      {
         moveDirection = 1;
      }

      if (_input.moveVector.x < 0)
      {
         moveDirection = -1;
      }
      
      if (_input.moveVector.x != 0 && transform.localScale.x != _input.moveVector.x) 
      { 
         transform.localScale = new Vector3(moveDirection, transform.localScale.y, transform.localScale.z);
      }
   }
   private void UpdateJumping()
   {
      VariableJumpHeight();
      if (!_isJumping && !isGrounded)
      {
         _coyoteTimeCounter += Time.deltaTime;
      }
      else
      {
         _coyoteTimeCounter = 0f;
      }

      if (_input.jumpPressed && (isGrounded || (_coyoteTimeCounter > 0.03f && _coyoteTimeCounter < coyoteTime )))
      {
         _rb.linearVelocity = Vector2.up * jumpForce;
         jumpTimeCounter = jumpTime;
         _isJumping = true;
         PlayRandomClip(jumpSounds);
      }
      /*else if (_input.jumpPressed && _jumps > 0)
      {
         _rb.linearVelocity = Vector2.up * jumpForce;
         _jumps--;
        // PlayRandomClip(jumpSounds);
         Debug.Log("jump2");
      }*/
      else if (_input.jumpReleased)
      {
         jumpTimeCounter = 0f;
      }

      if (_input.jumpValue < 0f)
      {
         _rb.linearVelocityY = Physics2D.gravity.y * (_fallMultiplier) * Time.deltaTime;
      }
   }

   private void VariableJumpHeight()
   {
      if (_input.jumpValue > 0f && _isJumping)
      {
         if (jumpTimeCounter > 0f)
         {
            _rb.linearVelocity = Vector2.up * jumpForce;
            jumpTimeCounter -= Time.deltaTime;
         }
      }
   }
   private void UpdateMovement()
   {
      //store rb velocity in _velocity
      _currentVelocity = _rb.linearVelocity;
      _currentVelocity.y = Mathf.Clamp(_currentVelocity.y, -maxVelocityY, maxVelocityY);
      
      //change velocity
      if (_input.moveVector.x != 0)
      {
         _moveSpeed = _input.moveVector.x * acceleration;
         _moveSpeed = Mathf.Clamp(_moveSpeed, -maxVelocityX, maxVelocityX);
      }
      else
      {
         _moveSpeed = Mathf.Lerp(_moveSpeed, 0f, isGrounded ? groundFriction : airFriction);
      }

      _currentVelocity.x = _moveSpeed;
      //Return current velocity into rb velocity
      _rb.linearVelocity = _currentVelocity;
   }
   #endregion
   #region Damage
   private void OnCollisionStay2D(Collision2D other)
   {
      if (other.transform.CompareTag("Enemy") || other.transform.CompareTag("Hazard"))
      {
         TakeDamage();
      }
   }

   public void RestartScene()
   {
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
   }
   public void TakeDamage()
   {
      if (Time.time > _damageCooldownTimer)
      {
         if (_isDead) return;
         playerHealth-= 1;
         _damageCooldownTimer = Time.time + damageCooldown;
         _deathTime = Time.time + 0.5f;
         _animator.Play("Player_Hurt");
         PlayRandomClip(hurtSounds);
         
      }
      if (playerHealth <= 0 && !_isDead)
      {
         _isDead = true;
         _canMove = false;
         AudioSource.PlayClipAtPoint(deathSound, transform.position);
      }
   }
   #endregion
   #region Sound
   private void PlayRandomClip(AudioClip[] clips)
   {
      int randomSounds = Random.Range(0, clips.Length);
      _audioSource.pitch = Random.Range(0.8f, 1.2f);
      _audioSource.PlayOneShot(clips[randomSounds]);
   }

   public void WalkAudio()
   {
      PlayRandomClip(walkSounds);
   }
   #endregion

   private void OnDrawGizmos()
   {
      Gizmos.color = Color.yellow;
      Gizmos.DrawWireCube(groundCheck.position, groundBoxSize);
   }
}
