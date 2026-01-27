using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerPanelController : MonoBehaviour
{

    public List<PlayerPanelController> otherPlayerPanels = new List<PlayerPanelController>();
    public List<GameObject> resourcePanels = new List<GameObject>();

    bool IsPanelOpen = false;
    private RectTransform rectTransform;
    [SerializeField] GameObject PlayerText;
    public Player player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InteractWithPanel()
    {
        int i = 0;
        if (IsPanelOpen)
        {
            ClosePanel(true);
        }
        else
        {
            OpenPanel(true);
        }
    }

    public void UpdateResourcesText()
    {
        TMP_Text playerInfoText = GetComponentInChildren<TMP_Text>();
        string playername = playerInfoText.text.Split(":")[0];
        playerInfoText.text = playername + ": " + player.VictoryPoints + " VP";

        for (int i = 0; i < resourcePanels.Count; i++)
        {
            TMP_Text resourceText = resourcePanels[i].GetComponentInChildren<TMP_Text>();
            resourceText.text = player.GetResourceCount((ResourceType)i).ToString();

        }
    }

    private void OpenPanel(bool initialCall)
    {
        RectTransform playerTextRect = PlayerText.GetComponent<RectTransform>();
        
        playerTextRect.anchorMin = new Vector2(0.5f, 0.5f);
        playerTextRect.anchorMax = new Vector2(0.5f, 0.6f);
        playerTextRect.pivot = new Vector2(0.5f, 0.7f);
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 170);
        IsPanelOpen = true;

        foreach(var resourcePanel in resourcePanels)
        {
            resourcePanel.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
        }



        for (int i = 0; i < otherPlayerPanels.Count; i++)
        {
            if (otherPlayerPanels[i].IsPanelOpen)
            {
                otherPlayerPanels[i].ClosePanel(false);
            }
            if (otherPlayerPanels[i].rectTransform.anchoredPosition.y < rectTransform.anchoredPosition.y)
            {
                otherPlayerPanels[i].rectTransform.anchoredPosition = new Vector2(otherPlayerPanels[i].rectTransform.anchoredPosition.x, otherPlayerPanels[i].rectTransform.anchoredPosition.y - 30);
            }
        }
    }

    private void ClosePanel(bool initialCall)
    {
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 130);
        IsPanelOpen = false;



        foreach (var resourcePanel in resourcePanels)
        {
            resourcePanel.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 30);
        }


        for (int i = 0; i < otherPlayerPanels.Count; i++)
        {
            if (otherPlayerPanels[i].rectTransform.anchoredPosition.y < rectTransform.anchoredPosition.y)
            {
                otherPlayerPanels[i].rectTransform.anchoredPosition = new Vector2(otherPlayerPanels[i].rectTransform.anchoredPosition.x, otherPlayerPanels[i].rectTransform.anchoredPosition.y + 30);
            }
        }
        if (!initialCall) return;
    }
}
