using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip buttonClickSound;  // Buton tıklama sesi
    private AudioSource audioSource;

    void Start()
    {
        // AudioSource bileşenini al
        audioSource = GetComponent<AudioSource>();
        
        if (audioSource == null)
        {
            Debug.LogError("AudioSource bileşeni bulunamadı! Lütfen bir AudioSource ekleyin.");
        }
    }

    // Butona tıklama sesi çalacak fonksiyon
    public void PlayButtonClickSound()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);  // Ses çal
        }
    }
}