using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake Instance { get; private set; }

    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin chinemachineMultiChannelPerlin;

    private float timer;
    private float timerMax;
    private float startingIntensity;

    private void Awake()
    {
        Instance = this;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        chinemachineMultiChannelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float intensity, float timerMax)
    {
        this.timerMax = timerMax;
        timer = 0f;
        startingIntensity = intensity;
        chinemachineMultiChannelPerlin.m_AmplitudeGain = intensity;
    }

    private void Update()
    {
        if(timer < timerMax)
        {
            timer += Time.deltaTime;
            float amplitude = Mathf.Lerp(startingIntensity, 0f, timer / timerMax);
            chinemachineMultiChannelPerlin.m_AmplitudeGain = amplitude;
        }
    }
}
