using UnityEngine;
using UnityEngine.UI;

public class ChordButton : MonoBehaviour
{
    public int slotIndex;
    private Image buttonImage;

    void Awake()
    {
        buttonImage = GetComponent<Image>();
    }

    public void SetActive(bool isActive)
    {
        if (isActive)
        {
            buttonImage.color = Color.yellow; // Or any highlight color you prefer
        }
        else
        {
            buttonImage.color = Color.white; // The default color
        }
    }
}