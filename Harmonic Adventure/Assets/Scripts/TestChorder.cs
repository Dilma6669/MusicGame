using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpellManager))]
public class ChordTester : MonoBehaviour
{
    private SpellManager spellManager;

    private void Start()
    {
        spellManager = GetComponent<SpellManager>();
        
        // Clear any existing spells to avoid duplicates during testing
        if (spellManager.AllSpells == null)
        {
            spellManager.AllSpells = new List<Spell>();
        }
        else
        {
            spellManager.AllSpells.Clear();
        }
        
        // -------------------------------------------------------------
        // Define a C Major Chord (x32010)
        // x = muted
        // 3 = 3rd fret
        // 2 = 2nd fret
        // 0 = open string
        // 1 = 1st fret
        // 0 = open string
        // -------------------------------------------------------------
        Spell cMajor = new Spell();
        cMajor.spellType = SpellType.C_Major_Chord;
        
        // Define the frets for the chord
        cMajor.RequiredFrets = new FretPattern
        {
            string_1_Fret = -1, // Muted String
            string_2_Fret = 3,  // 3rd Fret
            string_3_Fret = 2,  // 2nd Fret
            string_4_Fret = 0,  // Open String
            string_5_Fret = 1,  // 1st Fret
            string_6_Fret = 0,  // Open String
        };
        
        // Define a simple down strum pattern
        cMajor.RequiredStrums = new StrumPattern
        {
            strum_1 = GuitarInteractionManager.StrumButtonStatesEnum.Down,
            strum_2 = GuitarInteractionManager.StrumButtonStatesEnum.Down,
            strum_3 = GuitarInteractionManager.StrumButtonStatesEnum.Down,
            strum_4 = GuitarInteractionManager.StrumButtonStatesEnum.Down,
            strum_5 = GuitarInteractionManager.StrumButtonStatesEnum.Down,
            strum_6 = GuitarInteractionManager.StrumButtonStatesEnum.Down,
            strum_7 = GuitarInteractionManager.StrumButtonStatesEnum.Skip,
            strum_8 = GuitarInteractionManager.StrumButtonStatesEnum.Skip
        };
        
        // Add the spell to the list
        spellManager.AllSpells.Add(cMajor);

        // -------------------------------------------------------------
        // Define a G Major Chord (320003)
        // -------------------------------------------------------------
        Spell gMajor = new Spell();
        gMajor.spellType = SpellType.G_Major_Chord;
        
        gMajor.RequiredFrets = new FretPattern
        {
            string_1_Fret = 3,
            string_2_Fret = 2,
            string_3_Fret = 0,
            string_4_Fret = 0,
            string_5_Fret = 0,
            string_6_Fret = 3,
        };
        
        gMajor.RequiredStrums = new StrumPattern
        {
            strum_1 = GuitarInteractionManager.StrumButtonStatesEnum.Down,
            strum_2 = GuitarInteractionManager.StrumButtonStatesEnum.Down,
            strum_3 = GuitarInteractionManager.StrumButtonStatesEnum.Down,
            strum_4 = GuitarInteractionManager.StrumButtonStatesEnum.Down,
            strum_5 = GuitarInteractionManager.StrumButtonStatesEnum.Down,
            strum_6 = GuitarInteractionManager.StrumButtonStatesEnum.Down,
            strum_7 = GuitarInteractionManager.StrumButtonStatesEnum.Skip,
            strum_8 = GuitarInteractionManager.StrumButtonStatesEnum.Skip
        };
        
        spellManager.AllSpells.Add(gMajor);

        // -------------------------------------------------------------
        // Define an A Minor Chord (x02210)
        // -------------------------------------------------------------
        Spell aMinor = new Spell();
        aMinor.spellType = SpellType.A_Minor_Chord;

        aMinor.RequiredFrets = new FretPattern
        {
            string_1_Fret = -1, // Muted String
            string_2_Fret = 0,
            string_3_Fret = 2,
            string_4_Fret = 2,
            string_5_Fret = 1,
            string_6_Fret = 0,
        };
        
        aMinor.RequiredStrums = new StrumPattern
        {
            strum_1 = GuitarInteractionManager.StrumButtonStatesEnum.Down,
            strum_2 = GuitarInteractionManager.StrumButtonStatesEnum.Down,
            strum_3 = GuitarInteractionManager.StrumButtonStatesEnum.Down,
            strum_4 = GuitarInteractionManager.StrumButtonStatesEnum.Down,
            strum_5 = GuitarInteractionManager.StrumButtonStatesEnum.Down,
            strum_6 = GuitarInteractionManager.StrumButtonStatesEnum.Down,
            strum_7 = GuitarInteractionManager.StrumButtonStatesEnum.Skip,
            strum_8 = GuitarInteractionManager.StrumButtonStatesEnum.Skip
        };
        
        spellManager.AllSpells.Add(aMinor);

        Debug.Log("Test chords loaded into SpellManager.");
    }
}