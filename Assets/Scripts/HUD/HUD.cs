using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private HUDElement playerTurn;
    [SerializeField] private HUDElement opponentTurn;


    private void OnEnable()
    {
        PlayerTurnState.OnStateInit += playerTurn.SetTextContent;
    }

    private void OnDisable()
    {
        PlayerTurnState.OnStateInit -= playerTurn.SetTextContent;
    }



    private void Awake()
    {
        DisplayPlayerTurn();
    }

    public void DisplayPlayerTurn()
    {
        playerTurn.Show();
        opponentTurn.Hide();
    }

    public void DisplayOpponentTurn()
    {
        playerTurn.Hide();
        opponentTurn.Show();
    }

}
