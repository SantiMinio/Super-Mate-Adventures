using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelector : MonoBehaviour
{
    [SerializeField] CardProbability_Dictionary cards = new CardProbability_Dictionary();
    [SerializeField] int maxProbability = 200;
    [SerializeField] int probabilityPerWave = 10;


}
