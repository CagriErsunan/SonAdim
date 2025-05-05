using UnityEngine;

public class ClearDropZoneSlots : MonoBehaviour
{
    public GameObject[] dropZoneSlots; // Drop zone slot referansları
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearSlots()
    {
        foreach (GameObject slot in dropZoneSlots)
        {
            // Slot içeriğini temizle
            foreach (Transform child in slot.transform)
            {
                Destroy(slot.transform.GetChild(1).gameObject);
            }
        }
    }    
}
