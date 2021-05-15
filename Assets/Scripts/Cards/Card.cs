using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using TMPro;

public class Card : MonoBehaviour, IDragHandler,IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    Canvas canvas;
    RectTransform rectTransform;
    [Header("Basic Things")]
    [SerializeField] Image imgSprite = null;

    [SerializeField] float permissiveLimit = 10;
    [SerializeField] LayerMask mask = 1 << 8;
    [SerializeField] Animator anim = null;
    [SerializeField, Range(0,1f)] float moveSpeed = 0.1f;

    [SerializeField] GameObject debugDesc = null;
    [SerializeField] TextMeshProUGUI titleTxt = null;
    [SerializeField] TextMeshProUGUI descTxt = null;

    [SerializeField] TextMeshProUGUI cardTitle = null;
    [SerializeField] TextMeshProUGUI requireAmmountTxt = null;
    [SerializeField] Image requireImg = null;
    [SerializeField] RequirementString_SpriteDictionary requireSprites = new RequirementString_SpriteDictionary();

    [Header("Sounds")]
    [SerializeField] AudioClip initSound = null;
    [SerializeField] AudioClip pickSound = null;
    [SerializeField] AudioClip useSound = null;
    [SerializeField] AudioClip errorSound = null;

    Vector3 currentPos;
    DeckOfCards currentDeck;
    [HideInInspector] public CardSettings settings;

    CardModel currentModel;

    bool moving;

    public Action stopMoveCallback;
    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
        rectTransform = GetComponent<RectTransform>();

        if (initSound != null) AudioManager.instance.GetSoundPool(initSound.name, AudioManager.SoundDimesion.TwoD, initSound);
        if (pickSound != null) AudioManager.instance.GetSoundPool(pickSound.name, AudioManager.SoundDimesion.TwoD, pickSound);
        if (useSound != null) AudioManager.instance.GetSoundPool(useSound.name, AudioManager.SoundDimesion.TwoD, useSound);
        if (errorSound != null) AudioManager.instance.GetSoundPool(errorSound.name, AudioManager.SoundDimesion.TwoD, errorSound);
    }

    private void Start()
    {
        Main.instance.EventManager.SubscribeToEvent(GameEvent.MateDead, EndGame);
    }

    void EndGame()
    {
        GetComponent<Image>().raycastTarget = false;
    }

    private void Update()
    {
        if (moving)
        {
            moveTimer += Time.deltaTime * moveSpeed;
            rectTransform.position = Vector3.Lerp(rectTransform.position, currentPos, moveTimer);

            if(Vector3.Distance(rectTransform.position, currentPos) <= 1)
            {
                moveTimer = 0;
                rectTransform.position = currentPos;
                moving = false;
                GetComponent<Image>().raycastTarget = true;
                stopMoveCallback?.Invoke();
            }
        }
    }
    float moveTimer;

    public void Initialize(DeckOfCards deck, CardSettings _settings)
    {
        currentDeck = deck;
        settings = _settings;
        imgSprite.sprite = settings.img;
        currentModel = Instantiate(settings.model);
        debugDesc.gameObject.SetActive(true);
        titleTxt.text = settings.title;
        descTxt.text = settings.desc;
        debugDesc.gameObject.SetActive(false);
        cardTitle.text = settings.title;

        var cardRequire = currentModel.GetRequire();

        if (cardRequire == null)
        {
            requireImg.gameObject.SetActive(false);
        }
        else if (requireSprites.ContainsKey(cardRequire.GetType().FullName))
        {
            requireImg.gameObject.SetActive(true);
            requireAmmountTxt.text = cardRequire.GetRequirment().ToString();
            requireImg.sprite = requireSprites[cardRequire.GetType().FullName];
        }
        if (initSound != null) 
        {
            AudioManager.instance.PlaySound(initSound.name);
        }
    }

    Vector3 startPos;
    public void GoToPos(Vector3 position, Action Callback = null)
    {
        GetComponent<Image>().raycastTarget = false;
        stopMoveCallback = Callback;
        currentPos = position;
        startPos = rectTransform.position;
        moving = true;
    }

    public void SetPosition(Vector3 pos)
    {
        currentPos = pos;
        rectTransform.position = currentPos;
    }
    #region Events
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        Plane plane = new Plane(Vector3.up, -0.199999f);

        float distance;
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        if (plane.Raycast(ray, out distance))
        {
            currentModel.transform.position = ray.GetPoint(distance);
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 position = Camera.main.ScreenToViewportPoint(eventData.position) * 100;

        if (rectTransform.position.y < currentDeck.recycleBin.position.y + currentDeck.recycleBin.rect.height / 2
            && rectTransform.position.y > currentDeck.recycleBin.position.y - currentDeck.recycleBin.rect.height / 2
            && rectTransform.position.x > currentDeck.recycleBin.position.x - currentDeck.recycleBin.rect.width / 2
            && rectTransform.position.x < currentDeck.recycleBin.position.x + currentDeck.recycleBin.rect.width / 2)
        {
            currentModel.ResetCard();
            currentModel.Discard();
            currentDeck.DiscardCard(this, settings);

            return;
        }

        if (!CheckPosition() || !currentModel.CanUse())
        {
            currentModel.ResetCard();
            GoToPos(currentPos);
            if (errorSound != null) AudioManager.instance.PlaySound(errorSound.name);
        }
        else
        {
            if (useSound != null) AudioManager.instance.PlaySound(useSound.name);
            currentModel.UseCard();
            currentDeck.OnUseCard(this, settings);
        }
    }
    bool onCard;

    public void OnPointerEnter(PointerEventData eventData)
    {
        onCard = true;
        anim.Play("ScaleOnEnter");
        debugDesc.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (onCard)
        {
            onCard = false;
            anim.Play("ScaleOnExit");
            debugDesc.gameObject.SetActive(false);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (pickSound != null) AudioManager.instance.PlaySound(pickSound.name);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        currentModel.RangeFeedback();
        rectTransform.SetAsLastSibling();
    }
    #endregion

    #region CheckPosition

    bool CheckPosition()
    {
        if (rectTransform.position.x < 0 - permissiveLimit || rectTransform.position.x > Camera.main.pixelWidth + permissiveLimit
            || rectTransform.position.y < 0 - permissiveLimit || rectTransform.position.y > Camera.main.pixelHeight + permissiveLimit)
            return false;

        if (rectTransform.position.y < currentPos.y + 80 && rectTransform.position.x > currentDeck.lastPos.position.x - 80
            && rectTransform.position.x < currentDeck.firstPos.position.x + 80)
            return false;

        return true;
    }

    #endregion
}
