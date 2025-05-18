using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] string[] Pseudo;
    [SerializeField] int[] Score;
    [SerializeField] TextMeshProUGUI[] PseudoTextBox;
    [SerializeField] TextMeshProUGUI[] ScoreTextBox;
    [SerializeField] SaveManager SaveManager;

    [Header("Menus")]
    [SerializeField] GameObject ControlsMenu;
    [SerializeField] GameObject CreditsMenu;
    [SerializeField] GameObject GlobalButtons;
    [SerializeField] GameObject PlayMenu;

    [Header("Button Navigation")]
    [SerializeField] Selectable Map1Button;
    [SerializeField] Selectable ReturnControlsButton;
    [SerializeField] Selectable ReturnCreditsButton;
    [SerializeField] Selectable StartButton;
    [SerializeField] TextMeshProUGUI PlayIndicator;
    EventSystem EventSystem;

    private void Start()
    {
        EventSystem = FindFirstObjectByType<EventSystem>();
        SaveManager = GetComponent<SaveManager>();
    }

    public void StartPress()
    {
        GlobalButtons.SetActive(false);
        PlayMenu.SetActive(true);
        EventSystem.SetSelectedGameObject(Map1Button.gameObject);
    }

    public void SetRecords(int mapNumber)
    {
        Pseudo = SaveManager.GetPseudo(mapNumber);
        Debug.Log(Pseudo);
        Score = SaveManager.GetScore(mapNumber);
        Debug.Log(Score);
                
        for (int i = 0; i < PseudoTextBox.Length; i++)
        {
            PseudoTextBox[i].text = Pseudo[i];
            ScoreTextBox[i].text = Score[i].ToString();
        }
    }

    public void PlayColor(Color color)
    {
        PlayIndicator.color = color;
    }

    public void Play(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

    public void Controls()
    {
        GlobalButtons.SetActive(false);
        ControlsMenu.SetActive(true);
        EventSystem.SetSelectedGameObject(ReturnControlsButton.gameObject);
    }

    public void Credits()
    {
        GlobalButtons.SetActive(false);
        CreditsMenu.SetActive(true);
        EventSystem.SetSelectedGameObject(ReturnCreditsButton.gameObject);
    }

    public void Return()
    {
        ControlsMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        PlayMenu.SetActive(false);
        GlobalButtons.SetActive(true);
        EventSystem.SetSelectedGameObject(StartButton.gameObject);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
