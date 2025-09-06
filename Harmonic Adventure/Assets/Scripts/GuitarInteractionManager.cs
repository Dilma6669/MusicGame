using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class GuitarInteractionManager : MonoBehaviour
{
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
                var count = fretCount;
                child.StringRow = rowCount;
                child.FretNumber = count;
                
                AllFretButtons.Add(new KeyValuePair<int, int>(rowCount, count), child);

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

        // Set up the strum row
        int strumCount = 0;
        foreach (var child in StrumRow.GetComponentsInChildren<StrumButton>())
        {
            child.StrumNumber = strumCount;
            AllStrumButtons.Add(child);
            strumCount++;
        }
        
    }

    public static void FretButtonPressed(FretButton fretButton)
    {
        Debug.Log($"Fuck Guitar Interaction Manager string row = {fretButton.StringRow}, fret number = {fretButton.FretNumber} ");

        foreach (var child in CurrSelectedFrets)
        {
            if (child.StringRow == fretButton.StringRow)
            {
                child.ActivateButtonState(false);
                CurrSelectedFrets.Remove(child);
                break;
            }
        }

        CurrSelectedFrets.Add(fretButton);
        
    }
    
    public static void StrumButtonPressed(StrumButton strumButton)
    {
        Debug.Log($"Fuck Guitar Interaction Manager strum number = {strumButton.StrumNumber} strum pattern = {strumButton.CurrStrumButtonStateEnum} ");
    }

    public static void ClearAllSelectedFrets()
    {
        foreach (var child in CurrSelectedFrets)
        {
            child.ActivateButtonState(false);   
        }
    }
}