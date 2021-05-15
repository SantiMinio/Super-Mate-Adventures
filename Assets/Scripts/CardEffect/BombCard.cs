using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Frano;

public class BombCard : CardModel, IAttacker
{
    [SerializeField] GameObject feedback = null;
    [SerializeField] float range = 8;
    [SerializeField] float damage = 10;
    [SerializeField] ManaRequirement manaRequirement = new ManaRequirement();
    [SerializeField] ParticleSystem bombParticle = null;

    private void Awake()
    {
        feedback.SetActive(false);
        myRequirements.Add(manaRequirement);
    }

    public override Requirement GetRequire() => manaRequirement;

    protected override bool OnCanUse()
    {
        return true;
    }

    public override void RangeFeedback()
    {
        feedback.SetActive(true);
        feedback.transform.position = transform.position;
        feedback.transform.localScale = Vector3.one * range;
    }

    protected override void OnUseCard()
    {
        feedback.SetActive(false);
        var overlap = Physics.OverlapSphere(transform.position, range);
        bombParticle.transform.position = transform.position;
        bombParticle.Play();

        foreach (var item in overlap)
        {
            IHiteable hiteable = item.GetComponent<IHiteable>();
            if (hiteable != null && !item.GetComponent<CharacterHead>())
                hiteable.Hit(this);
        }

        StartCoroutine(BombParticleDissappear());
    }

    IEnumerator BombParticleDissappear()
    {
        yield return new WaitWhile(() => bombParticle.isPlaying);
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
