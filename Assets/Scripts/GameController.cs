using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    public GameBoard board;

    private List<Player> players = new();
    private int currentPlayerIndex = 0;
    private bool checkForPlacement;
    private int setupStep = 0;

    public int PlayerCount => players.Count;
    public bool InSetupPhase => setupStep < players.Count * 4;
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
                            }
                            else NextTurn();
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
        // setupStep % (players.Count * 2) || setupStep % (players.Count * 2) == (players.Count - currentPlayerIndex * 2)
        //

        // 2 players would have 4 turns
        // 0 % 4 && 6 % 4
        // 2 % 4 && 4 % 4

        //3 players would have 6 turns
        // 0 % 6 && 10 % 6
        // 2 % 6 && 8 % 6
        // 4 % 6 && 6 % 6

        //4 players would have 8 turns
        // 0 % 8 && 14 % 8
        // 2 % 8 && 12 % 8
        // 4 % 8 && 10 % 8
        // 6 % 8 && 8 % 8

        setupStep++;

        if (setupStep % (PlayerCount * 2) == 0 || setupStep % (PlayerCount * 2) == (PlayerCount - currentPlayerIndex * 2))
        {
            currentPlayerIndex = 0;
        }
        if (setupStep % (PlayerCount * 2) == 2 || setupStep % (PlayerCount * 2) == (PlayerCount - currentPlayerIndex * 2))
        {
            currentPlayerIndex = 1;
        }
        if (setupStep % (PlayerCount * 2) == 4 || setupStep % (PlayerCount * 2) == (PlayerCount - currentPlayerIndex * 2))
        {
            currentPlayerIndex = 2;
        }
        if (setupStep % (PlayerCount * 2) == 6 || setupStep % (PlayerCount * 2) == (PlayerCount - currentPlayerIndex * 2))
        {
            currentPlayerIndex = 3;
        }
    }

    bool NextTurn() // returns whether the game was won
    {
        currentPlayerIndex++;
        if (currentPlayerIndex >= PlayerCount) currentPlayerIndex = 0;
        return players[currentPlayerIndex].VictoryPoints == 10;
    }
}
