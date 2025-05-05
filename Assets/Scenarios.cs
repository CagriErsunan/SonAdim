using UnityEngine;
using System.Collections.Generic;

public enum Verdict { Undecided, Guilty, Innocent }

[CreateAssetMenu(fileName = "Scenarios", menuName = "Scriptable Objects/Scenarios")]
public class Scenarios : ScriptableObject
{
    
    [Header("Case Details")]
    public string caseName = "Yeni Vaka";
    [TextArea(3, 5)]
    public string caseDescription = "Vaka açıklaması...";

    [Header("Gameplay Data")]
    // Bu dava için doğru olan nihai sonuç
    public Verdict trueVerdict = Verdict.Undecided;

    [Header("Olayın Aslında Ne Oldu?")]    
    [TextArea(3, 5)]
    public string whatReallyHappened = "Aslında ne oldu...";
    // Başarılı sayılacak senaryo(lar).
    // Her bir iç liste, belirli bir sonucu kanıtlayan kartların ID'lerini veya isimlerini
    // doğru sırada içermeli.
    public List<ScenarioOutcome> validScenarios = new List<ScenarioOutcome>();

    // Belki bazı senaryolar oyuncuyu yanlış sonuca götürebilir ama yine de "geçerli" sayılabilir.
    // Bunu daha karmaşık hale getirmek isterseniz ekleyebilirsiniz.
    // public List<ScenarioOutcome> misleadingScenarios = new List<ScenarioOutcome>();
}

// Bir senaryonun hangi sonucu desteklediğini ve hangi kartları içerdiğini tutan yardımcı class
[System.Serializable]
public class ScenarioOutcome
{
    public string scenarioName = "Varsayılan Senaryo Adı"; // Editor'de ayırt etmek için
    public List<string> requiredCardIDs; // Kartların benzersiz kimlikleri (string veya int olabilir)
    public Verdict supportedVerdict; // Bu senaryo hangi sonucu kanıtlıyor?
}