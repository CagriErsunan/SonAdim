using UnityEngine;

public class ButtonClickHandler : MonoBehaviour
{
    public bool hasInteracted = false;

    public GameObject canvasToShow;
    public Transform cameraTransform;

    public AudioClip interactionSound; // Ses dosyasını buraya atayacaksın
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

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

                    // SESİ ÇAL
                    if (interactionSound != null)
                        audioSource.PlayOneShot(interactionSound);

                    // CANVAS'I AÇ
                    canvasToShow.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;

                    // KAMERAYI DÖNDÜR
                    Vector3 hedefPozisyon = new Vector3(5, 0, 0);
                    Vector3 direction = hedefPozisyon - cameraTransform.position;
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    cameraTransform.rotation = targetRotation;

                    // KAFAYI KİLİTLE
                    cameraTransform.GetComponent<HeadOnlyLook>().enabled = false;
                }
            }
        }
    }
}
