using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] Hand hand;
    [SerializeField] Deck deck;
    [SerializeField] Board board;
    [SerializeField] PassButton passButton;

    enum TurnStage { Draw, Play, AddUp }
    TurnStage turnStage = TurnStage.Play;

    private delegate bool StageFunction();
    private StageFunction[] stageFunctions;


    private void OnEnable()
    {
        board.OnCardDemand += PlaceSelectedCardOnBoard;
    }
    private void OnDisable()
    {
        board.OnCardDemand -= PlaceSelectedCardOnBoard;
    }

    private void Awake()
    {
        stageFunctions = new StageFunction[3] { DrawStageUpdate, PlayStageUpdate, AddUpStageUpdate };
    }

    private void Start()
    {
        StartCoroutine(LateDraw());
    }

    private IEnumerator LateDraw()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 4; ++i)
        {
            hand.AddCard(deck.DrawTopCard());
            yield return new WaitForSeconds(0.2f);
        }
    }



    private void Update()
    {
        if (stageFunctions[(int)turnStage]())
        {
            turnStage = (TurnStage)((int)(turnStage + 1) % stageFunctions.Length);
        }
    }


    // DRAW STAGE
    private bool DrawStageUpdate()
    {
        //if (Input.GetKeyDown(KeyCode.Z))
        if (deck.testingIsClicked)
        {
            if (!deck.IsEmpty)
            {
                hand.AddCard(deck.DrawTopCard());
                return true;
            }
        }

        return deck.IsEmpty;
    }


    // PLAY STAGE
    private bool PlayStageUpdate()
    {
        if (hand.HandUpdate(out Card cardToPlay))
        {
            hand.PlaySelectedCard();
            return true;
        }
        return false;
    }

    private void PlaceSelectedCardOnBoard(BoardSlot boardSlot)
    {
        if (!hand.HasSelectedCard) return;

        hand.SelectedCard.cardTransform.MoveOriginToMeshPosition();
        hand.SelectedCard.cardTransform.MoveToPosition(boardSlot.CardPosition, 0.2f);
        hand.SelectedCard.cardTransform.Rotate(boardSlot.transform.rotation, 0.2f);

        hand.PlaySelectedCard();
    }




    // BAORD ADD UP STAGE
    private bool AddUpStageUpdate()
    {
        board.AddUpScore();
        return true;
    }


}
