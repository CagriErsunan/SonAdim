using System.Collections;
using UnityEngine;
using UnityEngine.UI; // UI bileşenlerini kullanmak için gerekli

public class FadeToBlackController : MonoBehaviour
{
    public Image fadeImage; // Siyah Image referansı (FadePanel)
    public GameObject PopUpMasum; // FadePanel referansı (UI paneli)
    public GameObject PopUpSuclu; // FadePanel referansı (UI paneli)
    public GameObject OlayImg; // FadePanel referansı (UI paneli)
    public float fadeDuration = 1.5f;
    public ChoiceManager choiceManager; 

    public ButtonClickHandler buttonClickHandler; // ButtonClickHandler referansı

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