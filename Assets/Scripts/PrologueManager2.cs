using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PrologueManager2 : MonoBehaviour
{
    [SerializeField] private Button rockButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private DialogManager2 dialogManager2;
    [SerializeField] private Image characterImage;
    [SerializeField] private Text characterNameText;
    [SerializeField] private GameObject dialogBox2;
    [SerializeField] private GameObject characterImage2;
    [SerializeField] private Sprite[] characterSprites;

    private string[] characterNames = new string[] { "Beatrice", "William" };
    private string[] dialogueTexts = new string[]
    {
        "Beatrice: Ne!? Her zaman burdaydı?? Karıştırıyor muyum acaba?",
        "*hışırtı*",
        "Beatrice: Kim var orada! Elimdeki kayayı kullanmaktan çekinmem!!",
        "William: Kayayı kaldırınca bana da haber ver.",
        "Beatrice: Çocuk mu? Ne yapıyorsun evladım burada?",
        "William: Kitap okuyorum.",
        "Beatrice: Ben de onu arıyordum çok sağ ol. (Elinden çeker.)",
        "William: Hey, daha bitirmemiştin!",
        "Beatrice: Aynen ben de öyle. Aa, bunun birazını çözmüşsün!",
        "William: Niye zor muydu ki?"
    };

    private int[] characterIndices = new int[] { 0, 1, 0, 1, 0, 1, 0, 1 };

    private int currentDialogueIndex = 0;
    private bool isWaitingForInput = false;
    private bool dialogStarted = false;

    private void Start()
    {
        if (dialogBox2 != null)
            dialogBox2.SetActive(false);
        if (characterImage2 != null)
            characterImage2.SetActive(false);

        if (rockButton != null)
            rockButton.onClick.AddListener(OnRockButtonClicked);

        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinueClicked);

        if (dialogManager2 == null)
        {
            Debug.LogError("DialogManager2 boş!");
            return;
        }
    }

    private void OnRockButtonClicked()
    {
        if (!dialogStarted)
        {
            dialogStarted = true;
            rockButton.interactable = false;

            if (dialogBox2 != null)
                dialogBox2.SetActive(true);
            if (characterImage2 != null)
                characterImage2.SetActive(true);

            StartCoroutine(PlayDialogues());
        }
    }

    private IEnumerator PlayDialogues()
    {
        while (currentDialogueIndex < dialogueTexts.Length)
        {
            // Karakter güvenli erişim
            int charIndex = (currentDialogueIndex < characterIndices.Length) ? characterIndices[currentDialogueIndex] : 0;
            
            if (characterImage != null && charIndex >= 0 && charIndex < characterSprites.Length && characterSprites[charIndex] != null)
            {
                characterImage.sprite = characterSprites[charIndex];
            }

            if (characterNameText != null && charIndex >= 0 && charIndex < characterNames.Length)
            {
                characterNameText.text = characterNames[charIndex] + ":";
            }

            yield return StartCoroutine(dialogManager2.ShowDialogWithTyping(dialogueTexts[currentDialogueIndex]));

            isWaitingForInput = true;
            continueButton.gameObject.SetActive(true);

            yield return new WaitUntil(() => !isWaitingForInput);

            continueButton.gameObject.SetActive(false);
            currentDialogueIndex++;

            yield return new WaitForSeconds(0.3f);
        }

        // Tüm dialoglar bitti
        dialogManager2.HideDialog();
        if (characterImage != null)
            characterImage.gameObject.SetActive(false);
        if (characterNameText != null)
            characterNameText.gameObject.SetActive(false);

        Debug.Log("✅ Konuşma bitti!");
        
        // Crossword2 scene'e geç
        Invoke("GoToCrossword2", 1f);
    }

    private void OnContinueClicked()
    {
        if (isWaitingForInput)
        {
            dialogManager2.SkipToFullText();
            isWaitingForInput = false;
        }
    }

    private void GoToCrossword2()
    {
        Debug.Log("Crossword2'ye geçiliyor...");
        SceneManager.LoadScene("Crossword2");
    }
}