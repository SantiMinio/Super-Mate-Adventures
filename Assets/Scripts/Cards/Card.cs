using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class Card : MonoBehaviour, IDragHandler,IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    Canvas canvas;
    RectTransform rectTransform;
    [Header("Basic Things")]
    [SerializeField] Image imgSprite = null;

    [SerializeField] float permissiveLimit = 10;
    [SerializeField] LayerMask mask = 1 << 8;
    [SerializeField] Animator anim = null;

    Vector3 currentPos;
    DeckOfCards currentDeck;
    CardSettings settings;

    CardModel currentModel;
    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void Initialize(Vector3 position, DeckOfCards deck, CardSettings _settings)
    {
        currentPos = position;
        currentDeck = deck;
        settings = _settings;
        imgSprite.sprite = settings.img;
        currentModel = Instantiate(settings.model);
    }

    public void SetPosition(Vector3 pos)
    {
        currentPos = pos;
        rectTransform.position = currentPos;
    }
    Ray lastRay;
    #region Events
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        lastRay = ray;

        RaycastHit hit;
        if(Physics.Raycast(ray.origin, ray.direction, out hit, 8000000, mask, QueryTriggerInteraction.Ignore))
        {
            currentModel.transform.position = hit.point;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!CheckPosition() || !currentModel.CanUse())
        {
            rectTransform.position = currentPos;
            currentModel.ResetCard();
        }
        else
        {
            currentModel.UseCard();
            currentDeck.OnUseCard(this, settings);
        }
    }
    bool onCard;

    public void OnPointerEnter(PointerEventData eventData)
    {
        onCard = true;
        anim.Play("ScaleOnEnter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (onCard)
        {
            onCard = false;
            anim.Play("ScaleOnExit");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //onCard = false;
        //anim.Play("Idle");
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
        if (rectTransform.position.x < 0 - permissiveLimit || rectTransform.position.x > canvas.worldCamera.pixelWidth + permissiveLimit
            || rectTransform.position.y < 0 - permissiveLimit || rectTransform.position.y > canvas.worldCamera.pixelHeight + permissiveLimit)
            return false;

        if (rectTransform.position.y < currentPos.y + 80 && rectTransform.position.x > currentDeck.lastPos.position.x - 80
            && rectTransform.position.x < currentDeck.firstPos.position.x + 80)
            return false;

        return true;
    }

    #endregion
}
