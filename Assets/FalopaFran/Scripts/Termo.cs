using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Termo : MonoBehaviour
{
    [SerializeField] private LayerMask targets;
    
    private void OnMouseDown()
    {
        Main.instance.EventManager.TriggerEvent(GameEvent.TermoClicked);
        
        var sphereCheck = Physics.CheckSphere(transform.position, 15f, targets);

        if (sphereCheck)
        {
            Main.instance.GetMainCharacter.manaSystem.FillFullMana();
            Main.instance.GetMainCharacter.GetLifeHandler.ResetLife();
        }
    }
}
