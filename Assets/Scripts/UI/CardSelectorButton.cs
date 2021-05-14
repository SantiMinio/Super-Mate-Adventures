using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CardSelectorButton : MonoBehaviour,  IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [Header("Basic Things")]
    [SerializeField] CardSelector selector = null;
    [SerializeField] Image imgSprite = null;

    [SerializeField] Animator anim = null;

    [SerializeField] GameObject debugDesc = null;
    [SerializeField] TextMeshProUGUI titleTxt = null;
    [SerializeField] TextMeshProUGUI descTxt = null;

    [SerializeField] TextMeshProUGUI cardTitle = null;

    [Header("Sounds")]
    [SerializeField] AudioClip pickSound = null;
    [SerializeField] AudioClip useSound = null;

    CardSettings settings;

    private void Start()
    {
        if (pickSound != null) AudioManager.instance.GetSoundPool(pickSound.name, AudioManager.SoundDimesion.TwoD, pickSound);
        if (useSound != null) AudioManager.instance.GetSoundPool(useSound.name, AudioManager.SoundDimesion.TwoD, useSound);
    }

    public void Initialize(CardSettings _settings)
    {
        settings = _settings;
        imgSprite.sprite = settings.img;
        debugDesc.gameObject.SetActive(true);
        titleTxt.text = settings.title;
        descTxt.text = settings.desc;
        debugDesc.gameObject.SetActive(false);
        cardTitle.text = settings.title;
    }

    #region Events

    public void OnPointerEnter(PointerEventData eventData)
    {
        anim.Play("ScaleOnEnter");
        debugDesc.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
            anim.Play("ScaleOnExit");
            debugDesc.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (pickSound != null) AudioManager.instance.PlaySound(pickSound.name);

        selector.SelectCard(settings);
    }
    #endregion
}
