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
        
        // --- DEFINE C Major CHORD (x32010) ---
        FretPattern cMajorPattern = new FretPattern
        {
            string_1_Fret = -1, // Muted
            string_2_Fret = 3,
            string_3_Fret = 2,
            string_4_Fret = 0,
            string_5_Fret = 1,
            string_6_Fret = 0,
        };

        // --- DEFINE G Major CHORD (320003) ---
        FretPattern gMajorPattern = new FretPattern
        {
            string_1_Fret = 3,
            string_2_Fret = 2,
            string_3_Fret = 0,
            string_4_Fret = 0,
            string_5_Fret = 0,
            string_6_Fret = 3,
        };

        // --- DEFINE A Minor CHORD (x02210) ---
        FretPattern aMinorPattern = new FretPattern
        {
            string_1_Fret = -1, // Muted
            string_2_Fret = 0,
            string_3_Fret = 2,
            string_4_Fret = 2,
            string_5_Fret = 1,
            string_6_Fret = 0,
        };

        // --- DEFINE F Major CHORD (133211) ---
        FretPattern fMajorPattern = new FretPattern
        {
            string_1_Fret = 1,
            string_2_Fret = 3,
            string_3_Fret = 3,
            string_4_Fret = 2,
            string_5_Fret = 1,
            string_6_Fret = 1,
        };
        
        // Define the StrumPattern for all test spells
        StrumPattern simpleDownStrum = new StrumPattern
        {
            strum_1 = GuitarInteractionManager.StrumButtonStatesEnum.Down,
            strum_2 = GuitarInteractionManager.StrumButtonStatesEnum.Down,
            strum_3 = GuitarInteractionManager.StrumButtonStatesEnum.Down,
            strum_4 = GuitarInteractionManager.StrumButtonStatesEnum.Down,
            strum_5 = GuitarInteractionManager.StrumButtonStatesEnum.Down,
            strum_6 = GuitarInteractionManager.StrumButtonStatesEnum.Down,
            strum_7 = GuitarInteractionManager.StrumButtonStatesEnum.Down,
            strum_8 = GuitarInteractionManager.StrumButtonStatesEnum.Down
        };


        // -------------------------------------------------------------
        // Define a Chord Sequence Spell: C Major -> G Major
        // -------------------------------------------------------------
        Spell testSpell1 = new Spell();
        testSpell1.spellType = SpellType.TestSpell1;
        
        testSpell1.RequiredFrets = new FretPattern[4];
        testSpell1.RequiredFrets[0] = cMajorPattern;
        testSpell1.RequiredFrets[1] = gMajorPattern;
        testSpell1.RequiredFrets[2] = cMajorPattern;
        testSpell1.RequiredFrets[3] = gMajorPattern;
        
        testSpell1.RequiredStrums = simpleDownStrum;
        
        spellManager.AllSpells.Add(testSpell1);

        // -------------------------------------------------------------
        // Define a Chord Sequence Spell: A Minor -> F Major
        // -------------------------------------------------------------
        Spell testSpell2 = new Spell();
        testSpell2.spellType = SpellType.TestSpell2;
        
        testSpell2.RequiredFrets = new FretPattern[4];
        testSpell2.RequiredFrets[0] = aMinorPattern;
        testSpell2.RequiredFrets[1] = fMajorPattern;
        testSpell2.RequiredFrets[2] = aMinorPattern;
        testSpell2.RequiredFrets[3] = fMajorPattern;
        
        testSpell2.RequiredStrums = simpleDownStrum;
        
        spellManager.AllSpells.Add(testSpell2);

        Debug.Log("Test chords loaded into SpellManager.");
    }
}