using UnityEngine;
using System.Collections;

public class EnemyPatrol : MonoBehaviour
{
    public float moveSpeed;
   
    public LayerMask whatIsWall;
    public Transform wallCheck;
    public Transform fallCheck;
    
    public AudioClip walkSfx;
    public float sfxTimer;
    
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private AudioSource _audioSource;
    
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _animator.Play("PatrolTest");
    }
    
    private void Update()
    {
        if (DetectedWallOrFall())
        {
            moveSpeed *= -1;
            transform.localScale  = new Vector2(transform.localScale.x * -1f, 1f);
        }
        
        if (sfxTimer > 0)
        {
            sfxTimer -= Time.deltaTime;
        }
    }
    
    private void FixedUpdate()
    {
        _rigidbody2D.linearVelocityX = moveSpeed;
        
        if (sfxTimer <= 0)
        {
            _audioSource.pitch = Random.Range(0.8f, 1.2f);
            _audioSource.PlayOneShot(walkSfx);
            sfxTimer = 1;
        }
    }
    
    private bool DetectedWallOrFall()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.1f, whatIsWall)
               || !Physics2D.OverlapCircle(fallCheck.position, 0.1f);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(wallCheck.position, 0.1f);
        Gizmos.DrawWireSphere(fallCheck.position, 0.1f);
    }
}