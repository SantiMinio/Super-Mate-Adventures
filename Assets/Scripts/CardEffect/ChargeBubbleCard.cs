using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeBubbleCard : CardModel
{
    [SerializeField] LastCardRequirement lastCardRequirement = new LastCardRequirement();
    [SerializeField] int normalManaAmmount = 1;
    [SerializeField] AudioClip sound = null;
    [SerializeField] ParticleSystem pt;
    private void Awake()
    {
        AudioManager.instance.GetSoundPool(sound.name, AudioManager.SoundDimesion.ThreeD, sound);
    }

    public override void RangeFeedback()
    {
    }

    public override void ResetCard()
    {
    }

    protected override bool OnCanUse()
    {
        if (Main.instance.GetMainCharacter.manaSystem.IsFullMana())
        {
            UIManager.instance.DisplayDialog("Tenés el mana lleno!!");
            return false;
        }
        else return true;
    }

    protected override void OnUseCard()
    {

        pt.Play();
        pt.transform.parent = null;
        if (lastCardRequirement.CheckRequirement())
            Main.instance.GetMainCharacter.manaSystem.FillFullMana();
        else
            Main.instance.GetMainCharacter.manaSystem.ModifyMana(normalManaAmmount);

        AudioManager.instance.PlaySound(sound.name);
        Destroy(this.gameObject);
    }
}
