using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewCardDeck", menuName = "Cards/Card Deck")]
public class CardDeck : ScriptableObject
{
    public string deckName;
    public List<GameObject> cards; // 7 kartlık prefab listesi
}