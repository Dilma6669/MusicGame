using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class GuitarInteractionManager : MonoBehaviour
{
    public ChordSaver ChordSaverManager;
    
    public List<GameObject> StringRows;
    public GameObject StrumRow;

    public Dictionary<KeyValuePair<int, int>, FretButton> AllFretButtons = new();
    public static List<StrumButton> AllStrumButtons = new();
    
    public static List<FretButton> CurrSelectedFrets = new();
    
    public enum FretButtonStatesEnum
    {
        NotSelected,
        Selected,
    }
    
    public enum StrumButtonStatesEnum
    {
        Down,
        Up,
        Skip
    }

    private static readonly string[] ChromaticScaleSharps = new string[]
    {
        "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B"
    };


    private static readonly string[] ChromaticScaleFlats = new string[]
    {
        "C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab", "A", "Bb", "B"
    };

    private static readonly string[] OpenStrings = new string[]
    {
        "E", "A", "D", "G", "B", "E"
    };

    
    private void Awake()
    {
        void SetUpButtonsPerRow(GameObject rowObject, int rowCount)
        {
            int fretCount = 0;

            // Get the starting note for this string based on the user's pitch order.
            // StringRows[0] is high E, which is OpenStrings[5]
            // StringRows[1] is B, which is OpenStrings[4]
            // etc.
            string openNote = OpenStrings[OpenStrings.Length - 1 - rowCount];

            // Find the index of the open note in our chromatic scale array.
            int openNoteIndex = System.Array.IndexOf(ChromaticScaleSharps, openNote);

            foreach (var child in rowObject.GetComponentsInChildren<FretButton>())
            {
                var count = fretCount;
                child.StringRow = rowCount;
                child.FretNumber = count;
                
                AllFretButtons.Add(new KeyValuePair<int, int>(rowCount, count), child);
                
                fretCount++;
            }
        }

        // Setup the strings rows and frets
        for (int i = 0; i < StringRows.Count; i++)
        {
            SetUpButtonsPerRow(StringRows[i], i);
        }

        // Set up the strum row
        int strumCount = 0;
        foreach (var child in StrumRow.GetComponentsInChildren<StrumButton>())
        {
            child.StrumNumber = strumCount;
            AllStrumButtons.Add(child);
            strumCount++;
        }
    }
    

    private void Start()
    {
        void SetUpButtonsPerRow(GameObject rowObject, int rowCount)
        {
            int fretCount = 0;

            // Get the starting note for this string based on the user's pitch order.
            // StringRows[0] is high E, which is OpenStrings[5]
            // StringRows[1] is B, which is OpenStrings[4]
            // etc.
            string openNote = OpenStrings[OpenStrings.Length - 1 - rowCount];

            // Find the index of the open note in our chromatic scale array.
            int openNoteIndex = System.Array.IndexOf(ChromaticScaleSharps, openNote);

            foreach (var child in rowObject.GetComponentsInChildren<FretButton>())
            {
                // This is the core calculation.
                // 1. Add the fret number to the open string's index.
                // 2. Use the modulo operator to get the correct note index for this fret,
                //    wrapping around the 12-note scale.
                int noteIndex = (openNoteIndex + fretCount) % ChromaticScaleSharps.Length;
                
                // Set the button's text to the note from our array.
                child.GetTextComponent.text = ChromaticScaleSharps[noteIndex];
                
                fretCount++;
            }
        }

        // Setup the strings rows and frets
        for (int i = 0; i < StringRows.Count; i++)
        {
            SetUpButtonsPerRow(StringRows[i], i);
        }
        
        ChordSaverManager.Initialize();
    }
    
    public static void AddSelectedFret(FretButton fretButton)
    {
        CurrSelectedFrets.Add(fretButton);
    }


// In your GuitarInteractionManager.cs script

    public static void FretButtonPressed(FretButton fretButton)
    {
        Debug.Log(
            $"[Guitar Interaction Manager] FretButtonPressed string row = {fretButton.StringRow}, fret number = {fretButton.FretNumber} ");

        bool alreadySelected = CurrSelectedFrets.Contains(fretButton);
        
            // TODO: HERE try to make the mute button go from mute to not mute colour even tho its still technically 
            // selected.
        
            // First, clear any existing frets on the same string
            var fretsOnString = CurrSelectedFrets.Where(f => f.StringRow == fretButton.StringRow).ToList();
            foreach (var fret in fretsOnString)
            {
                CurrSelectedFrets.Remove(fret);
                fret.IsMuted = false; // Ensure it's not muted
                fret.ActivateButtonState(false);
            }

            // Now select the new fret and add it to the list
            CurrSelectedFrets.Add(fretButton);
            fretButton.IsMuted = false; // The new selection is not muted by default
            fretButton.ActivateButtonState(true);
            
            if (alreadySelected && fretButton.FretNumber == 0)
            {
                bool isMuted = !fretButton.IsMuted;
                fretButton.IsMuted = isMuted;
            
                fretButton.ActivateButtonState(isMuted);
            }
    }

    public static void StrumButtonPressed(StrumButton strumButton)
    {
        Debug.Log($"Guitar Interaction Manager StrumButtonPressed strum number = {strumButton.StrumNumber} strum pattern = {strumButton.CurrStrumButtonStateEnum} ");
    }

    public static void ClearAllSelectedFrets()
    {
        foreach (var fretButton in CurrSelectedFrets)
        {
            fretButton.ActivateButtonState(false);   
        }
        CurrSelectedFrets.Clear();
    }
}