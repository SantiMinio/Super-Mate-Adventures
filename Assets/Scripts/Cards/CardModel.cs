using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardModel : MonoBehaviour
{

    public abstract bool CanUse();

    public abstract void UseCard();

    public abstract void RangeFeedback();

    public abstract void ResetCard();
}
