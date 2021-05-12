using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    [SerializeField] Image manaBar = null;
    [SerializeField] Image lifeBar = null;

    [SerializeField] RectTransform bubbleParent = null;
    [SerializeField] Image bubbleModel = null;

    Image[] bubbles = new Image[0];

    private void Awake()
    {
        instance = this;
    }

    public void CreateManaBar(int maxMana)
    {
        bubbles = new Image[maxMana];
        for (int i = 0; i < maxMana; i++)
        {
            var newBubble = Instantiate(bubbleModel, bubbleParent);
            bubbles[i] = newBubble;
        }
    }

    public void ChangeManaBar(int manaAmmount)
    {
        for (int i = 0; i < bubbles.Length; i++)
        {
            if (i < manaAmmount && !bubbles[i].gameObject.activeSelf)
                bubbles[i].gameObject.SetActive(true);
            else if (i >= manaAmmount && bubbles[i].gameObject.activeSelf)
                bubbles[i].gameObject.SetActive(false);
        }
    }

    public void ChangeLifeBar(float percent)
    {
        lifeBar.fillAmount = percent;
    }
}
