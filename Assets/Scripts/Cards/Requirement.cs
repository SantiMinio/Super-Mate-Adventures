using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Requirement
{

    public abstract bool CheckRequirement();

    public abstract void RequirementEffect();

    public abstract float GetRequirment();
}

[Serializable]
public class ManaRequirement : Requirement
{
    [SerializeField] int requiredMana = 10;

    public override bool CheckRequirement() => Main.instance.GetMainCharacter.manaSystem.EnoughMana(requiredMana);

    public override float GetRequirment() => requiredMana;

    public override void RequirementEffect()
    {
        Main.instance.GetMainCharacter.manaSystem.ModifyMana(-requiredMana);
    }
}

[Serializable]
public class LifeRequirement : Requirement
{
    [SerializeField] float requiredLife = 10;

    public override bool CheckRequirement() => Main.instance.GetMainCharacter.GetComponent<LifeHandler>().CheckEnoughLife(requiredLife);

    public override float GetRequirment() => requiredLife;

    public override void RequirementEffect()
    {
        Main.instance.GetMainCharacter.GetComponent<LifeHandler>().TakeDamage(requiredLife);
    }
}

[Serializable]
public class DistanceRequirement : Requirement
{
    [SerializeField] float distance = 7;
    [SerializeField] Transform root;
    [SerializeField] string typeName = "CharacterHead";

    public override float GetRequirment() => distance;

    public override bool CheckRequirement()
    {
        var overlap = Physics.OverlapSphere(root.position, distance);
        foreach (var item in overlap)
        {
            if (item.GetComponent(typeName))
                return true;
        }

        return false;
    }

    public override void RequirementEffect()
    {
    }
}

[Serializable]
public class SpeedRequirement : Requirement
{
    [SerializeField] float speedToIncrease = 6;

    public override float GetRequirment() => 0;

    public override bool CheckRequirement() => !Main.instance.GetMainCharacter.GetComponent<Frano.CharController>().IsSpeedIncreased();

    public override void RequirementEffect()
    {
        Main.instance.GetMainCharacter.GetComponent<Frano.CharController>().IncreaseSpeed(speedToIncrease);
    }
}

[Serializable]
public class LastCardRequirement : Requirement
{
    [SerializeField] float timeToCombo = 3;
    [SerializeField] CardSettings cardUsedRequire = null;

    public override float GetRequirment() => 0;

    public override bool CheckRequirement()
    {
        Debug.Log(DeckOfCards.privateInstance.LastCard());
        if (DeckOfCards.privateInstance.TimeToUseLastCard() <= timeToCombo &&
            cardUsedRequire.title == DeckOfCards.privateInstance.LastCard().title)
            return true;
        else
            return false;
    }

    public override void RequirementEffect()
    {
        
    }
}

[Serializable]
public class CardOnHandRequirement : Requirement
{
    [SerializeField] int cardRequireAmmount = 3;
    [SerializeField] CardSettings cardRequire = null;

    public override float GetRequirment() => cardRequireAmmount;

    public override bool CheckRequirement()
    {
        if (DeckOfCards.privateInstance.IsInMyHand(cardRequire, cardRequireAmmount))
            return true;
        else
            return false;
    }

    public override void RequirementEffect()
    {
        DeckOfCards.privateInstance.DiscardCardOfType(cardRequire, cardRequireAmmount - 1);
    }
}
