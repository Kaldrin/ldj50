using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    //CinemachineVirtualCamera cinemachine;

    // Start is called before the first frame update
    void Awake()
    {
        //cinemachine = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float intensity, float time) 
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMulti = GameObject.FindObjectOfType<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMulti.m_AmplitudeGain = intensity;
        Invoke("StopShake", time);
    }

    void StopShake()
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMulti = GameObject.FindObjectOfType<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMulti.m_AmplitudeGain = 0f;
    }
}
