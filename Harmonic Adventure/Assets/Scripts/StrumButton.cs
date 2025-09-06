using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StrumButton : MonoBehaviour
{
    public int StrumNumber;
    
    public GuitarInteractionManager.StrumButtonStatesEnum currStrumButtonStateEnum =
        GuitarInteractionManager.StrumButtonStatesEnum.Skip;

    public GuitarInteractionManager.StrumButtonStatesEnum CurrStrumButtonStateEnum
    {
        get => currStrumButtonStateEnum;
        set => currStrumButtonStateEnum = value;
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

        UpdateButtonState(GuitarInteractionManager.StrumButtonStatesEnum.Skip);
    }
    
    private void ButtonPressed()
    {
        switch (CurrStrumButtonStateEnum)
        {
            case GuitarInteractionManager.StrumButtonStatesEnum.Skip:
                UpdateButtonState(GuitarInteractionManager.StrumButtonStatesEnum.Down);
                break;
            case GuitarInteractionManager.StrumButtonStatesEnum.Down:
                UpdateButtonState(GuitarInteractionManager.StrumButtonStatesEnum.Up);
                break;
            case GuitarInteractionManager.StrumButtonStatesEnum.Up:
                UpdateButtonState(GuitarInteractionManager.StrumButtonStatesEnum.Skip);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        Debug.Log($"Fuck Strum number pressed = {StrumNumber} to strum state = {CurrStrumButtonStateEnum}");
        
        GuitarInteractionManager.StrumButtonPressed(this);
    }
    
    public void UpdateButtonState(GuitarInteractionManager.StrumButtonStatesEnum state)
    {
        //string pattern = "↓ ↓ ↑ ↓";
        
        switch (state)
        {
            case GuitarInteractionManager.StrumButtonStatesEnum.Down:
                CurrStrumButtonStateEnum = GuitarInteractionManager.StrumButtonStatesEnum.Down;
                GetTextComponent.text = "↓";
                break;
            case GuitarInteractionManager.StrumButtonStatesEnum.Up:
                CurrStrumButtonStateEnum = GuitarInteractionManager.StrumButtonStatesEnum.Up;
                GetTextComponent.text = "↑";
                break;
            case GuitarInteractionManager.StrumButtonStatesEnum.Skip:
                CurrStrumButtonStateEnum = GuitarInteractionManager.StrumButtonStatesEnum.Skip;
                GetTextComponent.text = "-";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
}

