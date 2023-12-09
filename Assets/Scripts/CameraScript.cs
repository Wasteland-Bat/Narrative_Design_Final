using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCamera vcam;

    [SerializeField]
    float shakeDuration;

    [SerializeField]
    float trainShakeTimer;

    [SerializeField]
    float shakeFreq;

    [SerializeField]
    float shakeIntensity;

    CinemachineBasicMultiChannelPerlin perlin;

    // Start is called before the first frame update
    void Start()
    {
        perlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        StartCoroutine(trainShake());
        perlin.m_FrequencyGain = 0;
        perlin.m_AmplitudeGain = 0;
    }

    public void CameraShake()
    {
        //Debug.Log("shake");
        StartCoroutine(cameraShakeTimer());

        perlin.m_FrequencyGain = shakeFreq;
        perlin.m_AmplitudeGain = shakeIntensity;
    }

    IEnumerator cameraShakeTimer()
    {
        yield return new WaitForSeconds(shakeDuration);

        perlin.m_AmplitudeGain = 0;
        perlin.m_FrequencyGain = 0;
    }

    IEnumerator trainShake()
    {
        while (true)
        {
            yield return new WaitForSeconds(trainShakeTimer);
            CameraShake();
        }
    }
}
