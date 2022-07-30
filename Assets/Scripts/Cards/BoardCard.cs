using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoardCard : Card
{
    public event CardAbilityAction OnDeploy;
    public event CardAbilityAction OnDeath;

    private void Update()
    {
        TestInput();        
    }

    protected override void TestInput()
    {
        base.TestInput();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (OnDeploy != null) OnDeploy();
        }
    }
}
