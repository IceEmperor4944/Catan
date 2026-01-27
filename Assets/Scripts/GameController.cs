using NUnit.Framework;
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
    public int TotalSetupSteps => players.Count * 6;
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
                    int requiredType = (InSetupPhase ? setupStep % 3 == 0 ? 1 : 2 : 0);
                    if (point.CanPlaceAt(players[currentPlayerIndex].Color, requiredType))
                    {
                        var placedObject = board.PlaceObject(point, players[currentPlayerIndex]);
                        if (placedObject != null)
                        {

                            if (InSetupPhase)
                            {
                                var player = players[currentPlayerIndex];
                                for (int i = 0; i < 5; i++)
                                {
                                    
                                    if (!placedObject.cost.TryGetValue((ResourceType)i, out int resourceCount)) continue;
                                    player.GainResource((ResourceType)i, resourceCount);
                                    player.PanelController.UpdateResourcesText();
                                }
                                SetUp();
                                uiController.UpdateActionPanelStartup(setupStep);
                            }
                            //else NextTurn();
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

        checkForPlacement = true;
    }

    void SetUp()
    {
        // setupStep % (players.Count * 3) 
        // Reverse order on second half of setup
        // 

        // 2 players would have 6 actions
        // 0 % 6 && 8 % 6
        // 2 % 6 && 5 % 6

        //3 players would have 9 turns
        // 0 % 9 && 14 % 9
        // 2 % 9 && 11 % 9
        // 5 % 9 && 8 % 9

        //4 players would have 12 turns
        // 0 % 12 && 20 % 12
        // 2 % 12 && 17 % 12
        // 4 % 12 && 14 % 12
        // 8 % 12 && 11 % 12

        setupStep++;

        
    }

    public void NextTurn() // returns whether the game was won
    {
        if (InSetupPhase)
        {
            if (setupStep % (TotalSetupSteps * 0.5f) == 0 || setupStep % (TotalSetupSteps * 0.5f) == (PlayerCount + currentPlayerIndex) * 3 - 1 )
            {
                currentPlayerIndex = 0;
            }
            else if (setupStep % (TotalSetupSteps * 0.5f) == 2 || setupStep % (TotalSetupSteps * 0.5f) == (PlayerCount + currentPlayerIndex) * 3 - 1)
            {
                currentPlayerIndex = 1;
            }
            else if (setupStep % (TotalSetupSteps * 0.5f) == 5 || setupStep % (TotalSetupSteps * 0.5f) == (PlayerCount + currentPlayerIndex) * 3 - 1)
            {
                currentPlayerIndex = 2;
            }
            else if (setupStep % (TotalSetupSteps * 0.5f) == 8 || setupStep % (TotalSetupSteps * 0.5f) == (PlayerCount + currentPlayerIndex) * 3 - 1)
            {
                currentPlayerIndex = 3;
            }

            setupStep++;
            uiController.UpdateActionPanelStartup(setupStep);

        }
        else
        {
            currentPlayerIndex++;
            if (currentPlayerIndex >= PlayerCount) currentPlayerIndex = 0;
        }
        uiController.UpdateActionPanelStartup(setupStep);
        
    }
}
