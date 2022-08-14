using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : MonoBehaviour
{
    [SerializeField] Deck deck;
    [SerializeField] Hand hand;

    // STAGES:

    // from start:
    // 1. Turn Start
    //  - Trigger "turn start" abilities
    //  - Draw 1 card

    // after drawing the card:
    // 2. Turn Play
    //  - Player plays card(s) from hand
    //  - Pass, triggered by player

    // after passing:
    // 3. Turn End
    //  - add card points to score
    //  - check if winning score



    private void TurnStart()
    {
        //  - Trigger "turn start" abilities

        //  - Draw 1 card
        hand.AddCard(deck.DrawTopCard());
    }

    private void TurnPlay()
    {
        //  - Player plays card(s) from hand
        hand.PlayLeftmostCard();

        //  - Pass, triggered by player
        
    }





}
