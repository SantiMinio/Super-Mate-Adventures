using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatalityCard : CardModel, IAttacker
{
    float damage = 10000000;
    [SerializeField] CardOnHandRequirement cardOnHandRequirement = new CardOnHandRequirement();
    //[SerializeField] ManaRequirement manaRequirement = new ManaRequirement();

    private void Awake()
    {
        myRequirements.Add(cardOnHandRequirement);
    }

    public override Requirement GetRequire() => cardOnHandRequirement;

    protected override bool OnCanUse()
    {
        return true;
    }

    public override void RangeFeedback()
    {
    }

    protected override void OnUseCard()
    {
        var overlap = Physics.OverlapSphere(Main.instance.GetMainCharacter.transform.position, 30);

        foreach (var item in overlap)
        {
            IHiteable hiteable = item.GetComponent<EnemyDummy>();
            if (hiteable != null)
                hiteable.Hit(this);
        }

        Destroy(this.gameObject);
    }

    public override void ResetCard()
    {
    }

    public Vector3 GetPosition()
    {
        return Main.instance.GetMainCharacter.transform.position;
    }

    public float GetDamage()
    {
        return damage;
    }
}
