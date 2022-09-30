using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    //CinemachineVirtualCamera cinemachine;
    public static CameraShake instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        SetUpCamera();
    }

    void SetUpCamera()
    {
        if (GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>())
        {
            GetComponent<CinemachineVirtualCameraBase>().Follow = GameObject.FindGameObjectWithTag("Player").transform;
            GetComponent<CinemachineVirtualCameraBase>().LookAt = GameObject.FindGameObjectWithTag("Player").transform;
        }
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
