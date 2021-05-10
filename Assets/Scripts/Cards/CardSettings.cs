using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardSetting", menuName = "Card/Settings", order = 0)]
public class CardSettings : ScriptableObject
{
    public Sprite img;
    public string title;
    public string desc;
    public CardModel model;
}