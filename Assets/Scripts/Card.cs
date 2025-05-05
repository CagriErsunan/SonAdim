using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Card : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public string cardId;

    public static GameObject selectedCard;

    private Transform originalParent;
    private bool isPlaced = false;
    private float lastClickTime = 0f;
    private float doubleClickThreshold = 0.3f;
    private RectTransform rectTransform;

    private bool isHovered = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalParent = transform.parent;

        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0f); // Alt orta pivot
    }

    IEnumerator Start()
    {
        yield return null; // Tüm kartlar sahneye yerleşene kadar 1 frame bekle
        UpdateFanLayout(); // Doğru fan düzenlemesini uygula
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        float timeSinceLastClick = Time.time - lastClickTime;
        lastClickTime = Time.time;

        if (timeSinceLastClick < doubleClickThreshold)
        {
            if (isPlaced)
                ReturnToDeck();
            return;
        }

        if (isPlaced) return;

        if (selectedCard != null && selectedCard != gameObject)
            selectedCard.GetComponent<Card>().DeselectCard();

        if (selectedCard == gameObject)
        {
            DeselectCard();
            selectedCard = null;
        }
        else
        {
            selectedCard = gameObject;
            rectTransform.localPosition += new Vector3(0, 20, 0);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isPlaced || selectedCard == gameObject) return;

        rectTransform.localPosition += new Vector3(0, 20, 0); // Kartı yukarı taşı
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isPlaced || selectedCard == gameObject) return;

        if (isHovered)
        {
            UpdateFanLayout(); // Kartları eski düzenine getir
            isHovered = false;
        }
    }

    public void PlaceToSlot(Transform slot)
    {
        if (isPlaced) return;

        rectTransform.SetParent(slot);
        rectTransform.localPosition = Vector3.zero;
        rectTransform.localRotation = Quaternion.identity;
    
        // Kartı slotun boyutuna orantılı olarak ölçeklendir
        RectTransform slotRectTransform = slot.GetComponent<RectTransform>();
        float widthRatio = slotRectTransform.rect.width / rectTransform.rect.width;
        float heightRatio = slotRectTransform.rect.height / rectTransform.rect.height;

        rectTransform.localScale = new Vector3(widthRatio, heightRatio, 1); // Kartın boyutunu slot ile orantılı hale getir

        isPlaced = true;
        selectedCard = null;

        UpdateFanLayout(); // Yeni yerleşim düzenini uygula
    }

    public void ReturnToDeck()
    {
        if (!isPlaced) return;

        // Kartın orijinal boyutunu koruyun
        Vector2 originalSize = rectTransform.sizeDelta;

        rectTransform.SetParent(originalParent);
        isPlaced = false;
        selectedCard = null;

        // Kartın boyutunu sabitle ve scale'ini sıfırla
        rectTransform.sizeDelta = originalSize;
        rectTransform.localScale = Vector3.one; // Kartın boyutunu sabitle

        UpdateFanLayout(); // Yeni yerleşim düzenini uygula
    }

    private void DeselectCard()
    {
        // Kartı seçilmemiş hale getir (yukarı çıkarmayı geri al)
        UpdateFanLayout();
    }

    private void UpdateFanLayout()
    {
        List<RectTransform> siblings = new List<RectTransform>();

        foreach (Transform child in originalParent)
        {
            Card card = child.GetComponent<Card>();
            if (card != null && !card.isPlaced)
                siblings.Add(child.GetComponent<RectTransform>());
        }

        float fanAngle = 30f;
        float radius = 200f;
        int count = siblings.Count;
        float stepAngle = count > 1 ? fanAngle / (count - 1) : 0f;
        float startAngle = -fanAngle / 2f;

        for (int i = 0; i < count; i++)
        {
            float angle = startAngle + i * stepAngle;
            float rad = angle * Mathf.Deg2Rad;

            Vector3 pos = new Vector3(Mathf.Sin(rad) * radius, Mathf.Cos(rad) * radius - radius, 0);
            siblings[i].localPosition = pos;
            siblings[i].localRotation = Quaternion.Euler(0, 0, -angle);
        }
    }
}
