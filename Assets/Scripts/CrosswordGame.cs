using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class CrosswordGame : MonoBehaviour
{
    [SerializeField] private InputField wordInput;
    [SerializeField] private Text statusText;
    [SerializeField] private Text feedbackText;

    private string[] allWords = new string[] { "HATA", "ATA", "KAT", "AKİK", "HAKİKAT", "TAKTİK" };
    private HashSet<string> foundWords = new HashSet<string>();

    private void Start()
    {
        wordInput.onEndEdit.AddListener(OnWordSubmitted);
        UpdateStatus();
    }

    private void OnWordSubmitted(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return;

        string userInput = input.ToUpper().Trim();
        bool found = false;

        // Kelimeyi listede ara (büyük/küçük harf önemli değil)
        foreach (string word in allWords)
        {
            if (word.ToUpper() == userInput)
            {
                if (foundWords.Contains(word))
                {
                    feedbackText.text = "⚠ Bu kelimeyi zaten buldunuz!";
                    feedbackText.color = new Color(1, 1, 0);
                }
                else
                {
                    foundWords.Add(word);
                    feedbackText.text = "✓ Doğru! \"" + word + "\"";
                    feedbackText.color = Color.green;

                    if (foundWords.Count == allWords.Length)
                    {
                        Invoke("GoToEnding", 1.5f);
                    }
                }
                found = true;
                break;
            }
        }

        if (!found)
        {
            feedbackText.text = "✗ Böyle kelime yok!";
            feedbackText.color = Color.red;
        }

        wordInput.text = "";
        UpdateStatus();
        wordInput.ActivateInputField();
    }

    private void UpdateStatus()
    {
        string boxes = "";
        for (int i = 0; i < allWords.Length; i++)
        {
            boxes += (i < foundWords.Count) ? "☑ " : "☐ ";
        }
        statusText.text = boxes + "\n\nKelime girin:";
    }

    private void GoToEnding()
    {
        Debug.Log("Tüm kelimeler bulundu!");
        SceneManager.LoadScene("PrologueEnding");
    }
}