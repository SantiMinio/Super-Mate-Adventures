using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeTemperatureCard : CardModel
{
    [SerializeField] ManaRequirement manaRequirement = new ManaRequirement();
    //[SerializeField] LastCardRequirement lastCardRequirement = new LastCardRequirement();
    [SerializeField] int healAmmount = 15;
    [SerializeField] ParticleSystem pt;
    private void Awake()
    {
        myRequirements.Add(manaRequirement);
    }

    public override Requirement GetRequire() => manaRequirement;

    public override void RangeFeedback()
    {
    }

    public override void ResetCard()
    {

    }

    protected override bool OnCanUse()
    {
        
        if (Main.instance.GetMainCharacter.GetComponent<LifeHandler>().IsFullLife())
        {
            UIManager.instance.DisplayDialog("Tenés la vida llena!!");
            return false;
        }
        else return true;
    }

    protected override void OnUseCard()
    {
        pt.Play();
        pt.transform.parent = null;
        Main.instance.GetMainCharacter.GetComponent<LifeHandler>().Heal(healAmmount);

        Destroy(this.gameObject);
    }
}
