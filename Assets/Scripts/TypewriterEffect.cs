using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TypewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    
    public List<Scenarios> ScenarioList;
    public float typingSpeed = 0.05f;
    public float delayBeforeStart = 1f; // ⏳ Gecikme süresi
    public int scenarioIndex=0;
    public int indexOverride = -1;

    // Tam metni tutacak liste (örneğin, 5 senaryo)

    private void OnEnable()
    {
        if (indexOverride >= 0)
        {
            scenarioIndex = indexOverride;
        }
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {   
        string fullText;

        if (gameObject.CompareTag("AsilOlay")) // Eğer nesne "AsilOlay" tag'ine sahipse
        {
            Debug.Log("AsilOlay tag'ine sahip nesne bulundu. + scenarioIndex: " + scenarioIndex);
            fullText = ScenarioList[scenarioIndex].whatReallyHappened.ToString();
        }
        else 
        {
            Debug.Log("AsilOlay tag'ine sahip nesne bulunamadı. + scenarioIndex: " + scenarioIndex);
            fullText = ScenarioList[scenarioIndex].caseDescription.ToString();
        }

        Debug.Log(fullText); // Senaryonun metnini al
        textUI.text = "";
        yield return new WaitForSeconds(delayBeforeStart); // ✅ İlk gecikme
        foreach (char letter in fullText)
        {
            textUI.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

    }
}