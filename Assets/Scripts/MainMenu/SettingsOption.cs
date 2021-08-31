/*
 Settings in options by Eshan Kang
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsOption : MonoBehaviour
{
    public AudioMixer audioMix;

    public void setVol (float vol)
    {
        audioMix.SetFloat("vol", vol);
    }

    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
}
