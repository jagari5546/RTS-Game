using UnityEngine;


[System.Serializable]
public class CameraConfig
{
    [field: SerializeField] public float KeyboardPanSpeed = 5f;
    
    [Header("Camera Rotation")] 
    [field: SerializeField] public float RotationSpeed { get; private set; } = 90f;

    [Header("Camera Zoom")]
    [field: SerializeField] public float MouseZoomStep { get; private set; } = 1f;
    [field: SerializeField] public float MaxZoomDistance { get; private set; } = 20f;
    [field: SerializeField] public float MinZoomDistance { get; private set; } = 7.5f;
    [field: SerializeField] public float ZoomSpeed { get; private set; } =  20f;
    
    [Header("Mouse Panning")]
    [field: SerializeField] public bool EnableEdgePan { get; private set; } = true;
    [field: SerializeField] public float MousePanSpeed { get; private set; } =  5f;
    [field: SerializeField] public float EdgePanSize { get; private set; } =  50f;

}
