using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ImageDisplayer : MonoBehaviour
{
    [SerializeField] private Button bookButton;
    [SerializeField] private Image bookImage;

    private void Start()
    {
        if (bookButton != null)
            bookButton.onClick.AddListener(ShowImage);

        if (bookImage != null)
            bookImage.gameObject.SetActive(false);

        // Resim tıklandığında Crossword'e geç
        if (bookImage != null)
        {
            Button imageButton = bookImage.GetComponent<Button>();
            if (imageButton == null)
                imageButton = bookImage.gameObject.AddComponent<Button>();

            imageButton.onClick.AddListener(GoToCrossword);
        }
    }

    private void ShowImage()
    {
        if (bookImage != null)
            bookImage.gameObject.SetActive(true);
    }

    private void GoToCrossword()
    {
        SceneManager.LoadScene("Crossword");
    }
}