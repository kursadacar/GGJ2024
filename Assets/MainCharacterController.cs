using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterController : MonoBehaviour
{
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

    //[SerializeField]
    //private float _verticalVelocity;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float verticalVelocity = transform.position.y - _lastPosition.y;
        //_verticalVelocity = verticalVelocity;//TODO: remove
        HandleInput(verticalVelocity);

        _lastPosition = transform.position;
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

        if (IsKeyPressed(_jumpKeys) && verticalVelocity < 0.01f && verticalVelocity > -0.01f)
        {
            //Debug.Log("jump");
            _rigidBody.AddForce(Vector2.up * _jumpForce);
        }
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
