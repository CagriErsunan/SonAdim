using UnityEngine;
using UnityEngine.SceneManagement;  // SceneManager için gerekli

public class SceneSwitcher : MonoBehaviour
{
    // Bu fonksiyon butona basıldığında çağrılacak
    public void SwitchScene(string MAHKEME)
    {
        // SceneManager ile sahneyi değiştiriyoruz
        SceneManager.LoadScene(MAHKEME);
    }
}