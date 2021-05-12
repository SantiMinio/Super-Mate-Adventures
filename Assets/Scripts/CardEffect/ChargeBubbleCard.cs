using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeBubbleCard : CardModel
{
    [SerializeField] LastCardRequirement lastCardRequirement = new LastCardRequirement();
    [SerializeField] int normalManaAmmount = 1;


    public override void RangeFeedback()
    {
    }

    public override void ResetCard()
    {
    }

    protected override bool OnCanUse()
    {
        return !Main.instance.GetMainCharacter.manaSystem.IsFullMana();
    }

    protected override void OnUseCard()
    {
        if (lastCardRequirement.CheckRequirement())
            Main.instance.GetMainCharacter.manaSystem.FillFullMana();
        else
            Main.instance.GetMainCharacter.manaSystem.ModifyMana(normalManaAmmount);

        Destroy(this.gameObject);
    }
}
