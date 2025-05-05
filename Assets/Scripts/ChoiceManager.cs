using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.UI;

public class ChoiceManager : MonoBehaviour

{

    public GameObject sanik;
    [Header("Case Setup")]
    // O an oynanan davanın CaseData asset'ini buraya sürükleyin.
    public String feedbackMessage; // Karar sonucunu göstermek için kullanılacak mesaj
    public GameObject[] slotAreas = new GameObject[6]; // Slot alanlarını tutacak dizi (örneğin, 6 slot alanı)
    public List<Scenarios> Scenarios; // Tüm senaryoları tutacak dizi (örneğin, 6 senaryo)
    private int currentScenarioIndex; // Tüm senaryoları tutacak dizi (örneğin, 6 senaryo)
    private List<string> ikinciCocukIsimleri = new List<string>(); // İkinci çocuk isimlerini saklamak için bir alan
    
    
    public GameObject guiltyPopup;
    public GameObject innocentPopup;// Pop-up içindeki yazı alanı (Eski UI Text için)
    public GameObject resultPopup; // Pop-up içindeki yazı alanı (Eski UI Text için) 
    [Header("Camera Settings")]
    public Camera playerCamera; // FPS oyuncu kamerası
    public Vector3 defaultCameraPosition = new Vector3(0, 4.823f, -20.18f); // Varsayılan kamera pozisyonu
    public Vector3 defaultCameraRotation = new Vector3(0, 0, 0); // Varsayılan kamera rotasyonu
    
    // FPS kontrol scripti referansı (varsa)
    [SerializeField] private MonoBehaviour playerController;
    private bool cameraPreviouslyLocked = false;


    private void Update()
    {
        int doluSlotSayisi = 0;

        foreach (GameObject slot in slotAreas)
        {
            if (slot.transform.childCount >= 2)
            {
                doluSlotSayisi++;
            }
        }

        if (doluSlotSayisi == 6)
        {
            sanik.SetActive(true);
        }
        else
        {
            sanik.SetActive(false);
        }
    }
    public void Topla()
    {

        Debug.Log(Scenarios.Count); // Senaryo listesinin uzunluğunu kontrol et

        // 4. slot1 - slot6 arası çocukları gez
        for (int i = 0; i < slotAreas.Length; i++)
        {
            Transform slot = slotAreas[i].transform; // slot1 - slot6
            // 6. slot1 - slot6'in çocuklarını kontrol et

            if (slot.childCount >= 2)
            {
                Transform ikinciCocuk = slot.GetChild(1); // index 1 => ikinci çocuk
                ikinciCocukIsimleri.Add(ikinciCocuk.name);
            }
            else
            {
                Debug.Log($"{slot.name} objesinin en az 2 çocuğu yok.");
            }
        }

        if (ikinciCocukIsimleri.Count == 6)
        {
            CheckPlayerScenario();
        }
        else
        {
            Debug.Log("Yeterli kart dizilimi yok. Toplam: " + ikinciCocukIsimleri.Count);
            ikinciCocukIsimleri.Clear(); // Yeterli kart dizilimi yoksa listeyi temizle
        }
        /*
        // 6. Listeyi yazdır
        Debug.Log("İkinci çocuk isimleri:");
        foreach (string isim in ikinciCocukIsimleri)
        {
         Debug.Log("İSİMLER: " + isim);
        }
        */
    }

