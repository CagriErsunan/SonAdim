using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    public float typingSpeed = 0.05f;
    public float delayBeforeStart = 1f; // ⏳ Gecikme süresi
    [TextArea(3, 10)]
    public string fullText;

    private void OnEnable()
    {
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        textUI.text = "";
        yield return new WaitForSeconds(delayBeforeStart); // ✅ İlk gecikme
        foreach (char letter in fullText)
        {
            textUI.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}