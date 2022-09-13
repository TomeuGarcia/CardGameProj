using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAbilitiesDisplayer : MonoBehaviour
{
    [SerializeField] private Transform holderTransform;
    private List<GameObject> displayedAbilities = new List<GameObject>();


    public void AddAbilityAtSortedPosition(IAbility ability)
    {
        GameObject abilityDisplay = Instantiate(ability.abilityDisplayPrefab, holderTransform);
        abilityDisplay.transform.localPosition = new Vector3(displayedAbilities.Count * 0.2f, 0f, 0f);

        displayedAbilities.Add(abilityDisplay);
    }

    public void AddAbilityAtCenteredPosition(IAbility ability)
    {
        GameObject abilityDisplay = Instantiate(ability.abilityDisplayPrefab, holderTransform);

        displayedAbilities.Add(abilityDisplay);
    }


    public void RemoveAbilityAtIndex(int index)
    {
        displayedAbilities.RemoveAt(index);
    }


}
