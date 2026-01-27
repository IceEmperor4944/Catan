using System;
using System.Collections;
using System.Collections.Generic;
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

    [Header("Dice")]
    [SerializeField] Rigidbody die1;
    [SerializeField] Rigidbody die2;
    private int rollNumber;

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
        
        rollNumber = 0;
        StartCoroutine(RollAllDice());
    }

    IEnumerator RollDie(Rigidbody rb)
    {
        int turnX = UnityEngine.Random.Range(300, 700);
        int turnY = UnityEngine.Random.Range(300, 700);
        int turnZ = UnityEngine.Random.Range(300, 700);
        Vector3 axis = new Vector3(turnX, turnY, turnZ);
        rb.AddRelativeTorque(axis);

        yield return new WaitForSeconds(2);

        rb.angularVelocity = new Vector3(0, 0, 0);

        float side1 = Vector3.Angle(rb.transform.up, Vector3.up);
        float side2 = Vector3.Angle(rb.transform.forward, Vector3.up);
        float side3 = Vector3.Angle(-rb.transform.right, Vector3.up);
        float side4 = Vector3.Angle(rb.transform.right, Vector3.up);
        float side5 = Vector3.Angle(-rb.transform.forward, Vector3.up);
        float side6 = Vector3.Angle(-rb.transform.up, Vector3.up);

        float side = Mathf.Min(side1, side2, side3, side4, side5, side6);
        if (side == side1)
        {
            rollNumber += 1;
        }
        else if (side == side2)
        {
            rollNumber += 2;
        }
        else if (side == side3)
        {
            rollNumber += 3;
        }
        else if (side == side4)
        {
            rollNumber += 4;
        }
        else if (side == side5)
        {
            rollNumber += 5;
        }
        else if (side == side6)
        {
            rollNumber += 6;
        }
    }

    IEnumerator RollAllDice()
    {
        yield return RollDie(die1);
        yield return RollDie(die2);

        board.CollectResources(rollNumber);
        for (int i = 0; i < players.Count; i++)
        {
            players[i].PanelController.UpdateResourcesText();
        }

        uiController.SetStatus($"Player {currentPlayerIndex + 1}'s turn!\n{rollNumber} was rolled!");
    }
}
