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
            string_1_Fret = 0, // Muted
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
            string_1_Fret = 0, // Muted
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
        
        // --- DEFINE E Major CHORD (022100) ---
        FretPattern eMajorPattern = new FretPattern
        {
            string_1_Fret = 0,
            string_2_Fret = 2,
            string_3_Fret = 2,
            string_4_Fret = 1,
            string_5_Fret = 0,
            string_6_Fret = 0,
        };
        
        // --- DEFINE D Minor CHORD (x-x-0231) ---
        FretPattern dMinorPattern = new FretPattern
        {
            string_1_Fret = -1,
            string_2_Fret = -1,
            string_3_Fret = 0,
            string_4_Fret = 2,
            string_5_Fret = 3,
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
        
        // --- DEFINE ALTERNATING STRUM (DUDUDUDU) ---
        StrumPattern alternatingStrum = new StrumPattern
        {
            strum_1 = GuitarInteractionManager.StrumButtonStatesEnum.Down,
            strum_2 = GuitarInteractionManager.StrumButtonStatesEnum.Up,
            strum_3 = GuitarInteractionManager.StrumButtonStatesEnum.Down,
            strum_4 = GuitarInteractionManager.StrumButtonStatesEnum.Up,
            strum_5 = GuitarInteractionManager.StrumButtonStatesEnum.Down,
            strum_6 = GuitarInteractionManager.StrumButtonStatesEnum.Up,
            strum_7 = GuitarInteractionManager.StrumButtonStatesEnum.Down,
            strum_8 = GuitarInteractionManager.StrumButtonStatesEnum.Up
        };
        
        // --- DEFINE CHARM STRUM (D-DU-DU-) ---
        StrumPattern charmStrum = new StrumPattern
        {
            strum_1 = GuitarInteractionManager.StrumButtonStatesEnum.Down,
            strum_2 = GuitarInteractionManager.StrumButtonStatesEnum.Skip,
            strum_3 = GuitarInteractionManager.StrumButtonStatesEnum.Down,
            strum_4 = GuitarInteractionManager.StrumButtonStatesEnum.Up,
            strum_5 = GuitarInteractionManager.StrumButtonStatesEnum.Skip,
            strum_6 = GuitarInteractionManager.StrumButtonStatesEnum.Down,
            strum_7 = GuitarInteractionManager.StrumButtonStatesEnum.Up,
            strum_8 = GuitarInteractionManager.StrumButtonStatesEnum.Skip
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

        // -------------------------------------------------------------
        // Define an Intimidate Spell: E Major -> D Minor
        // -------------------------------------------------------------
        Spell intimidateSpell = new Spell();
        intimidateSpell.spellType = SpellType.Intimidate;
        
        intimidateSpell.RequiredFrets = new FretPattern[4];
        intimidateSpell.RequiredFrets[0] = eMajorPattern;
        intimidateSpell.RequiredFrets[1] = dMinorPattern;
        intimidateSpell.RequiredFrets[2] = eMajorPattern;
        intimidateSpell.RequiredFrets[3] = dMinorPattern;
        
        intimidateSpell.RequiredStrums = alternatingStrum;
        
        spellManager.AllSpells.Add(intimidateSpell);
        
        // -------------------------------------------------------------
        // Define a Charm Spell: C Major -> A Minor
        // -------------------------------------------------------------
        Spell charmSpell = new Spell();
        charmSpell.spellType = SpellType.Charm;
        
        charmSpell.RequiredFrets = new FretPattern[4];
        charmSpell.RequiredFrets[0] = cMajorPattern;
        charmSpell.RequiredFrets[1] = aMinorPattern;
        charmSpell.RequiredFrets[2] = cMajorPattern;
        charmSpell.RequiredFrets[3] = aMinorPattern;
        
        charmSpell.RequiredStrums = charmStrum;
        
        spellManager.AllSpells.Add(charmSpell);

        Debug.Log("Test chords loaded into SpellManager.");
    }
}