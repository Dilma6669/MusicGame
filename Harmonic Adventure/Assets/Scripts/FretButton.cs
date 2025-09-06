using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FretButton : MonoBehaviour
{
    public int StringRow;
    public int FretNumber;
    
    public GuitarInteractionManager.FretButtonStatesEnum currFretButtonStateEnum = GuitarInteractionManager.FretButtonStatesEnum.NotSelected;

    public GuitarInteractionManager.FretButtonStatesEnum CurrFretButtonStateEnum
    {
        get => currFretButtonStateEnum;
        set => currFretButtonStateEnum = value;
    }

    public Image GetImageComponent { get; private set; }

    public Button GetButtonComponent { get; private set; }

    public TextMeshProUGUI GetTextComponent { get; private set; }

    private void Awake()
    {
        GetImageComponent = GetComponent<Image>();
        GetButtonComponent = GetComponent<Button>();
        GetTextComponent = GetComponentInChildren<TextMeshProUGUI>();
        
        GetButtonComponent.onClick.AddListener(ButtonPressed);

        ActivateButtonState(false);
    }
    
    private void ButtonPressed()
    {
        Debug.Log($"Fuck string row = {StringRow}, fret number = {FretNumber} ");

        ActivateButtonState(currFretButtonStateEnum == GuitarInteractionManager.FretButtonStatesEnum.NotSelected);

        GuitarInteractionManager.FretButtonPressed(this);
    }

    public void ActivateButtonState(bool activate)
    {
        if (activate)
        {
            GetImageComponent.color = Color.chocolate;
            CurrFretButtonStateEnum = GuitarInteractionManager.FretButtonStatesEnum.Selected;
        }
        else
        {
            GetImageComponent.color = Color.white;
            CurrFretButtonStateEnum = GuitarInteractionManager.FretButtonStatesEnum.NotSelected;
        }
    }
}
