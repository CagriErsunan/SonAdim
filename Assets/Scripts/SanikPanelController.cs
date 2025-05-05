using UnityEngine;
using UnityEngine.UI;


public class SanikPanelController : MonoBehaviour
{
    public GameObject canvas;
    public Transform cameraTransform; // Main Camera buraya atanacak


    public void GizleCanvas() {
        canvas.SetActive(false);
        cameraTransform.GetComponent<HeadOnlyLook>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    
    
}
