﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class CameraTest : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        if (photonView.IsMine)
        {
            CinemachineVirtualCamera followCam = FindObjectOfType<CinemachineVirtualCamera>();
            followCam.Follow = transform;
            followCam.LookAt = transform;
        }
    }
}
