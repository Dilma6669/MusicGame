using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    [SerializeField] public List<Spell> AllSpells;
    
    public ChordSaver ChordSaver;

    public TextMeshProUGUI ResultPanel;

    public void CastSpellButtonPressed()
    {
        SpellType spellCast = CheckForSpell();

        ResultPanel.text = spellCast.ToString();
        
        GuitarInteractionManager.ClearAllSelectedFrets();
        ChordSaver.ResetRecordedChords(); // Reset the recorded chords for the next spell attempt
    }
    
    public SpellType CheckForSpell()
    {
        // Get the full sequence of chords the player has recorded
        FretPattern[] playerChords = ChordSaver.GetRecordedChords();

        // Check if the player has recorded the correct number of chords (4)
        if (playerChords.Length != 4)
        {
            Debug.Log("Please record 4 chords before attempting a spell.");
            ResultPanel.text = "Fail! (Need 4 chords)";
            return SpellType.None;
        }

        // We will no longer print the player's single fret pattern.
        // Instead, we will check the full recorded sequence.

        // Now, we'll check if the player's input sequence matches any of our defined spells.
        foreach (var spell in AllSpells)
        {
            // First, check if the required number of chords matches the player's input
            if (spell.RequiredFrets.Length != playerChords.Length)
            {
                continue; // Skip spells that don't have a 4-chord sequence
            }
            
            bool fretsMatch = true;
            // Loop through each of the 4 chords in the sequence
            for (int i = 0; i < 4; i++)
            {
                // Compare the player's chord at this position with the required chord for the spell
                FretPattern requiredChord = spell.RequiredFrets[i];
                FretPattern playerChord = playerChords[i];

                if (!CheckSingleFret(requiredChord.string_1_Fret, playerChord.string_1_Fret)) fretsMatch = false;
                if (!CheckSingleFret(requiredChord.string_2_Fret, playerChord.string_2_Fret)) fretsMatch = false;
                if (!CheckSingleFret(requiredChord.string_3_Fret, playerChord.string_3_Fret)) fretsMatch = false;
                if (!CheckSingleFret(requiredChord.string_4_Fret, playerChord.string_4_Fret)) fretsMatch = false;
                if (!CheckSingleFret(requiredChord.string_5_Fret, playerChord.string_5_Fret)) fretsMatch = false;
                if (!CheckSingleFret(requiredChord.string_6_Fret, playerChord.string_6_Fret)) fretsMatch = false;
                
                // If any chord in the sequence doesn't match, we can stop checking this spell
                if (!fretsMatch)
                {
                    break;
                }
            }

            bool strumsMatch = true;
            // The strum logic remains the same, as it's a single pattern
            StrumPattern currentStrumPattern = new StrumPattern();
            List<StrumButton> strumButtons = GuitarInteractionManager.AllStrumButtons;
            
            if (strumButtons.Count > 0)
            {
                currentStrumPattern.strum_1 = strumButtons[0].CurrStrumButtonStateEnum;
                currentStrumPattern.strum_2 = strumButtons[1].CurrStrumButtonStateEnum;
                currentStrumPattern.strum_3 = strumButtons[2].CurrStrumButtonStateEnum;
                currentStrumPattern.strum_4 = strumButtons[3].CurrStrumButtonStateEnum;
                currentStrumPattern.strum_5 = strumButtons[4].CurrStrumButtonStateEnum;
                currentStrumPattern.strum_6 = strumButtons[5].CurrStrumButtonStateEnum;
                currentStrumPattern.strum_7 = strumButtons[6].CurrStrumButtonStateEnum;
                currentStrumPattern.strum_8 = strumButtons[7].CurrStrumButtonStateEnum;
            }

            if (spell.RequiredStrums.strum_1 == currentStrumPattern.strum_1 &&
                spell.RequiredStrums.strum_2 == currentStrumPattern.strum_2 &&
                spell.RequiredStrums.strum_3 == currentStrumPattern.strum_3 &&
                spell.RequiredStrums.strum_4 == currentStrumPattern.strum_4 &&
                spell.RequiredStrums.strum_5 == currentStrumPattern.strum_5 &&
                spell.RequiredStrums.strum_6 == currentStrumPattern.strum_6 &&
                spell.RequiredStrums.strum_7 == currentStrumPattern.strum_7 &&
                spell.RequiredStrums.strum_8 == currentStrumPattern.strum_8)
            {
            }
            else
            {
                strumsMatch = false;
            }

            if (fretsMatch && strumsMatch)
            {
                Debug.Log($"Spell '{spell.spellType}' successfully cast!");
                ResultPanel.text = "Success!";
                return spell.spellType;
            }
        }
        
        Debug.Log("No spell found. Please try again.");
        ResultPanel.text = "Fail!";
        return SpellType.None;
    }
    
    // Helper method to check a single fret for a match.
    private bool CheckSingleFret(int requiredFret, int playerFret)
    {
        if (requiredFret == -1)
        {
            return playerFret == -1;
        }
        else
        {
            return requiredFret == playerFret;
        }
    }
}