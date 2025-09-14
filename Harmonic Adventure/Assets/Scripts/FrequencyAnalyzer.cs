using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FrequencyAnalyzer : MonoBehaviour
{
    private AudioSource audioSource;
    public int sampleSize = 1024;
    private float[] spectrum;

    // We'll need a list of musical note frequencies. This is a basic C Major scale.
    // In a final version, this would be a much larger dictionary or lookup table.
    private static readonly float[] noteFrequencies = { 261.63f, 293.66f, 329.63f, 349.23f, 392.00f, 440.00f, 493.88f, 523.25f };
    private static readonly string[] noteNames = { "C", "D", "E", "F", "G", "A", "B", "C" };

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spectrum = new float[sampleSize];
    }

    void Update()
    {
        // Don't analyze if the audio source isn't playing
        if (!audioSource.isPlaying)
        {
            return;
        }

        // Get the audio spectrum data from the AudioSource
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        // Find the most dominant frequency
        float maxVolume = 0;
        int maxIndex = 0;
        for (int i = 0; i < spectrum.Length; i++)
        {
            if (spectrum[i] > maxVolume)
            {
                maxVolume = spectrum[i];
                maxIndex = i;
            }
        }

        // Calculate the corresponding frequency in Hertz
        float dominantFrequency = maxIndex * (AudioSettings.outputSampleRate / 2f) / sampleSize;

        // Check if the dominant frequency is above a certain volume threshold
        if (maxVolume > 0.005f)
        {
            string detectedNote = FindClosestNote(dominantFrequency);
            Debug.Log($"Detected note: {detectedNote} (Frequency: {dominantFrequency} Hz)");
        }
    }

    // A simple function to find the closest note to a given frequency
    string FindClosestNote(float frequency)
    {
        float minDifference = float.MaxValue;
        string closestNote = "Unknown";

        for (int i = 0; i < noteFrequencies.Length; i++)
        {
            float difference = Mathf.Abs(noteFrequencies[i] - frequency);
            if (difference < minDifference)
            {
                minDifference = difference;
                closestNote = noteNames[i];
            }
        }
        return closestNote;
    }
}