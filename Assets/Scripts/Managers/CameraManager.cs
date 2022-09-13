using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
public class CameraManager
{
    [SerializeField] private CinemachineVirtualCamera handCamera;
    [SerializeField] private CinemachineVirtualCamera boardCamera;


    // Cameras
    public void SetBoardCamera()
    {
        boardCamera.Priority = 1;
        handCamera.Priority = 0;
    }

    public void SetHandCamera()
    {
        boardCamera.Priority = 0;
        handCamera.Priority = 1;
    }


}
