using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerEnterHandler
{
    Canvas canvas;
    RectTransform rectTransform;
    [SerializeField] float permissiveLimit = 10;

    Vector3 currentPos;
    DeckOfCards currentDeck;
    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void Initialize(Vector3 position, DeckOfCards deck)
    {
        currentPos = position;
        currentDeck = deck;
    }

    #region Events

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!CheckPosition())
            rectTransform.position = currentPos;
        else
            Debug.Log("No hay problema con esta posición");
}

    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    bool visible;
    private void OnBecameInvisible()
    {
        visible = false;
    }

    private void OnBecameVisible()
    {
        visible = true;
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
