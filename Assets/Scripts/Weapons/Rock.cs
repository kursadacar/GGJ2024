using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Rock : MonoBehaviour
{
    [SerializeField]
    private float _damage;
    [SerializeField]
    private float _initialVelocity;

    private Rigidbody2D _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
    }

    public void Launch(Vector3 initialVelocity)
    {
        _rigidBody.velocity = initialVelocity.normalized * _initialVelocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleCollisionWith(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleCollisionWith(other.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollisionWith(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollisionWith(collision.gameObject);
    }

    private void HandleCollisionWith(GameObject other)
    {
        var collidedEnemy = other.GetComponent<EnemyCharacter>();
        if (collidedEnemy != null)
        {
            collidedEnemy.TakeDamage(_damage);
        }

        Destroy(this.gameObject);
    }
}
