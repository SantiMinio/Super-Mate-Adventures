using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardSetting", menuName = "Card/Settings", order = 0)]
public class CardSettings : ScriptableObject
{
    public Sprite img;
    public string title;
    public string desc;
    public List<CardRequirement> requirement = new List<CardRequirement>();
}

public enum CardRequirement { Mana, Life, UsedCard }