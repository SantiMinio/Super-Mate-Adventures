using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Frano;

public class BombCard : CardModel, IAttacker
{
    [SerializeField] GameObject feedback = null;
    [SerializeField] float range = 8;
    [SerializeField] float damage = 10;

    private void Awake()
    {
        feedback.SetActive(false);
    }

    public override bool CanUse()
    {
        return true;
    }

    public override void RangeFeedback()
    {
        feedback.SetActive(true);
        feedback.transform.position = transform.position;
        feedback.transform.localScale = Vector3.one * range;
    }

    public override void UseCard()
    {
        feedback.SetActive(false);
        var overlap = Physics.OverlapSphere(transform.position, range);

        foreach (var item in overlap)
        {
            IHiteable hiteable = item.GetComponent<IHiteable>();
            if (hiteable != null && !item.GetComponent<CharacterHead>())
                hiteable.Hit(this);
        }

        Destroy(this.gameObject);
    }

    public override void ResetCard()
    {
        feedback.SetActive(false);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public float GetDamage()
    {
        return damage;
    }
}
