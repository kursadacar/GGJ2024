using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterController : MonoBehaviour
{
    #region DEBUG FIELDS
    [SerializeField]
    private bool _isGrounded;
    [SerializeField]
    private float _raycastHitDistance;

    #endregion

    [SerializeField]
    private float _movementSpeed;

    [SerializeField]
    private float _jumpForce;

    [SerializeField]
    private KeyCode[] _moveLeftKeys;
    [SerializeField]
    private KeyCode[] _moveRightKeys;
    [SerializeField]
    private KeyCode[] _jumpKeys;

    private Vector3 _lastPosition;

    private Rigidbody2D _rigidBody;
    private CapsuleCollider2D _capsuleCollider;

    //[SerializeField]
    //private float _verticalVelocity;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        float verticalVelocity = transform.position.y - _lastPosition.y;
        //_verticalVelocity = verticalVelocity;//TODO: remove
        HandleInput(verticalVelocity);

        _lastPosition = transform.position;
    }

    public float GetMovementSpeed()
    {
        return _movementSpeed;
    }

    private void HandleInput(float verticalVelocity)
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

        _isGrounded = IsCharacterGrounded();
        if (IsKeyPressed(_jumpKeys) && _isGrounded)
        {
            //Debug.Log("jump");
            _rigidBody.AddForce(Vector2.up * _jumpForce);
        }
    }

    private bool IsCharacterGrounded()
    {
        var rayOrigin = transform.position + (Vector3.up * _capsuleCollider.size.y * 0.5f * -1f) + Vector3.up * 0.1f;

        var characterLayerMask = LayerMask.GetMask("Character");

        var contactFiler = new ContactFilter2D()
        {
            minNormalAngle = 0f,
            maxNormalAngle = 150f,
            useNormalAngle = true,
            layerMask = characterLayerMask,
            useLayerMask = true,
        };

        var raycastResult = Physics2D.Raycast(rayOrigin, Vector3.up * -1f, 0.11f, ~characterLayerMask);
        if (raycastResult)
        {
            _raycastHitDistance = Vector2.Distance(raycastResult.point, transform.position);
            return _raycastHitDistance < (_capsuleCollider.size.y + 0.01f);
        }

        return false;
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
