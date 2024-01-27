using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    private Camera _camera;
    private float _defaultCameraSize;

    [SerializeField]
    private float _cameraHeightOffset;

    [SerializeField]
    private float _borderPercentage;

    [SerializeField]
    private float _groundDistanceMultiplier;

    [SerializeField]
    private Vector2 _cameraSizeLimit;

    void Start()
    {
        _camera = GetComponent<Camera>();

        Vector3 targetPosition = _camera.transform.position;
        CalculateCameraTargetPosition(1f, ref targetPosition);
        _camera.transform.position = targetPosition;

        _defaultCameraSize = _camera.orthographicSize;
    }

    void Update()
    {
        float dt = Time.deltaTime;

        Vector3 targetPosition = _camera.transform.position;

        CalculateCameraTargetPosition(dt, ref targetPosition);
        _camera.transform.position = targetPosition;
        //TickCameraMovement(targetPosition, dt);
    }

    private void CalculateCameraTargetPosition(float dt, ref Vector3 targetPosition)
    {
        targetPosition = _camera.transform.position;

        CalculateHorizontalTargetPosition(dt, ref targetPosition);
        CalculateVerticalTargetPosition(dt, ref targetPosition);
        CalculateTargetSize(dt, ref targetPosition);
    }

    private void CalculateHorizontalTargetPosition(float dt, ref Vector3 targetPosition)
    {
        var screenPoint = _camera.WorldToViewportPoint(MainCharacterController.Instance.transform.position);
        if (IsCloseToScreenBorder(screenPoint, _borderPercentage))
        {
            var distance = MainCharacterController.Instance.transform.position.x - _camera.transform.position.x;
            float distanceSign = distance < 0 ? -1f : 1f;
            distance = Mathf.Abs(distance);

            var followSpeed = MainCharacterController.Instance.GetMovementSpeed();
            followSpeed = Mathf.Abs(followSpeed);

            var followDistance = Mathf.Min(followSpeed * dt, distance);

            targetPosition.x = _camera.transform.position.x + followDistance * distanceSign;
        }
    }

    private void CalculateVerticalTargetPosition(float dt, ref Vector3 targetPosition)
    {
        var groundDistance = MainCharacterController.Instance.GetGroundDistance();
        targetPosition.y = MainCharacterController.Instance.transform.position.y + _cameraHeightOffset - (groundDistance * 0.5f);
    }

    private void CalculateTargetSize(float dt, ref Vector3 targetPosition)
    {
        var groundDistance = MainCharacterController.Instance.GetGroundDistance();
        var groundSizeOffset = groundDistance * _groundDistanceMultiplier;

        _camera.orthographicSize = Mathf.Clamp(_defaultCameraSize + groundSizeOffset, _cameraSizeLimit.x, _cameraSizeLimit.y);
    }

    private bool IsCloseToScreenBorder(Vector2 screenPoint, float borderPercentage)
    {
        var camRect = _camera.pixelRect;
        var borderRatio = borderPercentage * 0.01f;
        var screenPixel = new Vector2(screenPoint.x * camRect.width, screenPoint.y * camRect.height);

        var leftBorder = camRect.min.x + (camRect.width * borderRatio);
        var rightBorder = camRect.max.x - (camRect.width * borderRatio);

        if (screenPixel.x < leftBorder
            || screenPixel.x > rightBorder)
        {
            return true;
        }

        return false;
    }
}
