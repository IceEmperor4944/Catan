using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    public GameBoard board;

    [SerializeField] UIController uiController;

    private List<Player> players = new();
    private int currentPlayerIndex = 0;
    private bool checkForPlacement;
    private int setupStep = 0;

    public int SetupStep => setupStep;

    public int PlayerCount => players.Count;
    public int TotalSetupSteps => players.Count * 4;
    public bool InSetupPhase => setupStep < TotalSetupSteps; // 3 actions per player (Settlement, Road, End) * 2 turns of setup

    void Start()
    {
        StartGame(2);
    }

    public Player GetPlayer(int index)
    {
        if (index < 0 || index >= PlayerCount)
        {
            return null;
        }
        return players[index];
    }
    void Update()
    {
        if (checkForPlacement && Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                PlacePoint point;
                if (hit.collider.TryGetComponent<PlacePoint>(out point)) 
                {
                    int requiredType = (InSetupPhase ? setupStep % 2 == 0 ? 1 : 2 : 0);
                    if (point.CanPlaceAt(players[currentPlayerIndex].Color, requiredType))
                    {
                        var placedObject = board.PlaceObject(point, players[currentPlayerIndex], InSetupPhase);
                        if (placedObject != null)
                        {
                            Player player = players[currentPlayerIndex];
                            placedObject.Owner = player;
                            player.VictoryPoints += placedObject.VPValue;

                            if (InSetupPhase)
                            {
                                SetUp();
                                uiController.UpdateActionPanelStartup(setupStep);
                            }
                            players[currentPlayerIndex].PanelController.UpdateResourcesText();
                        }
                    }
                }
            }
        }
    }

    void StartGame(int playerCount)
    {
        setupStep = 0;
        board.RandomizeBoard();
        players.Clear();
        for (int i = 0; i < playerCount; i++)
        {
            Player newPlayer = new Player((PlayerColor)i);
            players.Add(newPlayer);
        }
        uiController.SetStatus($"Player 1: Place a settlement");
        checkForPlacement = true;
    }

    void SetUp()
    {
        switch (PlayerCount)
        {
            case 2:
            {
                if (setupStep == 1) currentPlayerIndex = 1;
                else if (setupStep == 5) currentPlayerIndex = 0;
                break;
            }
            case 3:
            {
                if (setupStep == 1 || setupStep == 7) currentPlayerIndex = 1;
                else if (setupStep == 3) currentPlayerIndex = 2;
                else if (setupStep == 9) currentPlayerIndex = 0;
                break;
            }
            case 4:
            {
                if (setupStep == 1 || setupStep == 11) currentPlayerIndex = 1;
                if (setupStep == 3 || setupStep == 9) currentPlayerIndex = 2;
                if (setupStep == 5) currentPlayerIndex = 3;
                if (setupStep == 13) currentPlayerIndex = 0;
                break;
            }
        }

        uiController.UpdateActionPanelStartup(setupStep);
        setupStep++;
        uiController.SetStatus($"Player {currentPlayerIndex + 1}: Place a " + (setupStep % 2 == 0 ? "settlement" : "road"));

        if (setupStep == TotalSetupSteps) 
        {
            board.TriggerAllTiles();
            for (int i = 0; i < players.Count; i++)
            {
                players[i].PanelController.UpdateResourcesText();
            }

            currentPlayerIndex = -1; // shhhh
            NextTurn();
        }
    }

    public void NextTurn() // returns whether the game was won
    {
        if (InSetupPhase) return;
        currentPlayerIndex++;

        if (currentPlayerIndex >= PlayerCount) currentPlayerIndex = 0;
        uiController.UpdateActionPanelStartup(setupStep);

        int numberRolled = UnityEngine.Random.Range(1, 6) + UnityEngine.Random.Range(1, 6);
        board.CollectResources(numberRolled);
        for (int i = 0; i < players.Count; i++)
        {
            players[i].PanelController.UpdateResourcesText();
        }
        uiController.SetStatus($"Player {currentPlayerIndex + 1}'s turn! {numberRolled} was rolled!");
    }
}
