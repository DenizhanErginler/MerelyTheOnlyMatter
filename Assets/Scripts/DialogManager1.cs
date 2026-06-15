using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogManager1 : MonoBehaviour
{
    [SerializeField] private GameObject dialogBox;
    [SerializeField] private Text dialogText;
    [SerializeField] private float typingSpeed = 0.05f;

    public IEnumerator ShowDialogWithTyping(string dialog)
    {
        dialogBox.SetActive(true);
        dialogText.text = "";

        foreach (char letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void HideDialog()
    {
        dialogBox.SetActive(false);
    }
}