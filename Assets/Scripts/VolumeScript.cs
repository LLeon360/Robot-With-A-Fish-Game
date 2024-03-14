using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio; // Required for AudioMixer
using UnityEngine.UI; // Required for Slider

public class VolumeScript : MonoBehaviour
{
    public AudioMixer audioMixer; // Reference to the AudioMixer
    public Slider slider; // Reference to the Slider

    void Start() 
    {
        //grab slider
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        float minVolume = -80; // Minimum volume in dB
        float maxVolume = 0; // Maximum volume in dB

        // Map the slider value from [0, 1] to [minVolume, maxVolume]
        float volume = slider.value * (maxVolume - minVolume) + minVolume;

        // Set the volume of the AudioMixer
        audioMixer.SetFloat("Volume", volume);

        //mute at min
        if(slider.value <= 0.55) 
        {
            audioMixer.SetFloat("Volume", -80);
        }
    }
}