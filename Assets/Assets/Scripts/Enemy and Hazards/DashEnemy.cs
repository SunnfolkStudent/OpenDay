using UnityEngine;

public class DashEnemy : MonoBehaviour
{
    public float moveSpeed;
    public float sightRange;

    public LayerMask whatIsGround;
    
    private bool _canAttack;
    private Vector3 _attackDirection;
    private Transform _target;

    private void Start()
    {
        _target = GameObject.Find("PatrolEnemy").transform;
    }
    private void Update()
    {
        if (!Physics2D.OverlapCircle(transform.position, 0.2f, whatIsGround))
        {
            if(!_canAttack && Vector2.Distance(_target.position, transform.position) < sightRange)
            {
                _attackDirection = Vector3.Normalize(_target.position - transform.position);
                _canAttack = true;
            }
        }
        else
        {
            Destroy(gameObject, 0.5f);
            _canAttack = false;
        }

        if (_canAttack)
        {
            transform.position += _attackDirection * (moveSpeed * Time.deltaTime);
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.chartreuse;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
