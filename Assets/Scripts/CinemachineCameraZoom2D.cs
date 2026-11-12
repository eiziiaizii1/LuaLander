using System;
using Unity.Cinemachine;
using UnityEngine;

public class CinemachineCameraZoom2D : MonoBehaviour
{
    private const float NORMAL_ORTHOGRAPHIC_SIZE = 10f;
    public static CinemachineCameraZoom2D Instance {get; private set;}
    [SerializeField] private CinemachineCamera cinemachineCamera;
    private float targetOrthographicsSize = 10f;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        float zoomSpeed = 5f;
        cinemachineCamera.Lens.OrthographicSize = 
            Mathf.Lerp(cinemachineCamera.Lens.OrthographicSize, targetOrthographicsSize, Time.deltaTime * zoomSpeed);
    }

    public void SetTargetOrthographicsSize(float targetOrthographicSize)
    {
        this.targetOrthographicsSize = targetOrthographicSize;
    }
    public void SetNormalOrthographicsSize()
    {
        SetTargetOrthographicsSize(NORMAL_ORTHOGRAPHIC_SIZE);
    }
}
