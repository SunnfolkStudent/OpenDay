using System;
using UnityEngine;

public class ChaseEnemy : MonoBehaviour
{
    public float speed;
    public float sightRange;
    public float chaseRange;
    public bool canChase;
    
    public Transform target;
    
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private Vector2 _moveDirection;
    
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
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
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.chartreuse;  
        Gizmos.DrawWireSphere(transform.position, sightRange);   
        Gizmos.color = Color.aquamarine;  
        Gizmos.DrawWireSphere(transform.position, chaseRange);    
    }
}
