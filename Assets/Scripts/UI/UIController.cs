using NUnit.Framework;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public enum ActionType
{
    BuildSettlement,
    BuildRoad,
    BuildCity,
    Trade,
    EndTurn
}

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject LeftInnerPanel;
    [SerializeField] GameObject RightInnerPanel;

    [SerializeField] GameObject PlayerPanelPrefab;
    [SerializeField] GameObject ActionPanelPrefab;
    [SerializeField] GameObject ResourcePanelPrefab;
    [SerializeField] GameController gameController;

    [SerializeField] TMP_Text statusDisplay;


    [Header("ActionPanels")]
    private List<ActionPanelController> actionPanels;

    private List<PlayerPanelController> playerPanels;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerPanels = new List<PlayerPanelController>();
        actionPanels = new List<ActionPanelController>();
        SetupPlayerPanels();
        SetupActionPanel();
    }

    private void SetupPlayerPanels()
    {
        for (int i = 0; i < gameController.PlayerCount; i++)
        {
            
            GameObject playerPanelInstance = Instantiate(PlayerPanelPrefab, RightInnerPanel.transform);
            PlayerPanelController panelController = playerPanelInstance.GetComponent<PlayerPanelController>();
            panelController.player = gameController.GetPlayer(i);
            gameController.GetPlayer(i).PanelController = panelController;
            RightInnerPanel.transform.SetParent(playerPanelInstance.transform);

            playerPanelInstance.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -150 * i);
            PlayerColor color = (PlayerColor)i;
            switch (color)
            {
                case PlayerColor.Red:
                    playerPanelInstance.GetComponent<Image>().color = Color.red;
                    break;
                case PlayerColor.White:
                    playerPanelInstance.GetComponent<Image>().color = Color.white;
                    break;
                case PlayerColor.Blue:
                    playerPanelInstance.GetComponent<Image>().color = Color.blue;
                    break;
                case PlayerColor.Yellow:
                    playerPanelInstance.GetComponent<Image>().color = Color.yellow;
                    break;
            }

            playerPanelInstance.name = "PlayerPanel_" + (i + 1);
            playerPanelInstance.GetComponentInChildren<TMP_Text>().text = $"Player {i + 1}: 0 VP";
            playerPanels.Add(playerPanelInstance.GetComponent<PlayerPanelController>());
            
            for (int j = 0; j < System.Enum.GetValues(typeof(ResourceType)).Length - 1; j++)
            {
                var resourcePanel = Instantiate(ResourcePanelPrefab, playerPanelInstance.transform);
                resourcePanel.transform.SetParent(playerPanelInstance.transform);
                resourcePanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100 + (j * 45), -35);
                resourcePanel.GetComponentInChildren<TMP_Text>().text = panelController.player.GetResourceCount((ResourceType)j).ToString();

                panelController.resourcePanels.Add(resourcePanel);
            }
            
        }

        for (int i = 0; i < playerPanels.Count; i++)
        {
            for (int j = 0; j < playerPanels.Count; j++)
            {
                if (i != j)
                {
                    playerPanels[i].otherPlayerPanels.Add(playerPanels[j]);
                }
            }
        }
    }

    private void SetupActionPanel()
    {
        foreach (ActionType action in System.Enum.GetValues(typeof(ActionType)))
        {
            GameObject actionPanelInstance = Instantiate(ActionPanelPrefab, LeftInnerPanel.transform);
            actionPanelInstance.transform.SetParent(LeftInnerPanel.transform);
            actionPanelInstance.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -75 * (int)action);
            actionPanelInstance.name = "ActionPanel_" + action.ToString();
            actionPanelInstance.GetComponentInChildren<TMP_Text>().text = Regex.Replace(action.ToString(), @"(?<!^)(?=[A-Z])", " ");
            actionPanelInstance.GetComponent<ActionPanelController>().action = action;
            actionPanelInstance.GetComponent<ActionPanelController>().UpdateActionAvailability(false);
            if (action == ActionType.EndTurn)
            {
                actionPanelInstance.GetComponent<ActionPanelController>().gameObject.AddComponent<Button>().onClick.AddListener(() =>
                {
                    gameController.NextTurn();
                });
            }
            actionPanels.Add(actionPanelInstance.GetComponent<ActionPanelController>());
        }

        

        UpdateActionPanelStartup(gameController.SetupStep);
    }

    public void UpdateActionPanelStartup(int startupCount)
    {
        var BuildSettlementController = actionPanels.Find(apc => apc.action == ActionType.BuildSettlement);
        var BuildRoadController = actionPanels.Find(apc => apc.action == ActionType.BuildRoad);
        var EndTurnController = actionPanels.Find(apc => apc.action == ActionType.EndTurn);

        if (startupCount % 2 == 0)
        {
            BuildSettlementController.UpdateActionAvailability(true);
            BuildRoadController.UpdateActionAvailability(false);
            EndTurnController.UpdateActionAvailability(false);

        }
        else if (startupCount % 2 == 1)
        {
            BuildSettlementController.UpdateActionAvailability(false);
            BuildRoadController.UpdateActionAvailability(true);
        }
        else
        {
            BuildSettlementController.UpdateActionAvailability(false);
            BuildRoadController.UpdateActionAvailability(false);
            EndTurnController.UpdateActionAvailability(true);
        }

    }

    public void SetStatus(string status)
    {
        statusDisplay.text = status;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
