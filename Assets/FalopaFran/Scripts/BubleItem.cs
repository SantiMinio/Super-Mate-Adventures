using System.Collections;
using System.Collections.Generic;
using Frano;
using UnityEngine;

public class BubleItem : GameItem
{
    [SerializeField] private int manaAmount;
    public override void Pick()
    {
        base.Pick();
        
        Picker.GetComponent<CharacterHead>().manaSystem.ModifyMana(manaAmount); 
    }
}
