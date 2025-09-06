using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    [SerializeField] public List<Spell> AllSpells;

    public GuitarInteractionManager GuitarManager;

    public TextMeshProUGUI ResultPanel;

    public void CastSpellButtonPressed()
    {
        SpellType spellCast = CheckForSpell();

        ResultPanel.text = spellCast.ToString();
        
        GuitarInteractionManager.ClearAllSelectedFrets();
    }
    
    public SpellType CheckForSpell()
    {
        // Get the current states of the fret buttons for ALL strings
        FretPattern playerFrets = new FretPattern();
        
        // Initialize the player's fret pattern with a default of -1 (muted) for each string.
        playerFrets.string_1_Fret = -1;
        playerFrets.string_2_Fret = -1;
        playerFrets.string_3_Fret = -1;
        playerFrets.string_4_Fret = -1;
        playerFrets.string_5_Fret = -1;
        playerFrets.string_6_Fret = -1;

        // Populate the FretPattern with the player's current selection.
        // We will now iterate backwards to get the fret order correct (Low E to High E)
        for (int i = GuitarManager.StringRows.Count - 1; i >= 0; i--)
        {
            foreach (var fretButton in GuitarManager.StringRows[i].GetComponentsInChildren<FretButton>())
            {
                if (fretButton.CurrFretButtonStateEnum == GuitarInteractionManager.FretButtonStatesEnum.Selected)
                {
                    // If a fret is selected on this string, record its fret number
                    // The switch statement now maps the reverse iteration to the correct string field
                    switch(i)
                    {
                        case 5: playerFrets.string_1_Fret = fretButton.FretNumber; break;
                        case 4: playerFrets.string_2_Fret = fretButton.FretNumber; break;
                        case 3: playerFrets.string_3_Fret = fretButton.FretNumber; break;
                        case 2: playerFrets.string_4_Fret = fretButton.FretNumber; break;
                        case 1: playerFrets.string_5_Fret = fretButton.FretNumber; break;
                        case 0: playerFrets.string_6_Fret = fretButton.FretNumber; break;
                    }
                    break;
                }
            }
        }
        
        // Get the current states of the strum buttons
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

        Debug.Log("Player Input:");
        Debug.Log($"Frets: [{playerFrets.string_1_Fret}, {playerFrets.string_2_Fret}, {playerFrets.string_3_Fret}, {playerFrets.string_4_Fret}, {playerFrets.string_5_Fret}, {playerFrets.string_6_Fret}]");
        Debug.Log($"Strums: [{currentStrumPattern.strum_1}, {currentStrumPattern.strum_2}, {currentStrumPattern.strum_3}, {currentStrumPattern.strum_4}, {currentStrumPattern.strum_5}, {currentStrumPattern.strum_6}, {currentStrumPattern.strum_7}, {currentStrumPattern.strum_8}]");

        
        foreach (var spell in AllSpells)
        {
            Debug.Log($"Checking against spell: {spell.spellType}");
            Debug.Log($"Required Frets: [{spell.RequiredFrets.string_1_Fret}, {spell.RequiredFrets.string_2_Fret}, {spell.RequiredFrets.string_3_Fret}, {spell.RequiredFrets.string_4_Fret}, {spell.RequiredFrets.string_5_Fret}, {spell.RequiredFrets.string_6_Fret}]");
            Debug.Log($"Required Strums: [{spell.RequiredStrums.strum_1}, {spell.RequiredStrums.strum_2}, {spell.RequiredStrums.strum_3}, {spell.RequiredStrums.strum_4}, {spell.RequiredStrums.strum_5}, {spell.RequiredStrums.strum_6}, {spell.RequiredStrums.strum_7}, {spell.RequiredStrums.strum_8}]");
            
            bool fretsMatch = true;
            
            if (!CheckSingleFret(spell.RequiredFrets.string_1_Fret, playerFrets.string_1_Fret)) fretsMatch = false;
            if (!CheckSingleFret(spell.RequiredFrets.string_2_Fret, playerFrets.string_2_Fret)) fretsMatch = false;
            if (!CheckSingleFret(spell.RequiredFrets.string_3_Fret, playerFrets.string_3_Fret)) fretsMatch = false;
            if (!CheckSingleFret(spell.RequiredFrets.string_4_Fret, playerFrets.string_4_Fret)) fretsMatch = false;
            if (!CheckSingleFret(spell.RequiredFrets.string_5_Fret, playerFrets.string_5_Fret)) fretsMatch = false;
            if (!CheckSingleFret(spell.RequiredFrets.string_6_Fret, playerFrets.string_6_Fret)) fretsMatch = false;

            bool strumsMatch = true;
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