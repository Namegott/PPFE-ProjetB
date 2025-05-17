using TMPro;
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
    [SerializeField] GameObject GlobalButtons;
    [SerializeField] GameObject PlayMenu;

    [Header("Button Navigation")]
    [SerializeField] Selectable ReturnButton;
    [SerializeField] Selectable Map1Button;
    [SerializeField] Selectable StartButton;
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

    public void Play(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

    public void Controls()
    {
        GlobalButtons.SetActive(false);
        ControlsMenu.SetActive(true);
        EventSystem.SetSelectedGameObject(ReturnButton.gameObject);
    }

    public void Return()
    {
        ControlsMenu.SetActive(false);
        PlayMenu.SetActive(false);
        GlobalButtons.SetActive(true);
        EventSystem.SetSelectedGameObject(StartButton.gameObject);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
