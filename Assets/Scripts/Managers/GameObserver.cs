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
    [SerializeField] private OpposingBoardSide opposingBoard;
    [SerializeField] private PassButton passButton;

    [Header("Game")]
    [SerializeField] private int startHandUnitCards = 2;
    [SerializeField] private int startHandSpecialCards = 1;
    
    [SerializeField] public Transform aiCardsTransform;


    [Header("Camera")]
    public CameraManager cameraManager = new CameraManager();

    [Header("AI")]
    [SerializeField] public AICardSequence aiCardSequence;
    [HideInInspector] public CardQueue aiCardQueue;

    // Game State
    private GameStateManager gameStateManager;

    // Game Queries
    private GameQuerier gameQuerier;


    // 
    private UnitCard selectedUnitCard;
    private SpecialCard selectedSpecialCard;



    public delegate void GameObserverAction();
    public event GameObserverAction OnCardDrawn;
    public event GameObserverAction OnHoldingSelectedCard;
    public event GameObserverAction OnHoldingUnitCard;
    public event GameObserverAction OnHoldingSpecialCard;
    public event GameObserverAction OnStopHoldingSelectedCard;
    public event GameObserverAction OnTurnPass;
    public event GameObserverAction OnCardPlayed;
    public event GameObserverAction OnBoardFinishedResolving;


    private void OnEnable()
    {
        EnableHoldingSelectedCard();

        CardSpawner.OnSpawnCardInHand += SpawnCardInHand;
    }

    private void OnDisable()
    {
        DisableHoldingSelectedCard();

        CardSpawner.OnSpawnCardInHand -= SpawnCardInHand;
    }

    private void Awake()
    {
        aiCardSequence.Init();
        aiCardQueue = new CardQueue(this, opposingBoard);

        DrawStartHand();

        gameStateManager = new GameStateManager(this);

        gameQuerier = new GameQuerier(friendlyBoard, opposingBoard);

        cameraManager.SetHandCamera();


        QueueTurnCards(aiCardSequence, aiCardQueue); // Plays turn 0 cards sequence
        WarnUpcomingTurnCards(aiCardSequence);
    }


    private void Update()
    {
        gameStateManager.Update();
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
    public void AddCardToHand(Card card)
    {
        hand.AddCard(card);
        hand.ArrangeCards();
    }
    
    private void DrawCardFromDeck(Deck deck)
    {
        if (OnCardDrawn != null) OnCardDrawn();

        AddCardToHand(deck.DrawTopCard());
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

    public void ResolveFriendlyBoard()
    {
        StartCoroutine(ResolveBoard(friendlyBoard, opposingBoard));

        hand.TriggerEndOfTurn();
    }
    public void ResolveOpponentBoard()
    {
        StartCoroutine(ResolveBoard(opposingBoard, friendlyBoard));
    }
    private IEnumerator ResolveBoard(BoardSide attackingSide, BoardSide defendingSide)
    {
        // Also Trigger abilities if needed, etc.
        yield return null;

        for (int i = 0; i < attackingSide.BoardSlotsCount; ++i)
        {
            if (attackingSide.boardSlots[i].HasCard)
            {
                UnitCard attacker = attackingSide.boardSlots[i].card;

                if (defendingSide.boardSlots[i].HasCard) // Cards Fight
                {
                    UnitCard defender = defendingSide.boardSlots[i].card;

                    int powerHolder = defender.Power;
                    defender.TakeDamageFromUnitCard(attacker.Power, attacker);
                    attacker.TakeDamageFromUnitCard(powerHolder, defender);

                    defender.cardTransform.PlayAttackAnimation(defendingSide.attackForward);
                }
                else
                {
                    attackingSide.AddUnitPointsToScore(attacker);
                }

                float waitTime = attacker.cardTransform.PlayAttackAnimation(attackingSide.attackForward);
                waitTime += 0.5f;

                yield return new WaitForSeconds(waitTime);
            }
        }

        if (OnBoardFinishedResolving != null) OnBoardFinishedResolving();
    }

    public void FriendlyTurnStart()
    {
        TurnStart(friendlyBoard);
    }

    public void OpponentTurnStart()
    {
        TurnStart(opposingBoard);
    }

    private void TurnStart(BoardSide boardSide)
    {
        foreach (BoardSlot boardSlot in boardSide.boardSlots)
        {
            if (boardSlot.HasCard) 
            {
                boardSlot.card.InvokeOnTurnStart();
            }
        }
    }



    private void InvokeOnHoldingSelectedCard()
    {
        if (OnHoldingSelectedCard != null) OnHoldingSelectedCard();
    }
    
    private void InvokeOnStopHoldingSelectedCard()
    {
        if (OnStopHoldingSelectedCard != null) OnStopHoldingSelectedCard();
    }



    // AI Plays

    public void QueueTurnCards(AICardSequence cardSequence, CardQueue cardQueue)
    {
        AICardSequence.Play[] plays;
        if (cardSequence.GetCurrentTurnPlays(out plays))
        {
            foreach (AICardSequence.Play play in plays)
            {
                cardQueue.QueueCard(play.cardPrefab, play.boardSlotIndex);
            }
        }

        cardSequence.IncrementTurn();

        cardQueue.PlayQueuedUnitCards();
    }

    public void SpawnAndPlayUnitCard(GameObject cardPrefab, BoardSide boardSide, int boardSlotI)
    {
        UnitCard unitCard = Instantiate(cardPrefab, aiCardsTransform).GetComponent<UnitCard>();

        boardSide.PlayCardAtBoardSlotI(unitCard, boardSlotI);

        unitCard.Play();

        //if (OnCardPlayed != null) OnCardPlayed();
    }


    public void WarnUpcomingTurnCards(AICardSequence cardSequence)
    {
        int[] boardSlotIndices;
        if (cardSequence.GetCurrentTurnBoardSlotIndicesPlays(out boardSlotIndices))
        {
            foreach (int index in boardSlotIndices)
            {
                WarnUpcomingCardAtBoardSlot(index);
            }
        }
    }

    public void WarnUpcomingCardAtBoardSlot(int boardSlotIndex)
    {
        opposingBoard.EnableWarning(boardSlotIndex);
    }
    
    public void StopWarnUpcomingCardAtBoardSlot(int boardSlotIndex)
    {
        opposingBoard.DisableWarning(boardSlotIndex);
    }




    // Spawn Cards
    private void SpawnCardInHand(GameObject cardPrefab, out Card spawnedCard)
    {
        spawnedCard = Instantiate(cardPrefab, hand.spawnTransform).GetComponent<Card>();
        AddCardToHand(spawnedCard);
    }

}
