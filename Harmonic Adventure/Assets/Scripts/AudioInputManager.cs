using System.Collections;
using UnityEngine;

public class AudioInputManager : MonoBehaviour
{
    private AudioSource audioSource;
    private string microphoneDevice;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        if (Microphone.devices.Length > 0)
        {
            microphoneDevice = Microphone.devices[0]; 
            
            audioSource.clip = Microphone.Start(microphoneDevice, true, 10, AudioSettings.outputSampleRate);
            audioSource.loop = true;
            
            Debug.Log($"Using microphone: {microphoneDevice}");
            
            StartCoroutine(WaitForMic());
        }
        else
        {
            Debug.LogError("No microphone device found!");
        }
    }
    
    IEnumerator WaitForMic()
    {
        yield return new WaitForSeconds(1); 
        audioSource.Play();
    }
}