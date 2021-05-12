using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeTemperatureCard : CardModel
{
    [SerializeField] ManaRequirement manaRequirement = new ManaRequirement();
    //[SerializeField] LastCardRequirement lastCardRequirement = new LastCardRequirement();
    [SerializeField] int healAmmount = 15;

    private void Awake()
    {
        myRequirements.Add(manaRequirement);
    }

    public override void RangeFeedback()
    {
    }

    public override void ResetCard()
    {

    }

    protected override bool OnCanUse()
    {
        return !Main.instance.GetMainCharacter.GetComponent<LifeHandler>().IsFullLife();
    }

    protected override void OnUseCard()
    {
        Main.instance.GetMainCharacter.GetComponent<LifeHandler>().Heal(healAmmount);

        Destroy(this.gameObject);
    }
}
