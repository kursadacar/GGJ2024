using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(MainCharacterArmController))]
public class MainCharacterController : MonoBehaviour
{
    private static MainCharacterController _instance;
    public static MainCharacterController Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<MainCharacterController>();
            }

            return _instance;
        }
    }

    #region DEBUG FIELDS
    [SerializeField]
    private bool _isGrounded;
    [SerializeField]
    private float _groundDistance;
    [SerializeField]
    private float _verticalVelocity;

    #endregion

    [SerializeField]
    private float _movementSpeed;

    [SerializeField]
    private float _maxJumpHeight;

    [SerializeField]
    private KeyCode[] _moveLeftKeys;
    [SerializeField]
    private KeyCode[] _moveRightKeys;
    [SerializeField]
    private KeyCode[] _jumpKeys;

    private Vector3 _lastPosition;

    private Rigidbody2D _rigidBody;
    private CapsuleCollider2D _capsuleCollider;
    private Animator _animator;
    private MainCharacterArmController _armController;
    private CharacterMouthController _mouthController;
    private Character _character;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _animator = GetComponent<Animator>();
        _armController = GetComponent<MainCharacterArmController>();
        _mouthController = GetComponent<CharacterMouthController>();
        _character = GetComponentInParent<Character>() ?? GetComponent<Character>();
    }

    void Start()
    {
        _character.MouthController.StartExpression(CharacterMouthController.ExpressionType.TalkUpset);
    }

    void Update()
    {
        _groundDistance = CalculateGroundDistance();

        HandleInput();

        _verticalVelocity = (transform.position.y - _lastPosition.y) * (1f / Time.deltaTime);
        _animator.SetVerticalVelocity(Mathf.Abs(_verticalVelocity));

        _lastPosition = transform.position;
    }

    private void LateUpdate()
    {
        if (!_isGrounded)
        {
            _animator.SetHorizontalVelocity(0f);
        }
    }

    public float GetMovementSpeed()
    {
        return _movementSpeed;
    }

    public float GetGroundDistance()
    {
        return _groundDistance;
    }

    public bool GetIsGrounded()
    {
        return _groundDistance < 0.01f;
    }

    private void HandleInput()
    {
        float movementAmount = 0f;

        if (IsKeyDown(_moveLeftKeys))
        {
            movementAmount += _movementSpeed * -1f;
        }

        if (IsKeyDown(_moveRightKeys))
        {
            movementAmount += _movementSpeed;
        }

        var position = transform.position;
        position.x += movementAmount * Time.deltaTime;

        transform.position = position;

        _isGrounded = GetIsGrounded();
        if (IsKeyPressed(_jumpKeys) && _isGrounded)
        {
            //Debug.Log("jump");
            //vMax = v(y^2) / 2g

            float initialSpeed = Mathf.Sqrt(_maxJumpHeight * Physics.gravity.magnitude * 2f);

            var velocity = _rigidBody.velocity;
            velocity.y = +initialSpeed;
            _rigidBody.velocity = velocity;
        }
    }

    private float CalculateGroundDistance()
    {
        if(_capsuleCollider == null)
        {
            return 0f;
        }

        var rayOrigin = transform.position + (Vector3.up * _capsuleCollider.size.y * 0.5f * -1f) + Vector3.up * 0.1f;
        var characterLayerMask = LayerMask.GetMask("Character");

        //var contactFilter = new ContactFilter2D()
        //{
        //    minNormalAngle = 0f,
        //    maxNormalAngle = 150f,
        //    useNormalAngle = true,
        //    layerMask = characterLayerMask,
        //    useLayerMask = true,
        //};

        var raycastResult = Physics2D.Raycast(rayOrigin, Vector3.up * -1f, 10000f, ~characterLayerMask);
        if (raycastResult)
        {
            _groundDistance = Vector2.Distance(raycastResult.point, transform.position) - (_capsuleCollider.size.y / 2f);
            return _groundDistance;
        }

        return 0f;
    }

    private bool IsKeyPressed(KeyCode[] codes)
    {
        for(int i = 0; i< codes.Length; i++)
        {
            if (Input.GetKeyDown(codes[i]))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsKeyDown(KeyCode[] codes)
    {
        for(int i = 0; i< codes.Length; i++)
        {
            if (Input.GetKey(codes[i]))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsKeyReleased(KeyCode[] codes)
    {
        for(int i = 0; i< codes.Length; i++)
        {
            if (Input.GetKeyUp(codes[i]))
            {
                return true;
            }
        }

        return false;
    }
}
