using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObserver : MonoBehaviour
{
    [Header("Subjects")]
    [SerializeField] private Hand hand;
    [SerializeField] private Deck deck;
    [SerializeField] private Deck manaDeck;
    [SerializeField] private FriendlyBoardSide friendlyBoard;
    [SerializeField] private BoardSide opposingBoard;
    [SerializeField] private PassButton passButton;

    [Header("Game")]
    [SerializeField] private int startHandUnitCards = 2;
    [SerializeField] private int startHandSpecialCards = 1;

    private UnitCard selectedUnitCard;
    private SpecialCard selectedSpecialCard;


    // Game States
    Dictionary<PlayerTurnState.States, PlayerTurnState> playerTurnStates;
    PlayerTurnState.States currentTurnState;


    public delegate void GameObserverAction();
    public event GameObserverAction OnCardDrawn;
    public event GameObserverAction OnHoldingSelectedCard;
    public event GameObserverAction OnHoldingUnitCard;
    public event GameObserverAction OnHoldingSpecialCard;
    public event GameObserverAction OnStopHoldingSelectedCard;
    public event GameObserverAction OnTurnPass;
    public event GameObserverAction OnCardPlayed;


    private void OnEnable()
    {
        EnableHoldingSelectedCard();
    }

    private void OnDisable()
    {
        DisableHoldingSelectedCard();
    }

    private void Awake()
    {
        DrawStartHand();
        InitPlayerTurnStates();
    }

    private void Update()
    {
        if (playerTurnStates[currentTurnState].Update())
        {
            playerTurnStates[currentTurnState].Finish();
            currentTurnState = playerTurnStates[currentTurnState].nextState;

            playerTurnStates[currentTurnState].Init();
        }
    }



    private void InitPlayerTurnStates()
    {
        playerTurnStates = new Dictionary<PlayerTurnState.States, PlayerTurnState>();
        playerTurnStates.Add(PlayerTurnState.States.TurnStart, new PlayerTurnStart(this));
        playerTurnStates.Add(PlayerTurnState.States.DrawCard, new PlayerDrawCard(this));
        playerTurnStates.Add(PlayerTurnState.States.Thinking, new PlayerThinking(this));
        playerTurnStates.Add(PlayerTurnState.States.HoldingSelectedCard, new PlayerHoldingSelectedCard(this));
        playerTurnStates.Add(PlayerTurnState.States.TurnEnd, new PlayerTurnEnd(this));

        currentTurnState = PlayerTurnState.States.Thinking;
        playerTurnStates[currentTurnState].Init();
    }

 





    // Draw Card
    public void EnableDrawCardFromDecks()
    {
        deck.OnCardDrawDemand += DrawCardFromDeck;
        manaDeck.OnCardDrawDemand += DrawCardFromDeck;
    }

    public void DisableDrawCardFromDecks()
    {
        deck.OnCardDrawDemand -= DrawCardFromDeck;
        manaDeck.OnCardDrawDemand -= DrawCardFromDeck;
    }

    public bool CanDrawCards()
    {
        return deck.HasCards || manaDeck.HasCards;
    }



    // Board Interaction
    public void EnablePlayUnitCardsOnBoard()
    {
        friendlyBoard.EnablePlayUnitCards();
    }
    public void DisablePlayUnitCardsOnBoard()
    {
        friendlyBoard.DisablePlayUnitCards();
    }
    private void EnablePlaySelectedUnitCard()
    {
        friendlyBoard.OnUnitCardDemand += PlaySelectedUnitCard;
    }
    private void DisablePlaySelectedUnitCard()
    {
        friendlyBoard.OnUnitCardDemand -= PlaySelectedUnitCard;
    }

    public void EnablePlaySpecialCards()
    {
        friendlyBoard.EnablePlaySpecialCards();
    }
    public void DisablePlaySpecialCards()
    {
        friendlyBoard.DisablePlaySpecialCards();
    }
    private void EnablePlaySelectedSpecialCard()
    {
        friendlyBoard.OnSpecialCardDemand += PlaySelectedSpecialCard;
    }
    private void DisablePlaySelectedSpecialCard()
    {
        friendlyBoard.OnSpecialCardDemand -= PlaySelectedSpecialCard;
    }

    // Holding Selected Card
    public void EnableHoldingSelectedCard()
    {
        UnitCard.OnCardCanBePlayedQuery += TestIfUnitCardCanBePlayed;
        SpecialCard.OnCardCanBePlayedQuery += TestIfSpecialCardCanBePlayed;
    }
    public void DisableHoldingSelectedCard()
    {
        UnitCard.OnCardCanBePlayedQuery -= TestIfUnitCardCanBePlayed;
        SpecialCard.OnCardCanBePlayedQuery -= TestIfSpecialCardCanBePlayed;
    }

    private void TestIfUnitCardCanBePlayed(UnitCard card)
    {
        if (friendlyBoard.score >= card.cost && !friendlyBoard.IsFull())
        {
            hand.ProceedSelectCardToPlay(card);
            InvokeOnHoldingSelectedCard();
            if (OnHoldingUnitCard != null) OnHoldingUnitCard();



            EnablePlaySelectedUnitCard();
            
            //...

            selectedUnitCard = card;

            hand.OnCardUnselected += DisablePlaySelectedUnitCard;
        }
    }

    private void TestIfSpecialCardCanBePlayed(SpecialCard card)
    {
        hand.ProceedSelectCardToPlay(card);
        InvokeOnHoldingSelectedCard();
        if (OnHoldingSpecialCard != null) OnHoldingSpecialCard();

        EnablePlaySelectedSpecialCard();

        selectedSpecialCard = card;

        hand.OnCardUnselected += DisablePlaySelectedSpecialCard;
    }




    // Stop Holding Selected Card
    public void EnableStopHoldingSelectedCard()
    {
        hand.OnCardUnselected += InvokeOnStopHoldingSelectedCard;
    }

    public void DisableStopHoldingSelectedCard()
    {
        hand.OnCardUnselected -= InvokeOnStopHoldingSelectedCard;
    }



    // Pass Button Interaction
    public void EnablePassButtonInteraction()
    {
        passButton.canPass = true;
        passButton.OnPass += PassTurn;
    }

    public void DisablePassButtonInteraction()
    {
        passButton.canPass = false;
        passButton.OnPass -= PassTurn;
    }


    // Card Selection
    public void EnableHandCardSelection()
    {
        hand.EnableCardsSelection();
    }

    public void DisableHandCardSelection()
    {
        hand.DisableCardsSelection();
    }





    // OPERATIONS
    private void DrawCardFromDeck(Deck deck)
    {
        if (OnCardDrawn != null) OnCardDrawn();

        hand.AddCard(deck.DrawTopCard());
        hand.ArrangeCards();
    }

    private void DrawStartHand()
    {
        for (int i = 0; i < startHandUnitCards; ++i)
        {
            DrawCardFromDeck(deck);
        }
        for (int i = 0; i < startHandSpecialCards; ++i)
        {
            DrawCardFromDeck(manaDeck);
        }
    }


    private void PlaySelectedUnitCard()
    {
        friendlyBoard.PlayCardInSelectedBoardSlot(selectedUnitCard);
        hand.RemoveSelectedCard();

        selectedUnitCard.Play();
        selectedUnitCard = null;

        if (OnCardPlayed != null) OnCardPlayed();

        DisablePlaySelectedUnitCard();
    }

    private void PlaySelectedSpecialCard()
    {       
        if (friendlyBoard.IsBoardSlotSelected)
        {
            selectedSpecialCard.SetTargetingUnitCard(friendlyBoard.UnitInSelectedBoardSlot);
            friendlyBoard.PlaySpecialCardOnUnit(selectedSpecialCard);
        }
        else
        {
            friendlyBoard.PlaySpecialCard(selectedSpecialCard);
        }            

        hand.RemoveSelectedCard();

        selectedSpecialCard.Play();
        selectedSpecialCard = null;


        if (OnCardPlayed != null) OnCardPlayed();

        DisablePlaySelectedSpecialCard();
    }


    private void PassTurn()
    {
        if (OnTurnPass != null) OnTurnPass();
    }

    public void ResolveBoard()
    {
        // Also Trigger abilities if needed, etc.

        friendlyBoard.AddUnitPointsToScore();
    }


    private void InvokeOnHoldingSelectedCard()
    {
        if (OnHoldingSelectedCard != null) OnHoldingSelectedCard();
    }
    
    private void InvokeOnStopHoldingSelectedCard()
    {
        if (OnStopHoldingSelectedCard != null) OnStopHoldingSelectedCard();
    }

}
