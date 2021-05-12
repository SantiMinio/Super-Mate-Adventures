using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Termo : MonoBehaviour
{
    [SerializeField] private LayerMask targets;
    
    private void OnMouseDown()
    {
        var sphereCheck = Physics.CheckSphere(transform.position, 20f, targets);

        if (sphereCheck)
        {
            Main.instance.GetMainCharacter.manaSystem.FillFullMana();
        }
    }
}
