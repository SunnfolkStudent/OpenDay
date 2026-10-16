using UnityEngine;

public class DashEnemy : MonoBehaviour
{
    public float moveSpeed;
    public float sightRange;

    public LayerMask whatIsGround;
    
    private bool _canAttack;
    private Vector3 _attackDirection;
    private Transform _target;
    private Animator _animator;
    private bool _isAttacking;

    private void Start()
    {
        _target = GameObject.Find("Player").transform;
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Physics2D.OverlapCircle(transform.position, 1.2f, whatIsGround))
        {
            if(!_canAttack && Vector2.Distance(_target.position, transform.position) < sightRange)
            {
                _attackDirection = Vector3.Normalize(_target.position - transform.position);
                _canAttack = true;
                _isAttacking = true;
            }
            else if (Vector2.Distance(_target.position, transform.position) > sightRange && _isAttacking)
            {
                Destroy(gameObject, 0.5f);
            }
        }
        
        
        

        if (_canAttack)
        {
            transform.position += _attackDirection * (moveSpeed * Time.deltaTime);
            _animator.Play("dash");
        }
        else
        {
            _animator.Play("idle");
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.chartreuse;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
