using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FretButton : MonoBehaviour
{
    public int StringRow;
    public int FretNumber;
    
    public bool IsMuted { get; set; }
    
    public UnityEvent OnFretStateChanged;
    
    private GuitarInteractionManager.FretButtonStatesEnum currFretButtonStateEnum = GuitarInteractionManager.FretButtonStatesEnum.NotSelected;

    public GuitarInteractionManager.FretButtonStatesEnum CurrFretButtonStateEnum
    {
        get => currFretButtonStateEnum;
        set
        {
          //  Debug.Log($"[FretButton] Setting state for fret string={StringRow}, fret={FretNumber} to {value}");
            currFretButtonStateEnum = value;
            ActivateButtonState(value == GuitarInteractionManager.FretButtonStatesEnum.Selected);
        }
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
        Debug.Log($"[FretButton] Button Pressed: string={StringRow}, fret={FretNumber}");

        CurrFretButtonStateEnum = GuitarInteractionManager.FretButtonStatesEnum.Selected;
        
        GuitarInteractionManager.FretButtonPressed(this);
        
        OnFretStateChanged.Invoke();
    }

    public void ActivateButtonState(bool activate)
    {
      //  Debug.Log($"[FretButton] Activating state for fret string={StringRow}, fret={FretNumber}. Activate={activate}");
        if (activate)
        {
            if (IsMuted)
            {
                GetImageComponent.color = Color.grey;
            }
            else
            {
                GetImageComponent.color = Color.chocolate;
            }
        }
        else
        {
            GetImageComponent.color = Color.white;
        }
    }
}