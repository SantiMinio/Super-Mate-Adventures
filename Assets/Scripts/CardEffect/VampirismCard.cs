using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampirismCard : CardModel, IAttacker
{
    [SerializeField] GameObject feedback = null;
    [SerializeField] float range = 8;
    [SerializeField] float damage = 4;
    [SerializeField] float healPerEnemy = 1.5f;
    [SerializeField] ManaRequirement manaRequirement = new ManaRequirement();
    [SerializeField] DistanceRequirement distanceRequirement = new DistanceRequirement();
    [SerializeField] ParticleSystem chupada;
    [SerializeField] AudioClip sound = null;
    bool onFeedback;

    private void Awake()
    {
        feedback.SetActive(false);
        myRequirements.Add(manaRequirement);
        myRequirements.Add(distanceRequirement);
        AudioManager.instance.GetSoundPool(sound.name, AudioManager.SoundDimesion.ThreeD, sound);
    }
    public override Requirement GetRequire() => manaRequirement;

    public override void RangeFeedback()
    {
        onFeedback = true;
    }

    public override void ResetCard()
    {
        onFeedback = false;
        feedback.gameObject.SetActive(false);
    }

    protected override bool OnCanUse()
    {
        return true;
    }

    protected override void OnUseCard()
    {
        AudioManager.instance.PlaySound(sound.name);
        onFeedback = false;
        feedback.SetActive(false);

        var overlap = Physics.OverlapSphere(transform.position, range);
        chupada.Play();
        chupada.transform.parent = null;
        foreach (var item in overlap)
        {
            IHiteable hiteable = item.GetComponent<IHiteable>();
            if (hiteable != null && !item.GetComponent<Frano.CharacterHead>())
            {
                hiteable.Hit(this);
                Main.instance.GetMainCharacter.GetComponent<LifeHandler>().Heal(healPerEnemy);
            }
        }

        Destroy(this.gameObject);

    }

    private void Update()
    {
        if (onFeedback && distanceRequirement.CheckRequirement())
        {
            transform.position = Main.instance.GetMainCharacter.transform.position;
            feedback.SetActive(true);
        }
        else if (feedback.activeSelf)
        {
            feedback.SetActive(false);
        }
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
