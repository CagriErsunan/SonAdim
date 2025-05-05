using UnityEngine;
using UnityEngine.UI;

public class SlotButton : MonoBehaviour
{
    public Transform slotArea; // Bu butonun temsil ettiği slot

    public void OnSlotClicked()
    {
        if (Card.selectedCard == null)
            return;

        // Sadece kart (örneğin "Card" tag'li obje) var mı kontrol et
        bool hasCard = false;
        foreach (Transform child in slotArea)
        {
            if (child.CompareTag("Card"))
            {
                hasCard = true;
                break;
            }
        }

        if (hasCard)
        {
            Debug.Log("Bu slot zaten dolu.");
            return;
        }

        // Kartı slota yerleştir
        Card.selectedCard.GetComponent<Card>().PlaceToSlot(slotArea);
    }
}