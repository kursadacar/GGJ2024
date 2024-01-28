using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class MainCharacter : Character
{
    private static MainCharacter _instance;
    public static MainCharacter Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<MainCharacter>();
            }

            return _instance;
        }
    }

    [SerializeField]
    private MainCharacterSettings _settings;
    [SerializeField]
    private Transform _rockSpawnPoint;
    [SerializeField]
    private Transform _leftArmPivot;
    [SerializeField]
    private Transform _rightArmPivot;

    private KeyCode[] _moveLeftKeys;
    private KeyCode[] _moveRightKeys;
    private KeyCode[] _jumpKeys;
    private KeyCode[] _attackKeys;

    private bool _isAttacking;

    protected override void Awake()
    {
        base.Awake();

        _moveLeftKeys = new KeyCode[] { KeyCode.A, KeyCode.LeftArrow };
        _moveRightKeys = new KeyCode[] { KeyCode.D, KeyCode.RightAlt };
        _jumpKeys = new KeyCode[] { KeyCode.Space };
        _attackKeys = new KeyCode[] { KeyCode.Mouse0 };
    }

    protected override void Update()
    {
        base.Update();

        HandleInput();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

    }

    public float GetMovementSpeed()
    {
        return _settings.MovementSpeed;
    }

    private void HandleInput()
    {
        if (InputHelper.IsKeyDown(_moveLeftKeys))
        {
            MoveLeft(_settings.MovementSpeed);
        }

        if (InputHelper.IsKeyDown(_moveRightKeys))
        {
            MoveRight(_settings.MovementSpeed);
        }

        if (InputHelper.IsKeyPressed(_jumpKeys) && IsGrounded)
        {
            float initialSpeed = Mathf.Sqrt(_settings.MaxJumpHeight * Physics.gravity.magnitude * 2f);

            Jump(initialSpeed);
        }

        if (InputHelper.IsKeyReleased(_attackKeys))
        {
            if (!_isAttacking)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    private IEnumerator AttackCoroutine()
    {
        _isAttacking = true;

        float timer = 0f;
        float animationTime = 0.15f;

        var originalRotation = _rightArmPivot.rotation;
        var euler = originalRotation.eulerAngles;
        euler.z -= 160f;
        var targetRotation = Quaternion.Euler(euler);

        while (timer < animationTime)
        {
            timer += Time.deltaTime;
            _rightArmPivot.rotation = Quaternion.Lerp(originalRotation, targetRotation, timer / animationTime);
            yield return new WaitForEndOfFrame();
        }

        var rock = GameObject.Instantiate(_settings.RockPrefab.gameObject).GetComponent<Rock>();
        rock.transform.position = _rockSpawnPoint.transform.position;
        rock.transform.rotation = Quaternion.LookRotation(Vector3.forward, transform.right);
        rock.Launch(transform.right);

        timer = 0f;
        animationTime = 0.22f;

        while (timer < animationTime)
        {
            timer += Time.deltaTime;
            _rightArmPivot.rotation = Quaternion.Lerp(targetRotation, originalRotation, timer / animationTime);
            yield return new WaitForEndOfFrame();
        }

        _isAttacking = false;
    }
}
