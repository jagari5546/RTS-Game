using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Rigidbody cameraTarget;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private CameraConfig cameraConfig;
    
    private CinemachineFollow cinemachineFollow;
    private float zoomStartTime;
    private float rotationStartTime;
    private Vector3 startingFollowOffset;
    private float maxRotationAmount;

    private float horizontalFollowDistance;
    private float currentRotationAngle;
    private float targetZoomDistance;

    private void Awake()
    {
        if (!cinemachineCamera.TryGetComponent(out cinemachineFollow))
        {
            Debug.LogError("CinemachineFollow component not found on the camera.");
            return;
        }

        startingFollowOffset = cinemachineFollow.FollowOffset;

        maxRotationAmount = Mathf.Abs(cinemachineFollow.FollowOffset.z);

        horizontalFollowDistance = new Vector2(
            startingFollowOffset.x,
            startingFollowOffset.z
        ).magnitude;

        currentRotationAngle = Mathf.Atan2(
            startingFollowOffset.x,
            startingFollowOffset.z
        ) * Mathf.Rad2Deg;

        targetZoomDistance = startingFollowOffset.y;
    }

    private void Update()
    {
        HandlePanning(); 
        HandleZooming();
        HandleRotation();
    }

    private void HandleRotation()
    {
        if (cinemachineFollow == null) return;

        float rotationDirection = 0f;

        if (Keyboard.current.qKey.isPressed)
        {
            rotationDirection += 1f;
        }

        if (Keyboard.current.eKey.isPressed)
        {
            rotationDirection -= 1f;
        }

        currentRotationAngle -= rotationDirection * cameraConfig.RotationSpeed * Time.deltaTime;

        float angleInRadians = currentRotationAngle * Mathf.Deg2Rad;

        float x = Mathf.Sin(angleInRadians) * horizontalFollowDistance;
        float z = Mathf.Cos(angleInRadians) * horizontalFollowDistance;

        cinemachineFollow.FollowOffset = new Vector3(
            x,
            cinemachineFollow.FollowOffset.y,
            z
        );
    }
    

    private void HandleZooming()
    {
        if (cinemachineFollow == null) return;
        if (Mouse.current == null) return;

        float scrollY = Mouse.current.scroll.ReadValue().y;

        if (scrollY > 0f)
        {
            targetZoomDistance -= cameraConfig.MouseZoomStep;
        }
        else if (scrollY < 0f)
        {
            targetZoomDistance += cameraConfig.MouseZoomStep;
        }

        targetZoomDistance = Mathf.Clamp(
            targetZoomDistance,
            cameraConfig.MinZoomDistance,
            cameraConfig.MaxZoomDistance
        );

        float newZoomDistance = Mathf.Lerp(
            cinemachineFollow.FollowOffset.y,
            targetZoomDistance,
            Time.deltaTime * cameraConfig.ZoomSpeed
        );

        cinemachineFollow.FollowOffset = new Vector3(
            cinemachineFollow.FollowOffset.x,
            newZoomDistance,
            cinemachineFollow.FollowOffset.z
        );
    }
    

    private void HandlePanning()
    {
        if (cinemachineFollow == null) return;
        var moveAmount = GetKeyboardMoveAmount();

        moveAmount += GetMouseMoveAmount();
        moveAmount = moveAmount.normalized;

        Vector3 cameraForward = new Vector3(
            -cinemachineFollow.FollowOffset.x,
            0f,
            -cinemachineFollow.FollowOffset.z
        ).normalized;

        Vector3 cameraRight = new Vector3(
            cameraForward.z,
            0f,
            -cameraForward.x
        ).normalized;

        Vector3 moveDirection =
            cameraForward * moveAmount.y +
            cameraRight * moveAmount.x;
        cameraTarget.linearVelocity = moveDirection * cameraConfig.KeyboardPanSpeed ;
    }

    private Vector2 GetMouseMoveAmount()
    {
        Vector2 moveAmount = Vector2.zero;
        if (!cameraConfig.EnableEdgePan)
        {
            return moveAmount;
        }
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;

        if (mousePosition.x <= cameraConfig.EdgePanSize)
        {
            moveAmount.x -= cameraConfig.MousePanSpeed;
        }
        if (mousePosition.y >= screenHeight - cameraConfig.EdgePanSize)
        {
            moveAmount.y += cameraConfig.MousePanSpeed;
        }
        else if (mousePosition.x >= screenWidth-cameraConfig.EdgePanSize)
        {
            moveAmount.x += cameraConfig.MousePanSpeed;
        }
        else if (mousePosition.y <= cameraConfig.EdgePanSize)
        {
            moveAmount.y -= cameraConfig.MousePanSpeed;
        }
        return moveAmount;
    }

    private Vector2 GetKeyboardMoveAmount()
    {
        Vector2 moveAmount = Vector2.zero;
        if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed)
        {
            moveAmount.y += 1f;
        }

        if (Keyboard.current.downArrowKey.isPressed || Keyboard.current.sKey.isPressed)
        {
            moveAmount.y -= 1f;
        }

        if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
        {
            moveAmount.x += 1f;
        }

        if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
        {
            moveAmount.x -= 1f;
        }

        return moveAmount;
    }
}