using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera virtualCamera;
    private Cinemachine.CinemachineBasicMultiChannelPerlin bmcPerlin;
    
    public IEnumerator Shake(float duration, float amplitudeGain)
    {

        float elapsed = 0f;

        bmcPerlin = GetComponentInChildren<Cinemachine.CinemachineBasicMultiChannelPerlin>();

        while (elapsed < duration)
        {
            bmcPerlin.m_AmplitudeGain = amplitudeGain;
            elapsed += Time.deltaTime;

            yield return null;

        }
        GetComponentInChildren<Cinemachine.CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;
    }
}
