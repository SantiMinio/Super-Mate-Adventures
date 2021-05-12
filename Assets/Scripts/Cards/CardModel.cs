using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardModel : MonoBehaviour
{
    protected List<Requirement> myRequirements = new List<Requirement>();


    public bool CanUse()
    {
        for (int i = 0; i < myRequirements.Count; i++)
        {
            if (!myRequirements[i].CheckRequirement())
                return false;
        }

        return OnCanUse();
    }

    protected abstract bool OnCanUse();

    public void UseCard()
    {
        for (int i = 0; i < myRequirements.Count; i++)
            myRequirements[i].RequirementEffect();

        OnUseCard();
    }

    protected abstract void OnUseCard();

    public abstract void RangeFeedback();

    public abstract void ResetCard();
}
