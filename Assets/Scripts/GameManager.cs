using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<string> correctOrder = new List<string> { "Alibi", "Delil1", "Tanık", "Zaman", "Konum" };
    public Transform[] slots; // DropZone'daki 5 slot

    void Awake()
    {
        Instance = this;
    }

    public void CheckOrder()
    {
        List<string> currentOrder = new List<string>();

        foreach (Transform slot in slots)
        {
            if (slot.childCount == 0)
            {
                Debug.Log("Tüm slotlar dolu değil!");
                return;
            }

            // Slot içindeki kartı al
            Card card = slot.GetComponentInChildren<Card>();
        
            if (card != null)
            {
                currentOrder.Add(card.cardId);  // Kartın cardId'sini ekle
                Debug.Log($"Kart {card.cardId} {slot.name} slotuna yerleştirildi.");
            }
            else
            {
                Debug.LogWarning($"{slot.name} slotunda kart bulunamadı!");
            }
        }

        Debug.Log("Mevcut Sıra: " + string.Join(", ", currentOrder));
        Debug.Log("Doğru Sıra: " + string.Join(", ", correctOrder));

        bool isCorrect = true;
        for (int i = 0; i < correctOrder.Count; i++)
        {
            if (currentOrder[i] != correctOrder[i])
            {
                isCorrect = false;
                Debug.Log($"Yanlış sıralama! Hata: {currentOrder[i]} yerine {correctOrder[i]} olmalıydı.");
                break;
            }
        }

        if (isCorrect)
        {
            Debug.Log("Kişi SUÇSUZ! 👼");
        }
        else
        {
            Debug.Log("Kişi SUÇLU! ⚖️");
        }
    }
}