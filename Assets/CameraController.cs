using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private float _cameraHeightOffset;

    [SerializeField]
    private float _borderPercentage;

    void Start()
    {
        _camera = GetComponent<Camera>();

        Vector3 targetPosition = _camera.transform.position;
        CalculateCameraTargetPosition(1f, ref targetPosition);
        _camera.transform.position = targetPosition;
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
        CalculateDistance(dt, ref targetPosition);
    }

    private void CalculateHorizontalTargetPosition(float dt, ref Vector3 targetPosition)
    {
        var screenPoint = _camera.WorldToViewportPoint(_mainCharacter.transform.position);
        if (IsCloseToScreenBorder(screenPoint, _borderPercentage))
        {
            var distance = _mainCharacter.transform.position.x - _camera.transform.position.x;
            float distanceSign = distance < 0 ? -1f : 1f;
            distance = Mathf.Abs(distance);

            var followSpeed = _mainCharacter.GetComponent<MainCharacterController>().GetMovementSpeed();
            followSpeed = Mathf.Abs(followSpeed);

            var followDistance = Mathf.Min(followSpeed * dt, distance);

            targetPosition.x = _camera.transform.position.x + followDistance * distanceSign;
        }
    }

    private void CalculateVerticalTargetPosition(float dt, ref Vector3 targetPosition)
    {
        targetPosition.y = _mainCharacter.transform.position.y + _cameraHeightOffset;
    }

    private void CalculateDistance(float dt, ref Vector3 targetPosition)
    {
        targetPosition.z = -_cameraDistance;
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

    private void TickCameraMovement(Vector3 targetPosition, float dt)
    {
        var direction = targetPosition - _camera.transform.position;
        var followSpeed = _mainCharacter.GetComponent<MainCharacterController>().GetMovementSpeed();
        var followDistance = Mathf.Min(followSpeed * dt, direction.magnitude);

        _camera.transform.position += direction.normalized * followDistance;
    }
}
