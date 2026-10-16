using UnityEngine;
using System.Collections;

public class ChaseEnemy : MonoBehaviour
{
    public float speed;
    public float sightRange;
    public float chaseRange;
    public bool canChase;
    
    public Transform target;
    
    public AudioClip walkSfx;
    public float sfxTimer;
    
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private Vector2 _moveDirection;
    private AudioSource _audioSource;
    
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _animator.Play("Chase");
    }

    private void FixedUpdate()          
    {                                              
        if (canChase)                          
        {                      
            _rigidbody2D.linearVelocityX = _moveDirection.x * speed;  
        }                             
        else                     
        {                                       
            _rigidbody2D.linearVelocityX = 0;    
        }

        if (canChase)
        {
            _rigidbody2D.linearVelocityY = _moveDirection.y * speed;
        }
        else
        {
            _rigidbody2D.linearVelocityY = 0;
        }
        
        if (sfxTimer <= 0)
        {
            _audioSource.pitch = Random.Range(0.8f, 1.2f);
            _audioSource.PlayOneShot(walkSfx);
            sfxTimer = 0.4f;
        }
    }
    
    private void Update()
    {                         
        if (target == null) return;
        _moveDirection = Vector3.Normalize(target.position - transform.position);
        
        if (Vector2.Distance(target.position, transform.position) < sightRange)
        {
            canChase = true;
        }
        else if (Vector2.Distance(target.position, transform.position) > chaseRange)
        {
            canChase = false;
        }
        
        if (transform.position.x > target.position.x)
        {
            transform.localScale = new Vector2(1, 1);
        }
        else
        {
            transform.localScale = new Vector2(-1, 1);
        }
        
        if (sfxTimer > 0)
        {
            sfxTimer -= Time.deltaTime;
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.chartreuse;  
        Gizmos.DrawWireSphere(transform.position, sightRange);   
        Gizmos.color = Color.aquamarine;  
        Gizmos.DrawWireSphere(transform.position, chaseRange);    
    }
}
