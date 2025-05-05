using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI bileşenlerini kullanmak için gerekli

public class FadeToBlackController : MonoBehaviour
{
    public Image fadeImage; // Siyah Image referansı (FadePanel)
    public GameObject PopUpMasum; // FadePanel referansı (UI paneli)
    public GameObject PopUpSuclu; // FadePanel referansı (UI paneli)
    public GameObject OlayImg;
    
    public TypewriterEffect Scenario; // FadePanel referansı (UI paneli)
    public float fadeDuration = 1.5f;
    public ChoiceManager choiceManager; 
    public DeckPicker deckPicker; // Kart destesini yöneten referans
    public ClearDropZoneSlots clearDropZoneSlots; // Drop zone slotlarını temizleyen referans
    public ButtonClickHandler buttonClickHandler; // ButtonClickHandler referansı
    public List<GameObject> OLs; // Pop-up referansları
    public void StartFadeOut()
    {
        Debug.Log("Fade Out başlatıldı!");
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        // Panel aktif değilse aktif et
        if (!fadeImage.gameObject.activeSelf)
            fadeImage.gameObject.SetActive(true);

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, 1f);
        Debug.Log("Fade Out tamamlandı!");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        // ✳️ Biraz bekle (örneğin 1 saniye sonra)
        yield return new WaitForSeconds(1f);

        // Paneli devre dışı bırak
        fadeImage.gameObject.SetActive(false);
        PopUpMasum.SetActive(false);
        PopUpSuclu.SetActive(false);
        OlayImg.SetActive(false);
        Scenario.scenarioIndex += 1;
        
        int currentIndex = deckPicker.selectedDeckIndex;
        int nextIndex = currentIndex + 1;

        if (currentIndex >= 0 && currentIndex < OLs.Count)
        {
            OLs[currentIndex].SetActive(false);
        }

        if (nextIndex >= 0 && nextIndex < OLs.Count)
        {
            OLs[nextIndex].SetActive(true);

                    // TypewriterEffect varsa indexOverride'ı güncelle
            TypewriterEffect tw = OLs[nextIndex].GetComponentInChildren<TypewriterEffect>();
            if (tw != null)
            {
                tw.indexOverride = Scenario.scenarioIndex;
            }
        }
        else
        {
            Debug.LogWarning("OLs listesinde geçerli bir sonraki index yok.");
        }

        // Indexi güncelle
        deckPicker.selectedDeckIndex++;

        // Eğer oyun bitti şartı varsa burada kontrol edilmeli
        if (deckPicker.selectedDeckIndex == 4)
        {
            Debug.Log("Oyun Bitti!");
        }
        deckPicker.ClearDeck();
        deckPicker.CreateDeck(); // Senaryo dizinini sıfırla
        clearDropZoneSlots.ClearSlots(); // Drop zone slotlarını temizle
        // Kartları temizle
        if (choiceManager != null )
        {   
            choiceManager.onPopUpClick(); // Kamerayı geri açan fonksiyon
        }
        else
        {
            Debug.LogError("ChoiceManager referansı atanmadı!");
        }
        buttonClickHandler.hasInteracted = false; // Buton tıklama durumunu sıfırla
        Debug.Log("FadePanel devre dışı bırakıldı!");
    }
}