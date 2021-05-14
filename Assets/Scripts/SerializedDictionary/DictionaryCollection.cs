using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RequirementString_SpriteDictionary : SerializableDictionary<string, Sprite> { }

[Serializable]
public class CardProbability_Dictionary : SerializableDictionary<CardSettings, int> { }
