using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayTurnState
{
    public enum States { TurnStart, DrawCard, PlayHandCard, CardSelected, CardPlayed, ResolveBoard, TurnEnd }

    public abstract void Init();
    public abstract void Finish();
    public abstract bool Update(ref States nextState);


}
