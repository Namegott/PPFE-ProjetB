using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Finish : MonoBehaviour
{
    [SerializeField] string[] Pseudo;
    [SerializeField] int[] Score;
    [SerializeField] TextMeshProUGUI[] PseudoTextBox;
    [SerializeField] TextMeshProUGUI[] ScoreTextBox;
    [SerializeField] TextMeshProUGUI EndStatus;
    [SerializeField] TextMeshProUGUI GameScoreTextBox;
    [SerializeField] int GameScore;
    [SerializeField] string GamePseudo;

    [SerializeField] ScoreManager ScoreManager;
    [SerializeField] GameObject FinishMenu;
    [SerializeField] SaveManager SaveManager;
    [SerializeField] EventSystem EventSystem;
    [SerializeField] Selectable PseudoInput;
    [SerializeField] Selectable MainMenuButtonInput;
    [SerializeField] int SceneNumber;

    void Start()
    {
        ScoreManager = FindAnyObjectByType<ScoreManager>();
        EventSystem = FindFirstObjectByType<EventSystem>();
        SaveManager = GetComponent<SaveManager>();
    }

    private void Update()
    {
        if (FinishMenu.activeInHierarchy && Input.GetButtonDown("Submit"))
        {
            if (GamePseudo != "")
            {
                ReturnMainMenu();
            }
            else
            {
                Debug.Log("no nickname !");
            }
        }
    }

    public void FinishGame(string endStatus)
    {
        FinishMenu.SetActive(true);
        GameScore = ScoreManager.ScoreCalculator();
        EventSystem.SetSelectedGameObject(PseudoInput.gameObject);

        EndStatus.text = endStatus;
        GameScoreTextBox.text = GameScore.ToString();

        //show the highscores
        Pseudo = SaveManager.GetPseudo(SceneNumber);
        Score = SaveManager.GetScore(SceneNumber);
        
        for(int i = 0; i < PseudoTextBox.Length; i++)
        {
            PseudoTextBox[i].text = Pseudo[i];
            ScoreTextBox[i].text = Score[i].ToString();
        }

        Destroy(FindAnyObjectByType<MovementManager>().gameObject);
    }

    public void UpdatePseudo(TextMeshProUGUI pseudo)
    {
        GamePseudo = pseudo.text;
    }

    public void ReturnMainMenu()
    {
        //Debug.Log("a");
        //save the score (if highscore)
        SaveManager.SetHighScore(SceneNumber, GameScore, GamePseudo);
        //Debug.Log("b");
        SceneManager.LoadScene(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            FinishGame("LEVEL FINISHED !");
        }
    }
}
