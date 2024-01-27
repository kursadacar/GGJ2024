using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    private Camera _camera;

    [SerializeField]
    private Transform _mainCharacter;

    [SerializeField]
    private float _cameraDistance;

    [SerializeField]
    private float _borderPercentage;

    private Vector3? _cameraTargetPosition;

    void Start()
    {
        _camera = GetComponent<Camera>();

        SetCameraPosition(GetTargetCameraPosition());
    }

    void Update()
    {
        float dt = Time.deltaTime;

        var screenPoint = _camera.WorldToViewportPoint(_mainCharacter.transform.position);
        if (IsCloseToScreenBorder(screenPoint, _borderPercentage))
        {
            _cameraTargetPosition = GetTargetCameraPosition();
        }

        if (_cameraTargetPosition != null)
        {
            if (Vector3.Distance(_camera.transform.position, _cameraTargetPosition.Value) < float.Epsilon)
            {
                _cameraTargetPosition = null;
            }
            else
            {
                TickCameraMovement(_cameraTargetPosition.Value, dt);
            }
        }
    }

    private bool IsCloseToScreenBorder(Vector2 screenPoint, float borderPercentage)
    {
        var camRect = _camera.pixelRect;
        var borderRatio = borderPercentage * 0.01f;
        var screenPixel = new Vector2(screenPoint.x * camRect.width, screenPoint.y * camRect.height);

        var leftBorder = camRect.min.x + (camRect.width * borderRatio);
        var rightBorder = camRect.max.x - (camRect.width * borderRatio);
        var topBorder = camRect.min.y + (camRect.height * borderRatio);
        var bottomBorder = camRect.max.y - (camRect.height * borderRatio);

        if (screenPixel.x < leftBorder || screenPixel.x > rightBorder || screenPixel.y < topBorder || screenPixel.y > bottomBorder)
        {
            return true;
        }

        return false;
    }

    private Vector3 GetTargetCameraPosition()
    {
        return _mainCharacter.transform.position + (Vector3.forward * _cameraDistance * -1f);
    }

    private void TickCameraMovement(Vector3 targetPosition, float dt)
    {
        var direction = targetPosition - _camera.transform.position;
        var followSpeed = _mainCharacter.GetComponent<MainCharacterController>().GetMovementSpeed();
        var followDistance = Mathf.Min(followSpeed * dt, direction.magnitude);

        SetCameraPosition(_camera.transform.position + (direction.normalized * followDistance));
    }

    private void SetCameraPosition(Vector3 targetPosition)
    {
        _camera.transform.position = targetPosition;
    }
}
