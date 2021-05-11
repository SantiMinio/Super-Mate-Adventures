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
