using UnityEngine;
using UnityEngine.UI;

public class PopUpClickHandler : MonoBehaviour
{
    public Image OlayImage;
     // Inspector'dan atanacak
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnClick()
    {
        // Pointer ve click aktif olur
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; // Mouse imlecini görünür yap
        OlayImage.gameObject.SetActive(true); // Olay resmini göster
        
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
