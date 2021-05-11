using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    [SerializeField] Image manaBar = null;
    [SerializeField] Image lifeBar = null;

    private void Awake()
    {
        instance = this;
    }

    public void ChangeManaBar(float percent)
    {
        manaBar.fillAmount = percent;
    }

    public void ChangeLifeBar(float percent)
    {
        lifeBar.fillAmount = percent;
    }
}
