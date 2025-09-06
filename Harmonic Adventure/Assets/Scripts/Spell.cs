using System;
using System.Collections.Generic;

[Serializable]
public struct Spell
{
    public SpellType spellType;
    // We are no longer using a list of frets.
    public FretPattern RequiredFrets;
    public StrumPattern RequiredStrums;
}


[Serializable]
public struct Fret
{
    public int stringRow;
    public int fretNumber;

    public Fret(int stringRow, int fretNumber)
    {
        this.stringRow = stringRow;
        this.fretNumber = fretNumber;
    }
}

[Serializable]
public struct Strum
{
    public GuitarInteractionManager.StrumButtonStatesEnum strumDirection;
}

[Serializable]
public struct StrumPattern
{
    public GuitarInteractionManager.StrumButtonStatesEnum strum_1;
    public GuitarInteractionManager.StrumButtonStatesEnum strum_2;
    public GuitarInteractionManager.StrumButtonStatesEnum strum_3;
    public GuitarInteractionManager.StrumButtonStatesEnum strum_4;
    public GuitarInteractionManager.StrumButtonStatesEnum strum_5;
    public GuitarInteractionManager.StrumButtonStatesEnum strum_6;
    public GuitarInteractionManager.StrumButtonStatesEnum strum_7;
    public GuitarInteractionManager.StrumButtonStatesEnum strum_8;
}

[Serializable]
public struct FretPattern
{
    public int string_1_Fret;
    public int string_2_Fret;
    public int string_3_Fret;
    public int string_4_Fret;
    public int string_5_Fret;
    public int string_6_Fret;
}

public enum SpellType
{
    // Default value for a new spell
    None,

    // Your spells go here.
    // Example:
    C_Major_Chord,
    G_Major_Chord,
    A_Minor_Chord
}