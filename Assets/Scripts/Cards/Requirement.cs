using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Requirement
{

    public abstract bool CheckRequirement();

    public abstract void RequirementEffect();
}

[Serializable]
public class ManaRequirement : Requirement
{
    [SerializeField] int requiredMana = 10;

    public override bool CheckRequirement() => Main.instance.GetMainCharacter.manaSystem.EnoughMana(requiredMana);

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

    public override void RequirementEffect()
    {
    }
}

[Serializable]
public class DistanceRequirement : Requirement
{
    [SerializeField] float distance = 7;
    [SerializeField] Transform root;
    [SerializeField] string typeName = "CharacterHead";

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

    public override bool CheckRequirement() => !Main.instance.GetMainCharacter.GetComponent<Frano.CharController>().IsSpeedIncreased();

    public override void RequirementEffect()
    {
        Main.instance.GetMainCharacter.GetComponent<Frano.CharController>().IncreaseSpeed(speedToIncrease);
    }
}
