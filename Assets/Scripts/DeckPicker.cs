using System.Collections.Generic;
using UnityEngine;
public class DeckPicker : MonoBehaviour
{
    public List<CardDeck> cardDecks;
    public int selectedDeckIndex = 0;

    public void CreateDeck()
    {
        if (selectedDeckIndex >= cardDecks.Count)
        {
            Debug.Log("Deck index out of range!");
            return;
        }

        CardDeck selectedDeck = cardDecks[selectedDeckIndex];
        Debug.Log("Seçili kart destesi: " + selectedDeck.deckName);

        List<Card> instantiatedCards = new List<Card>();

        foreach (GameObject cardPrefab in selectedDeck.cards)
        {
            GameObject cardInstance = Instantiate(cardPrefab, transform);
            cardInstance.name = cardPrefab.name; // (Clone) ekini temizler
            cardInstance.transform.localPosition = Vector3.zero;

            Card card = cardInstance.GetComponent<Card>();
            if (card != null)
                instantiatedCards.Add(card);
        }

        if (instantiatedCards.Count > 0)
            instantiatedCards[0].SendMessage("UpdateFanLayout");
    }

    public void ClearDeck()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
