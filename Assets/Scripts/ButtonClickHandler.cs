using UnityEngine;

public class ButtonClickHandler : MonoBehaviour
{
    public bool hasInteracted = false;

    public GameObject canvasToShow;
    public Transform cameraTransform; // Main Camera buraya atanacak

    void Update() 
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Interactable") && !hasInteracted)
                {
                    hasInteracted = true;

                    canvasToShow.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;

                    Vector3 hedefPozisyon = new Vector3(5,0,0);
                    Vector3 direction = Vector3.zero;
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    cameraTransform.rotation = targetRotation;

                    cameraTransform.GetComponent<HeadOnlyLook>().enabled = false;
                }
            }
        }
    }
}