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
