using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoardCard : Card
{
    public event CardAbilityAction OnDeploy;
    public event CardAbilityAction OnDeath;



}
