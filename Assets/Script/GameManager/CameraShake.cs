using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera cvc;
    
    private CinemachineBasicMultiChannelPerlin cbmp;

    private Coroutine shakeCamera;

    void Start()
    {
        cvc = GetComponent<CinemachineVirtualCamera>();
        cbmp = cvc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cbmp.m_AmplitudeGain = 0.0f;
    }
    

    public void ShakeCamera(float ShakeIntesity, float time)
    {
        cbmp.m_AmplitudeGain = ShakeIntesity;

        if (shakeCamera != null)
        {
            StopCoroutine(shakeCamera);
        }
        shakeCamera = StartCoroutine(stopShake(time));
    }

    IEnumerator stopShake(float time)
    {
        yield return new WaitForSeconds(time);
        
        cbmp.m_AmplitudeGain = 0.0f;
    }
}
