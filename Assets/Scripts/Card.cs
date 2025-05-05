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
    private Vector2 originalSize;

    private bool isHovered = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalParent = transform.parent;
        originalSize = rectTransform.sizeDelta; // Orijinal boyutu kaydet

        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0f); // Alt orta pivot
    }

    IEnumerator Start()
    {
        yield return null; // Tüm kartlar sahneye yerleşene kadar 1 frame bekle
       // UpdateFanLayout(); // Doğru fan düzenlemesini uygula
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

        // Önce kartı slot'un child'ı olarak ayarla
        rectTransform.SetParent(slot);
    
        // Kartı slot'un merkezine yerleştir
        rectTransform.localPosition = Vector3.zero;
        rectTransform.localRotation = Quaternion.identity;
    
        // Kartın boyutunu slot'un boyutuyla aynı yap
        RectTransform slotRectTransform = slot.GetComponent<RectTransform>();
    
        // Kartın anchors ve sizeDelta değerlerini ayarla
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    
        // Scale'i sıfırla, artık stretch kullanacağız
        rectTransform.localScale = Vector3.one;

        isPlaced = true;
        selectedCard = null;

        UpdateFanLayout(); // Yeni yerleşim düzenini uygula
    }

    public void ReturnToDeck()
    {
        if (!isPlaced) return;

        rectTransform.SetParent(originalParent);
    
        // Fan düzenine uygun ayarları geri yükle
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0f); // Alt orta pivot - fan düzeni için
    
        // Kartın orijinal boyutlarını geri yükle
        rectTransform.sizeDelta = new Vector2(100f, 150f); // Kartın orijinal boyutlarını buraya yazın
        rectTransform.localScale = Vector3.one;
    
        isPlaced = false;
        selectedCard = null;
        rectTransform.sizeDelta = originalSize; // Orijinal boyutu geri yükle

        UpdateFanLayout(); // Yeni yerleşim düzenini uygula
    }

    private void DeselectCard()
    {
        // Kartı seçilmemiş hale getir (yukarı çıkarmayı geri al)
        UpdateFanLayout();
    }

    public void UpdateFanLayout()
    {
        List<RectTransform> siblings = new List<RectTransform>();

        foreach (Transform child in originalParent)
        {
            Card card = child.GetComponent<Card>();
            if (card != null && !card.isPlaced)
                siblings.Add(child.GetComponent<RectTransform>());
        }

        float fanAngle = 28f;
        float radius = 500f;
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
