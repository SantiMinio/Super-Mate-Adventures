using System.Collections;
using System.Collections.Generic;
using Frano;
using UnityEngine;

public class YerbitaItem : GameItem
{

    [SerializeField] private float healAmount;
    public override void Pick()
    {
        base.Pick();
        
        Picker.GetComponent<CharacterHead>().GetLifeHandler.Heal(healAmount); 
    }
}
