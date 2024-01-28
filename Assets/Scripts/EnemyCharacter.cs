using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{
    private bool _isAttacking;

    [SerializeField] private float _damage;
    [SerializeField] private float _attackDuration;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _damageRange;

    [SerializeField] private float _movementSpeed;

    void Start()
    {
        
    }

    protected override void Update()
    {
        base.Update();

        if (MainCharacter.Instance != null)
        {
            var distance = Vector2.Distance(MainCharacter.Instance.transform.position, transform.position);
            if (distance < _attackRange && !_isAttacking)
            {
                Attack();
            }

            if (IsPlayerOnRightSide())
            {
                MoveRight(_movementSpeed);
            }
            else
            {
                MoveLeft(_movementSpeed);
            }
        }
    }

    private bool IsPlayerOnRightSide()
    {
        return MainCharacter.Instance.transform.position.x >= transform.position.x;
    }

    private void Attack()
    {
        StartCoroutine(OnPreAttack());
    }

    private IEnumerator OnPreAttack()
    {
        _isAttacking = true;

        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<Collider2D>().isTrigger = true;

        float timer = 0f;
        float duration = _attackDuration;

        Vector3 fromPosition = transform.position;
        Vector3 toPosition = transform.position - (transform.right * 0.2f);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(fromPosition, toPosition, Mathf.Clamp01(timer / duration));
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(OnAttack());
    }

    private IEnumerator OnAttack()
    {
        float timer = 0f;
        float duration = _attackDuration / 2f;

        Vector3 fromPosition = transform.position;
        Vector3 toPosition = transform.position + (transform.right * 0.35f);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(fromPosition, toPosition, Mathf.Clamp01(timer / duration));
            yield return new WaitForEndOfFrame();
        }

        if(MainCharacter.Instance != null)
        {
            var distance = Vector2.Distance(MainCharacter.Instance.transform.position, transform.position);
            if(distance < _damageRange)
            {
                MainCharacter.Instance.TakeDamage(_damage);
            }
        }

        timer = 0f;
        duration = _attackDuration;

        fromPosition = transform.position;
        toPosition = transform.position - (transform.right * 0.15f);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(fromPosition, toPosition, Mathf.Clamp01(timer / duration));
            yield return new WaitForEndOfFrame();
        }

        GetComponent<Rigidbody2D>().isKinematic = false;
        GetComponent<Collider2D>().isTrigger = false;
        _isAttacking = false;
    }
}
