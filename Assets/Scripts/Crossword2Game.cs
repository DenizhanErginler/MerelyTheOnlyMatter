using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Crossword2Game : MonoBehaviour
{
    [SerializeField] private InputField[] inputFields = new InputField[5];
    [SerializeField] private InputField finalInput;
    [SerializeField] private Text feedbackText;

    // TMP destekleyelim (Inspector'da TMP kullanıyorsanız bu alanlara atayın)
    [SerializeField] private TMP_InputField[] inputFieldsTMP = new TMP_InputField[0];
    [SerializeField] private TMP_InputField finalInputTMP;
    [SerializeField] private TextMeshProUGUI feedbackTextTMP;

    private string[] answers = new string[] { "PROPAGANDA", "KAFEİN", "SERZENİŞ", "ROPDÖŞAMBIR", "ŞADIRVAN" };
    private string finalAnswer = "PROFESÖR";
    
    private int correctCount = 0;
    private int fieldCount = 0;
    private bool useTMPInputs = false;

    private void Start()
    {
        if (feedbackText == null)
        {
            Debug.LogError("Feedback Text referansı boş!");
        }

        // Hangi tür input alanlarının kullanıldığını tespit et
        int uiCount = Mathf.Min(inputFields.Length, answers.Length);
        int tmpCount = Mathf.Min(inputFieldsTMP.Length, answers.Length);

        if (uiCount > 0)
        {
            useTMPInputs = false;
            fieldCount = uiCount;
        }
        else if (tmpCount > 0)
        {
            useTMPInputs = true;
            fieldCount = tmpCount;
        }
        else
        {
            fieldCount = 0;
        }

        if (fieldCount == 0)
        {
            Debug.LogWarning("Hiçbir input alanı bulunamadı. `inputFields` veya `inputFieldsTMP` alanlarını Inspector'da atayın.");
        }

        // İlk input'lar için listener ekle (UI veya TMP'ye göre)
        if (!useTMPInputs)
        {
            for (int i = 0; i < fieldCount; i++)
            {
                if (inputFields[i] != null)
                {
                    int index = i;
                    inputFields[i].onEndEdit.AddListener((text) => CheckInput(index, text));
                }
            }
        }
        else
        {
            for (int i = 0; i < fieldCount; i++)
            {
                if (inputFieldsTMP[i] != null)
                {
                    int index = i;
                    inputFieldsTMP[i].onEndEdit.AddListener((text) => CheckInput(index, text));
                }
            }
        }

        // Başlangıçta final input kilitli olsun (oyuncunun önce tüm kelimeleri çözmesi için)
        if (!useTMPInputs)
        {
            if (finalInput != null)
            {
                finalInput.interactable = false;
                finalInput.onEndEdit.AddListener(CheckFinalInput);
            }
        }
        else
        {
            if (finalInputTMP != null)
            {
                finalInputTMP.interactable = false;
                finalInputTMP.onEndEdit.AddListener(CheckFinalInput);
            }
        }
    }

    private void CheckInput(int fieldIndex, string userText)
    {
        if (fieldIndex < 0 || fieldIndex >= answers.Length)
        {
            Debug.LogError($"CheckInput: geçersiz fieldIndex {fieldIndex}");
            return;
        }

        // Hangi input tipini kullanıyoruz ona göre null/interactive kontrolü
        if (!useTMPInputs)
        {
            if (inputFields[fieldIndex] == null)
            {
                Debug.LogError($"CheckInput: inputFields[{fieldIndex}] referansı null");
                return;
            }
            if (!inputFields[fieldIndex].interactable) return;
        }
        else
        {
            if (inputFieldsTMP[fieldIndex] == null)
            {
                Debug.LogError($"CheckInput: inputFieldsTMP[{fieldIndex}] referansı null");
                return;
            }
            if (!inputFieldsTMP[fieldIndex].interactable) return;
        }

        // Boş input kontrolü
        if (string.IsNullOrWhiteSpace(userText))
        {
            if (feedbackText != null) feedbackText.text = "";
            return;
        }

        // Girilen metni temizle
        string userInput = userText.Trim().ToUpper();
        string correctAnswer = answers[fieldIndex].ToUpper();

        Debug.Log($"Input {fieldIndex}: '{userInput}' vs '{correctAnswer}'");

        // Karşılaştır
        if (userInput == correctAnswer)
        {
            // DOĞRU
            correctCount++;
            // Alanı kilitle ve doğru cevabı yaz
            if (!useTMPInputs)
            {
                inputFields[fieldIndex].interactable = false;
                inputFields[fieldIndex].text = correctAnswer;
            }
            else
            {
                inputFieldsTMP[fieldIndex].interactable = false;
                inputFieldsTMP[fieldIndex].text = correctAnswer;
            }
            SetFeedback("✓ DOĞRU! " + answers[fieldIndex], Color.green);
            
            Debug.Log($"✓ DOĞRU! Sayaç: {correctCount}/{fieldCount}");

            // Eğer tüm kelimeler doğruysa finali etkinleştir
            if (correctCount == fieldCount)
            {
                Debug.Log("Tüm kelimeler çözüldü — final etkinleştiriliyor.");
                if (finalInput != null)
                {
                    finalInput.interactable = true;
                    if (feedbackText != null) feedbackText.text = "Tebrikler! Final aktif.";
                }
            }
        }
        else
        {
            // YANLIŞ
            if (!useTMPInputs)
            {
                inputFields[fieldIndex].text = "";
            }
            else
            {
                inputFieldsTMP[fieldIndex].text = "";
            }
            SetFeedback("✗ YANLIŞŞ!", Color.red);
            
            Debug.Log($"✗ YANLIŞŞ! Beklenen: {correctAnswer}");
        }
    }

    private void CheckFinalInput(string userText)
    {
        if (string.IsNullOrWhiteSpace(userText))
        {
            SetFeedback("", Color.white);
            return;
        }

        string userInput = userText.Trim().ToUpper();
        string correctFinal = finalAnswer.ToUpper();

        Debug.Log($"Final: '{userInput}' vs '{correctFinal}'");

        if (userInput == correctFinal)
        {
            // DOĞRU - BULMACA BİTTİ
            SetFeedback("✓ HARIKA! Bulmaca çözüldü!", Color.green);
            if (!useTMPInputs)
            {
                if (finalInput != null)
                {
                    finalInput.interactable = false;
                    finalInput.text = finalAnswer;
                }
            }
            else
            {
                if (finalInputTMP != null)
                {
                    finalInputTMP.interactable = false;
                    finalInputTMP.text = finalAnswer;
                }
            }
            
            Debug.Log("🎉 BULMACA ÇÖZÜLDÜü");
            Invoke("GoToNextScene", 1.5f);
        }
        else
        {
            // YANLIŞ
            if (!useTMPInputs)
            {
                if (finalInput != null) finalInput.text = "";
            }
            else
            {
                if (finalInputTMP != null) finalInputTMP.text = "";
            }
            SetFeedback("✗ YANLIŞŞ!", Color.red);
            
            Debug.Log($"✗ Beklenen: {correctFinal}");
        }
    }

    private void SetFeedback(string message, Color color)
    {
        if (feedbackText != null)
        {
            feedbackText.text = message;
            feedbackText.color = color;
            return;
        }

        if (feedbackTextTMP != null)
        {
            feedbackTextTMP.text = message;
            feedbackTextTMP.color = color;
            return;
        }

        // Fallback: konsola yaz
        if (!string.IsNullOrEmpty(message)) Debug.Log(message);
    }

    private void GoToNextScene()
    {
        SceneManager.LoadScene("ToBeContinue");
    }
}