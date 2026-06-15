using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogManager2 : MonoBehaviour
{
    [SerializeField] private GameObject dialogBox;
    [SerializeField] private Text dialogText;
    [SerializeField] private float typingSpeed = 0.05f;

    private bool skipTyping = false;
    private string currentFullText = "";

    public IEnumerator ShowDialogWithTyping(string dialog)
    {
        dialogBox.SetActive(true);
        dialogText.text = "";
        skipTyping = false;
        currentFullText = dialog;

        foreach (char letter in dialog.ToCharArray())
        {
            if (skipTyping)
            {
                dialogText.text = currentFullText;
                skipTyping = false;
                yield break;
            }

            dialogText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void SkipToFullText()
    {
        skipTyping = true;
        dialogText.text = currentFullText;
    }

    public void HideDialog()
    {
        dialogBox.SetActive(false);
    }
}
