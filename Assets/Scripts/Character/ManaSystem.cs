using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ManaSystem
{
    [SerializeField] int maxMana = 100;
    int currentMana;

    public void Initialize()
    {
        currentMana = maxMana;
    }

    public void ModifyMana(int manaAmmount)
    {
        currentMana += manaAmmount;
        Debug.Log(currentMana);

        if (currentMana < 0) currentMana = 0;
        else if (currentMana > maxMana) currentMana = maxMana;
        UIManager.instance.ChangeManaBar((float)currentMana / (float)maxMana);
    }

    public bool EnoughMana(int manaAmmount) => currentMana - manaAmmount < 0 ? false : true;
}
