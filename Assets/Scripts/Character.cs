using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected Color _damageTakenColor;
    [SerializeField] protected float _health;
    [SerializeField] protected float _deathDuration;
    [SerializeField] protected float _takeDamageDuration;

    public bool IsDead { get; private set; }
    public bool IsGrounded { get; private set; }

    public CharacterMouthController MouthController;

    private Vector3 _movementVector;
    private float _jumpSpeed;
    private float _groundDistance;

    private Collider2D _collider;
    private Rigidbody2D _rigidBody;

    protected virtual void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _rigidBody = GetComponent<Rigidbody2D>();

        MouthController = GetComponentInChildren<CharacterMouthController>();
    }

    protected virtual void Update()
    {
        _groundDistance = CalculateGroundDistance();
        IsGrounded = GetIsGrounded();
    }

    protected virtual void LateUpdate()
    {
        if (!IsDead && _movementVector.magnitude > 0)
        {
            //Debug.Log("Move");
            UpdateMovement();
        }

        if (!IsGrounded)
        {
            if (_rigidBody != null)
            {
                var velocity = _rigidBody.velocity;
                velocity.y = +_jumpSpeed;
                _rigidBody.velocity = velocity;
            }
        }

        _movementVector = Vector3.zero;
        _jumpSpeed = 0;
    }

    private void UpdateMovement()
    {
        transform.position += _movementVector;

        if (_movementVector.x < 0)
        {
            var rotation = transform.rotation.eulerAngles;
            rotation.y = 180f;
            transform.rotation = Quaternion.Euler(rotation);
        }

        if (_movementVector.x > 0)
        {
            var rotation = transform.rotation.eulerAngles;
            rotation.y = 0f;
            transform.rotation = Quaternion.Euler(rotation);
        }
    }

    protected void MoveLeft(float speed)
    {
        _movementVector -= Vector3.right * speed * Time.deltaTime;
    }

    protected void MoveRight(float speed)
    {
        _movementVector += Vector3.right * speed * Time.deltaTime;
    }

    protected void Jump(float speed)
    {
        _jumpSpeed = speed;
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        StartCoroutine(TakeDamageAnimationCoroutine());

        if (_health <= 0)
        {
            if (!IsDead)
            {
                IsDead = true;
                StartCoroutine(OnDeath());
            }
        }
    }

    public float GetGroundDistance()
    {
        return _groundDistance;
    }

    public bool GetIsGrounded()
    {
        return _groundDistance < 0.01f;
    }

    private float CalculateGroundDistance()
    {
        if (_collider == null)
        {
            return 0f;
        }

        var rayOrigin = _collider.bounds.min;

        var characterLayerMask = LayerMask.GetMask("Character");

        var raycastResult = Physics2D.Raycast(rayOrigin, Vector3.up * -1f, 10000f, ~characterLayerMask);
        if (raycastResult)
        {
            _groundDistance = Vector2.Distance(raycastResult.point, transform.position);
            return _groundDistance;
        }

        return 0f;
    }

    protected IEnumerator OnDeath()
    {
        var rigidBody = GetComponentInChildren<Rigidbody2D>();
        if(rigidBody != null)
        {
            rigidBody.isKinematic = true;
        }

        float timer = 0f;
        float duration = _deathDuration;

        Quaternion fromRotation = transform.rotation;
        var euler = fromRotation.eulerAngles;
        euler.z += 180f;
        Quaternion toRotation = Quaternion.Euler(euler);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            var ratio = Mathf.Clamp01(timer / duration);
            var newRotation = Quaternion.Lerp(fromRotation, toRotation, ratio);
            transform.rotation = newRotation;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(0.6f);

        Destroy(gameObject);
    }

    protected IEnumerator TakeDamageAnimationCoroutine()
    {
        var spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        var fromColor = Color.white;
        var toColor = _damageTakenColor;
        var timer = 0f;
        var duration = _takeDamageDuration;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            var ratio = Mathf.Clamp01(timer / duration);
            var newColor = Color.Lerp(fromColor, toColor, ratio);

            foreach(var rend in spriteRenderers)
            {
                rend.color = newColor;
            }

            yield return new WaitForEndOfFrame();
        }

        timer = 0f;
        duration = 0.17f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            var ratio = Mathf.Clamp01(timer / duration);
            var newColor = Color.Lerp(toColor, fromColor, ratio);

            foreach (var rend in spriteRenderers)
            {
                rend.color = newColor;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
