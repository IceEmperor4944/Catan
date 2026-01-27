using UnityEngine;
using UnityEngine.UI;

public class ActionPanelController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public ActionType action;
    [SerializeField] Image actionPanelImage;

    [SerializeField] Color availableColor;
    [SerializeField] Color unavailableColor;

    void Start()
    {
        
    }

    public bool IsActionAvailable()
    {
        // Placeholder logic for action availability
        return true;
    }

    public void UpdateActionAvailability()
    {
        if (IsActionAvailable())
        {
            actionPanelImage.color = availableColor;
        }
        else
        {
            actionPanelImage.color = unavailableColor;
        }
    }

    public void UpdateActionAvailability(bool isAvailable)
    {
        if (isAvailable)
        {
            actionPanelImage.color = availableColor;
        }
        else
        {
            actionPanelImage.color = unavailableColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
