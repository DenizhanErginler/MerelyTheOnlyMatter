using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PrologueManager1 : MonoBehaviour
{
    [SerializeField] private Image characterImage;
    [SerializeField] private Image mapImage;
    [SerializeField] private Button continueButton;
    [SerializeField] private DialogManager1 dialogManager1;
    [SerializeField] private Sprite[] characterSprites;

    private string[] dialogueTexts = new string[]
    {
        "Beatrice: \nSonunda! Akik ha? Tamamdır, ilk anahtar kelimem hazır.",
        "Beatrice: \nSıra ikinci rotada. Neredeydi bu kaya??"
    };

    private int currentDialogueIndex = 0;
    private bool isWaitingForInput = false;

    private void Start()
    {
        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinueClicked);

        if (dialogManager1 == null)
        {
            Debug.LogError("DialogManager1 boş!");
            return;
        }

        // Başla
        StartCoroutine(PlayDialogues());
    }

    private IEnumerator PlayDialogues()
    {
        while (currentDialogueIndex < dialogueTexts.Length)
        {
            // Sprite değiştir
            if (currentDialogueIndex < characterSprites.Length && characterSprites[currentDialogueIndex] != null)
            {
                characterImage.sprite = characterSprites[currentDialogueIndex];
            }

            // Yazı yazı göster
            yield return StartCoroutine(dialogManager1.ShowDialogWithTyping(dialogueTexts[currentDialogueIndex]));

            // Tıklanmasını bekle
            isWaitingForInput = true;
            continueButton.gameObject.SetActive(true);

            yield return new WaitUntil(() => !isWaitingForInput);

            continueButton.gameObject.SetActive(false);
            currentDialogueIndex++;

            yield return new WaitForSeconds(1f);
        }

        // Tüm dialoglar bitti - Hepsi gizle
        dialogManager1.HideDialog();
        characterImage.gameObject.SetActive(false);
        mapImage.gameObject.SetActive(false);
        Debug.Log("Dialoglar bitti!");
    }

    private void OnContinueClicked()
    {
        if (isWaitingForInput)
            isWaitingForInput = false;
    }
}