using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class ChordSaver : MonoBehaviour
{
    private const int MAX_CHORDS = 4;
    
    public FretPattern[] RecordedChords = new FretPattern[MAX_CHORDS];
    
    [SerializeField] private FretPattern[] displayChords = new FretPattern[MAX_CHORDS];
    
    private int currentActiveChordIndex = 0;
    
    public GuitarInteractionManager GuitarManager;
    
    public List<ChordButton> AllChordButtons;
    
    public void Initialize()
    {
        AddFretButtonListeners();
        SelectChordSlot(0);
    }

    private void AddFretButtonListeners()
    {
        foreach (var stringRow in GuitarManager.StringRows)
        {
            foreach (var fretButton in stringRow.GetComponentsInChildren<FretButton>())
            {
                fretButton.OnFretStateChanged.AddListener(OnFretStateChanged);
            }
        }
    }

    public void SelectChordSlot(int slotIndex)
    {
//        Debug.Log($"[ChordSaver] Attempting to select slot {slotIndex}");

        if (slotIndex < 0 || slotIndex >= MAX_CHORDS)
        {
            Debug.LogWarning("Invalid slot index. Must be between 0 and 3.");
            return;
        }

        if (AllChordButtons.Count > 0)
        {
            AllChordButtons[currentActiveChordIndex].SetActive(false);
            AllChordButtons[slotIndex].SetActive(true);
        }

        currentActiveChordIndex = slotIndex;
        
        LoadChordPattern(currentActiveChordIndex, RecordedChords[currentActiveChordIndex]);
      //  
      //  Debug.Log($"[ChordSaver] Chord slot {slotIndex} selected.");
    }
    
    public void OnFretStateChanged()
    {
        Debug.Log($"[ChordSaver] Fret button state changed. Saving to slot {currentActiveChordIndex}.");
        SaveCurrentStateToChord(currentActiveChordIndex);
    }

    private void SaveCurrentStateToChord(int slotIndex)
    {
        FretPattern currentChord = new FretPattern();
        
        foreach (var fretButton in GuitarInteractionManager.CurrSelectedFrets)
        {
            switch (fretButton.StringRow)
            {
                case 5: currentChord.string_1_Fret = fretButton.FretNumber; break;
                case 4: currentChord.string_2_Fret = fretButton.FretNumber; break;
                case 3: currentChord.string_3_Fret = fretButton.FretNumber; break;
                case 2: currentChord.string_4_Fret = fretButton.FretNumber; break;
                case 1: currentChord.string_5_Fret = fretButton.FretNumber; break;
                case 0: currentChord.string_6_Fret = fretButton.FretNumber; break;
            }
        }
        
        RecordedChords[slotIndex] = currentChord;
        displayChords[slotIndex] = currentChord;
        
      //  Debug.Log($"[ChordSaver] Saving Frets to slot {slotIndex}.");
      //  Debug.Log($"[ChordSaver] Saved frets: {currentChord.string_1_Fret},{currentChord.string_2_Fret}," +
      //            $"{currentChord.string_3_Fret},{currentChord.string_4_Fret}," +
      //            $"{currentChord.string_5_Fret},{currentChord.string_6_Fret} to slot {slotIndex}.");
    }

    private void LoadChordPattern(int slotIndex, FretPattern patternToLoad)
    {
//        Debug.Log($"[ChordSaver] Loading Frets to slot {slotIndex}.");
    //    Debug.Log(
    //        $"[ChordSaver] Loading frets: {patternToLoad.string_1_Fret}," +
   //         $"{patternToLoad.string_2_Fret},{patternToLoad.string_3_Fret}," +
    //        $"{patternToLoad.string_4_Fret},{patternToLoad.string_5_Fret},{patternToLoad.string_6_Fret}");

        GuitarInteractionManager.ClearAllSelectedFrets();

        SetFretButtonState(patternToLoad.string_1_Fret, 5);
        SetFretButtonState(patternToLoad.string_2_Fret, 4);
        SetFretButtonState(patternToLoad.string_3_Fret, 3);
        SetFretButtonState(patternToLoad.string_4_Fret, 2);
        SetFretButtonState(patternToLoad.string_5_Fret, 1);
        SetFretButtonState(patternToLoad.string_6_Fret, 0);
        
    }

    private void SetFretButtonState(int fretNumber, int stringIndex)
    {
        if (fretNumber == -1)
        {
            return;
        }

        foreach (var fretButton in GuitarManager.StringRows[stringIndex].GetComponentsInChildren<FretButton>())
        {
            if (fretButton.FretNumber == fretNumber)
            {
                fretButton.CurrFretButtonStateEnum = GuitarInteractionManager.FretButtonStatesEnum.Selected;
                
                GuitarInteractionManager.AddSelectedFret(fretButton);

                break;
            }
        }
    }

    public FretPattern[] GetRecordedChords()
    {
        return RecordedChords;
    }

    public void ResetRecordedChords()
    {
        RecordedChords = new FretPattern[MAX_CHORDS];
        displayChords = new FretPattern[MAX_CHORDS];
        
        for (int i = 0; i < MAX_CHORDS; i++)
        {
            RecordedChords[i] = new FretPattern();
        }

        currentActiveChordIndex = 0;
        GuitarInteractionManager.ClearAllSelectedFrets();
        Debug.Log("Recorded chords reset.");
        
        if (AllChordButtons.Count > 0)
        {
            foreach (var button in AllChordButtons)
            {
                button.SetActive(false);
            }
        }
        SelectChordSlot(0);
    }
}