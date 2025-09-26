using UnityEngine;
using NAudio.Wave;
using Aurio;
using System;
using System.Linq;

public class AudioAnalyzer : MonoBehaviour
{
    // --- Audio Input ---
    private WaveInEvent waveIn;
    private int sampleRate = 44100;
    private int bufferSize = 2048; // Must be power of 2 for FFT

    // --- Audio Analysis ---
    private PitchDetector pitchDetector;
    private float[] currentBuffer;
    private float[] chromagram;
    private float[] fftResult;

    private void Start()
    {
        // 1. Initialize our audio buffer
        currentBuffer = new float[bufferSize];

        // 2. Initialize the PitchDetector from the Aurio library
        pitchDetector = new PitchDetector(sampleRate, bufferSize);
        fftResult = new float[bufferSize];

        // 3. Set up NAudio to listen to the microphone
        waveIn = new WaveInEvent();
        waveIn.DeviceNumber = 0; // Use default microphone
        waveIn.WaveFormat = new WaveFormat(sampleRate, 1);
        waveIn.BufferMilliseconds = 50;

        // 4. Subscribe to the event that provides new audio data
        waveIn.DataAvailable += OnDataAvailable;

        // 5. Start listening!
        waveIn.StartRecording();
        Debug.Log("Microphone recording started.");
    }

    private void OnDataAvailable(object sender, WaveInEventArgs e)
    {
        // Copy the incoming audio bytes into our float buffer
        for (int i = 0; i < e.BytesRecorded / 2; i++)
        {
            currentBuffer[i] = BitConverter.ToInt16(e.Buffer, i * 2) / 32768f;
        }

        // --- Perform the analysis with Aurio ---
        pitchDetector.DetectPitch(currentBuffer, fftResult);
        
        // This is the chromagram, the most important part!
        chromagram = pitchDetector.GetChromagram();
        
        // At this point, you have the chromagram data!
        // We will do the chord matching here in the next step.
    }

    private void OnDestroy()
    {
        // Clean up when the application quits
        if (waveIn != null)
        {
            waveIn.StopRecording();
            waveIn.Dispose();
            waveIn = null;
        }
    }
}