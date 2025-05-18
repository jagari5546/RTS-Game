using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private float keyboardPanSpeed = 5f;
    [SerializeField] private float zoomSpeed = 1f;
    [SerializeField] private float minZoomDistance = 7.5f;

    private CinemachineFollow cinemachineFollow;
    private float zoomStartTime;
    private Vector3 startingFollowOffset;

    private void Awake()
    {
        if (!cinemachineCamera.TryGetComponent(out cinemachineFollow))
        {
            Debug.LogError("CinemachineFollow component not found on the camera.");
        }

        startingFollowOffset = cinemachineFollow.FollowOffset;
    }

    private void Update()
    {
        HandlePanning();
        HandleZooming();
    }

    private void HandleZooming()
    {
        if (ShouldSetZoomStartTime())
        {
            zoomStartTime = Time.time;
        }

        float zoomTime = Mathf.Clamp01((Time.time - zoomStartTime)*zoomSpeed);
        Vector3 targetFollowOffset;

        if (Keyboard.current.nKey.isPressed)
        {
            targetFollowOffset = new Vector3(
                cinemachineFollow.FollowOffset.x,
                minZoomDistance,
                cinemachineFollow.FollowOffset.z
            );
        }
        else
        {
            targetFollowOffset = new Vector3(
                cinemachineFollow.FollowOffset.x,
                startingFollowOffset.y,
                cinemachineFollow.FollowOffset.z
            );
        }
        
        cinemachineFollow.FollowOffset = Vector3.Slerp(
            cinemachineFollow.FollowOffset,
            targetFollowOffset,
            zoomTime
        );
    }

    private bool ShouldSetZoomStartTime()
    {
        return Keyboard.current.nKey.wasPressedThisFrame
               || Keyboard.current.nKey.wasReleasedThisFrame;
    }

    private void HandlePanning()
    {
        var moveAmount = Vector2.zero;
        if (Keyboard.current.upArrowKey.isPressed)
        {
            moveAmount.y += keyboardPanSpeed;
        }
        if (Keyboard.current.downArrowKey.isPressed)
        {
            moveAmount.y -= keyboardPanSpeed;
        }
        if (Keyboard.current.rightArrowKey.isPressed)
        {
            moveAmount.x += keyboardPanSpeed;
        }
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            moveAmount.x -= keyboardPanSpeed;
        }
        
        moveAmount *= Time.deltaTime;
        cameraTarget.position += new Vector3(moveAmount.x, 0, moveAmount.y);
    }
}
