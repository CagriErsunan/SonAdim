using UnityEngine;

public class DropZone : MonoBehaviour
{
    public Transform[] slotAreas; // 5 slotluk alanlar

    public void OnDropZoneClicked()
    {
        if (Card.selectedCard == null)
            return;

        foreach (Transform slot in slotAreas)
        {
            if (slot.childCount == 0)
            {
                Card.selectedCard.GetComponent<Card>().PlaceToSlot(slot);
                return;
            }
        }

        Debug.Log("Boş slot kalmadı.");
    }
}