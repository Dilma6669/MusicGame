using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(AudioSource))]
public class ChordRecognizer : MonoBehaviour
{
	[Header("Microphone Configuration")]
	[SerializeField] private string microphoneDevice = null; // Null for default microphone
	[SerializeField] private int sampleRate = 44100; // Audio sample rate (Hz)
	[SerializeField] private int fftSampleSize = 1024; // FFT sample size (power of 2)

	[Header("Chord Detection Parameters")]
	[SerializeField] private float amplitudeThreshold = 0.1f; // Minimum amplitude for frequency detection
	[SerializeField] private float frequencyTolerance = 5f; // Hz tolerance for frequency matching

	private AudioSource audioSource;
	private float[] spectrumData;
	private Dictionary<string, float[]> chordDictionary; // Maps chord names to frequency arrays
	private bool isRecording = false;

	private void Start()
	{
		// Initialize AudioSource and spectrum data
		audioSource = GetComponent<AudioSource>();
		spectrumData = new float[fftSampleSize];

		// Set up chord dictionary
		InitializeChordDictionary();

		// Begin microphone capture
		StartMicrophone();
	}

	private void Update()
	{
		if (!isRecording) return;

		// Retrieve FFT spectrum data
		audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);

		// Identify chord from spectrum
		string detectedChord = DetectChord();
		if (!string.IsNullOrEmpty(detectedChord))
		{
			Debug.Log($"Detected Chord: {detectedChord}");
		}
	}

	// Initialize the expanded chord dictionary with frequencies
	private void InitializeChordDictionary()
	{
		chordDictionary = new Dictionary<string, float[]>
		{
            // Major Triads (Root, Major 3rd, Perfect 5th)
            { "C Major", new float[] { 261.63f, 329.63f, 392.00f } },  // C4, E4, G4
            { "C3 Major", new float[] { 130.81f, 164.81f, 196.00f } }, // C3, E3, G3
            { "C5 Major", new float[] { 523.25f, 659.25f, 783.99f } }, // C5, E5, G5
            { "G Major", new float[] { 392.00f, 493.88f, 587.33f } },  // G4, B4, D5
            { "D Major", new float[] { 293.66f, 369.99f, 440.00f } },  // D4, F#4, A4
            { "A Major", new float[] { 440.00f, 554.37f, 659.25f } },  // A4, C#5, E5
            { "F Major", new float[] { 349.23f, 440.00f, 523.25f } },  // F4, A4, C5
            { "Bb Major", new float[] { 466.16f, 587.33f, 698.46f } }, // Bb4, D5, F5
            { "E Major", new float[] { 329.63f, 415.30f, 493.88f } },  // E4, G#4, B4

            // Minor Triads (Root, Minor 3rd, Perfect 5th)
            { "A Minor", new float[] { 440.00f, 523.25f, 659.25f } },  // A4, C5, E5
            { "E Minor", new float[] { 329.63f, 392.00f, 493.88f } },  // E4, G4, B4
            { "D Minor", new float[] { 293.66f, 349.23f, 440.00f } },  // D4, F4, A4
            { "F# Minor", new float[] { 369.99f, 440.00f, 554.37f } }, // F#4, A4, C#5
            { "C Minor", new float[] { 261.63f, 311.13f, 392.00f } },  // C4, Eb4, G4
            { "B Minor", new float[] { 493.88f, 587.33f, 739.99f } },  // B4, D5, F#5

            // Major Seventh Chords (Root, Major 3rd, Perfect 5th, Major 7th)
            { "C Major 7", new float[] { 261.63f, 329.63f, 392.00f, 493.88f } }, // C4, E4, G4, B4
            { "G Major 7", new float[] { 392.00f, 493.88f, 587.33f, 739.99f } }, // G4, B4, D5, F#5
            { "F Major 7", new float[] { 349.23f, 440.00f, 523.25f, 659.25f } }, // F4, A4, C5, E5
            { "A Major 7", new float[] { 440.00f, 554.37f, 659.25f, 830.61f } }, // A4, C#5, E5, G#5

            // Dominant Seventh Chords (Root, Major 3rd, Perfect 5th, Minor 7th)
            { "G7", new float[] { 392.00f, 493.88f, 587.33f, 698.46f } }, // G4, B4, D5, F5
            { "C7", new float[] { 261.63f, 329.63f, 392.00f, 466.16f } }, // C4, E4, G4, Bb4
            { "D7", new float[] { 293.66f, 369.99f, 440.00f, 523.25f } }, // D4, F#4, A4, C5
            { "A7", new float[] { 440.00f, 554.37f, 659.25f, 783.99f } }, // A4, C#5, E5, G5

            // Minor Seventh_spaces_and_newlines_within_values
            // Minor Seventh Chords (Root, Minor 3rd, Perfect 5th, Minor 7th)
            { "A Minor 7", new float[] { 440.00f, 523.25f, 659.25f, 783.99f } }, // A4, C5, E5, G5
            { "D Minor 7", new float[] { 293.66f, 349.23f, 440.00f, 523.25f } }, // D4, F4, A4, C5
            { "E Minor 7", new float[] { 329.63f, 392.00f, 493.88f, 587.33f } }  // E4, G4, B4, D5
        };
	}

	// Start microphone capture
	private void StartMicrophone()
	{
		if (Microphone.devices.Length == 0)
		{
			Debug.LogError("No microphone detected!");
			return;
		}

		// Select microphone device
		string device = string.IsNullOrEmpty(microphoneDevice) ? Microphone.devices[0] : microphoneDevice;
		audioSource.clip = Microphone.Start(device, true, 10, sampleRate);
		audioSource.loop = true;
		audioSource.Play();
		isRecording = true;
		Debug.Log($"Microphone started: {device}");
	}

	// Detect chord by matching spectrum frequencies to dictionary
	private string DetectChord()
	{
		// Extract dominant frequencies
		List<float> detectedFrequencies = GetDominantFrequencies();

		if (detectedFrequencies.Count < 3) return null; // Require at least 3 notes for a chord

		// Compare with chord dictionary
		foreach (var chord in chordDictionary)
		{
			string chordName = chord.Key;
			float[] chordFrequencies = chord.Value;

			// Check if all chord frequencies are present within tolerance
			bool isMatch = chordFrequencies.All(chordFreq =>
				detectedFrequencies.Any(detectedFreq =>
					Mathf.Abs(detectedFreq - chordFreq) <= frequencyTolerance));

			if (isMatch)
			{
				return chordName;
			}
		}

		return null; // No chord matched
	}

	// Extract dominant frequencies from spectrum data
	private List<float> GetDominantFrequencies()
	{
		List<float> dominantFrequencies = new List<float>();
		float frequencyResolution = (float)sampleRate / fftSampleSize;

		// Analyze spectrum for significant frequencies
		for (int i = 0; i < fftSampleSize / 2; i++)
		{
			if (spectrumData[i] > amplitudeThreshold)
			{
				float frequency = i * frequencyResolution;
				if (frequency >= 70f && frequency <= 1000f) // Chord frequency range
				{
					dominantFrequencies.Add(frequency);
				}
			}
		}

		// Sort by amplitude and limit to top frequencies
		return dominantFrequencies.OrderByDescending(i => spectrumData[(int)(i / frequencyResolution)])
								 .Take(5)
								 .ToList();
	}

	private void OnDestroy()
	{
		// Stop microphone on object destruction
		if (isRecording)
		{
			Microphone.End(microphoneDevice);
			isRecording = false;
		}
	}

	// Helper function to calculate frequency from MIDI note number
	private float GetFrequencyFromMidiNote(int midiNote)
	{
		return 440f * Mathf.Pow(2f, (midiNote - 69f) / 12f);
	}
}