    public void onPopUpClick()
    {
        // Pop-up'a tıklandığında yapılacak işlemler
        Debug.Log("Pop-up'a tıklandı!");

        // Pop-up'ı kapat
        HidePopup();
        // Oyuncu kontrollerini geri yükle
        // UnlockCameraAfterPopup();
    }
    public void CheckPlayerScenario()
    {
        if (currentScenarioIndex >= Scenarios.Count)
        {
            Debug.Log("Tüm senaryolar tamamlandı.");
            return;
        }

        var currentScenario = Scenarios[currentScenarioIndex];
        Debug.Log("Senaryo adı: " + currentScenario.caseName);
        List<string> playerSequence = ikinciCocukIsimleri;

        foreach (var item in currentScenario.validScenarios)
        {
            if (playerSequence.SequenceEqual(item.requiredCardIDs))
            {
                LockCameraForPopup();

                Debug.Log("Eşleşen senaryo: " + item.scenarioName);
                Debug.Log("Beklenen sonuç: " + item.supportedVerdict);
                Debug.Log("Gerçek sonuç: " + currentScenario.trueVerdict);

                bool isCorrect = currentScenario.trueVerdict == item.supportedVerdict;

                feedbackMessage = isCorrect ? "DOĞRU KARAR VERDİNİZ!" : "YANLIŞ KARAR VERDİNİZ!";
                Debug.Log(feedbackMessage);

                // Mesaj popup göster
                ShowVerdictPopup(feedbackMessage);

                // Oyuncunun karar verdiği sonucu göster (örneğin: Guilty / Innocent)
                ShowVerdictPopup(item.supportedVerdict.ToString());

                currentScenarioIndex++; // sıradaki senaryoya geç
                ikinciCocukIsimleri.Clear(); // kartları sıfırla
                return;
            }
        }

        // Hiçbir eşleşme bulunamazsa
        feedbackMessage = "Hiçbir eşleşme bulunamadı!";
        Debug.Log(feedbackMessage);
        ikinciCocukIsimleri.Clear();
    }
    private void LockCameraForPopup()
    {
        if (playerCamera == null)
        {
            Debug.LogError("Oyuncu kamerası bulunamadı!");
            return;
        }
        playerCamera.transform.localPosition = defaultCameraPosition;
        playerCamera.transform.localEulerAngles = defaultCameraRotation;
        // Kameranın kilit durumunu kaydet
        cameraPreviouslyLocked = Cursor.lockState == CursorLockMode.Locked;
        
        // Kamera kontrollerini devre dışı bırak
        if (playerController != null && playerController.enabled)
        {
            playerController.enabled = false;
            Debug.Log("Oyuncu kontrolleri devre dışı bırakıldı");
        }
        
        // İmleci göster ve serbest bırak (popup'taki butonlara tıklayabilmek için)
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        // Eğer First Person Controller kullanıyorsanız, hareket scripti de devre dışı bırakılmalı
        var movementScript = playerCamera.GetComponentInParent<MonoBehaviour>();
        if (movementScript != null && movementScript.GetType().Name.Contains("Controller"))
        {
            movementScript.enabled = false;
        }
        
        Debug.Log("Kamera popup için kilitlendi");
    }
    
    // Popup kapatıldığında kamera kontrollerini geri yükleyen fonksiyon
    public void UnlockCameraAfterPopup()
    {
        if (playerCamera == null) return;

        // Kontrolleri tekrar aktif et
        if (playerController != null)
        {
            Debug.Log("Oyuncu kontrolleri tekrar aktif edildi");
            playerController.enabled = true;
        }

        // Cursor eski haline dönsün
        if (cameraPreviouslyLocked)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        Debug.Log("Kamera ve kontroller yeniden aktif");
    }
    void ShowVerdictPopup(string verdict)
    {
        if (resultPopup != null)
        {
            if (verdict == "Guilty") {
                // Suçlu popup'ını göster
                guiltyPopup.SetActive(true);
                innocentPopup.SetActive(false);
            } else if (verdict == "Innocent") {
                // Masum popup'ını göster
                innocentPopup.SetActive(true);
                guiltyPopup.SetActive(false);
            }
            
            // Genel sonuç popup'ını da göster (isteğe bağlı)
            resultPopup.SetActive(true);
            Debug.Log("Sonuç: " + verdict + " popup gösterildi");
        }
        else
        {
            Debug.LogError("Pop-up veya Text referansı atanmamış!");
            Debug.Log("Sonuç: " + feedbackMessage); // En kötü ihtimalle Console'a yazdır
        }
    }

    // Enum'ı kullanıcı dostu string'e çeviren yardımcı fonksiyon
    string VerdictToString(Verdict verdict)
    {
        switch (verdict)
        {
            case Verdict.Guilty: return "Suçlu";
            case Verdict.Innocent: return "Suçsuz";
            default: return "Belirsiz";
        }
    }

    // Pop-up'ı kapatmak için bir fonksiyon (Butona bağlanabilir)
    public void HidePopup()
    {
        if (playerController != null)
        {
            Debug.Log("Oyuncu kontrolleri tekrar aktif edildi");
            playerController.enabled = true;
        }

    }



}