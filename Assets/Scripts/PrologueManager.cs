using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PrologueManager : MonoBehaviour
{
    [SerializeField] private Image characterImage;
    [SerializeField] private Image mapImage;
    [SerializeField] private Button continueButton;
    [SerializeField] private DialogManager dialogManager;
    [SerializeField] private Sprite[] characterSprites;

    private string[] dialogueTexts = new string[]
    {
        "Beatrice: \nBu rotayı yıllardır yürüyorum. Gözlerimi kapadığımda elimdeki haritayı en ince detayına kadar görebiliyorum. Elli koca yıl. Sonunda ipuçlarının ne anlama geldiğini anladım sanırım.",
        "Beatrice: \nİlk ağaç… Bütün ormanda en çok benzeyen türler bu tarafta. Her seferinde hangisi olduğunu karıştırıyorum ama. Kitabın yeşil olması da cabası. Gel de bul şimdi."
    };

    private int currentDialogueIndex = 0;
    private bool isWaitingForInput = false;

    private void Start()
    {
        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinueClicked);

        if (dialogManager == null)
        {
            Debug.LogError("DialogManager boş!");
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
            yield return StartCoroutine(dialogManager.ShowDialogWithTyping(dialogueTexts[currentDialogueIndex]));

            // Tıklanmasını bekle
            isWaitingForInput = true;
            continueButton.gameObject.SetActive(true);

            yield return new WaitUntil(() => !isWaitingForInput);

            continueButton.gameObject.SetActive(false);
            currentDialogueIndex++;

            yield return new WaitForSeconds(1f);
        }

        // Tüm dialoglar bitti - Hepsi gizle
        dialogManager.HideDialog();
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