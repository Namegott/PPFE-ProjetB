using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
    [SerializeField] TextMeshProUGUI GamePseudoTextBox;
    [SerializeField] int GameScore;
    [SerializeField] string GamePseudo;

    [SerializeField] ScoreManager ScoreManager;
    [SerializeField] GameObject FinishMenu;
    [SerializeField] SaveManager SaveManager;
    [SerializeField] EventSystem EventSystem;
    [SerializeField] Selectable PseudoInput;
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
        GameScore = ScoreManager.ScoreCalculator();

        EndStatus.text = endStatus;
        if (endStatus == "LEVEL FINISHED !")
        {
            EndStatus.color = new Vector4(0.01579743f, 0.6698113f, 0.01579743f, 1);
        }
        else if (endStatus == "YOU DIED !")
        {
            EndStatus.color = new Vector4(0.6698113f, 0.01579743f, 0.01579743f, 1);
        }
        GameScoreTextBox.text = GameScore.ToString();

        //show the highscores
        Pseudo = SaveManager.GetPseudo(SceneNumber);
        Score = SaveManager.GetScore(SceneNumber);
        
        for(int i = 0; i < PseudoTextBox.Length; i++)
        {
            PseudoTextBox[i].text = Pseudo[i];
            ScoreTextBox[i].text = Score[i].ToString();
        }

        //set actual score color
        if (GameScore >= Score[0])
        {
            GameScoreTextBox.color = new Vector4(0.9372549f, 0.7490196f, 0.01568628f, 1);
            GamePseudoTextBox.color = new Vector4(0.9372549f, 0.7490196f, 0.01568628f, 1);
        }
        else if (GameScore >= Score[1])
        {
            GameScoreTextBox.color = new Vector4(0.7529412f, 0.7529412f, 0.7529412f, 1);
            GamePseudoTextBox.color = new Vector4(0.7529412f, 0.7529412f, 0.7529412f, 1);
        }
        else if (GameScore >= Score[2])
        {
            GameScoreTextBox.color = new Vector4(0.8078431f, 0.5372549f, 0.2745098f, 1);
            GamePseudoTextBox.color = new Vector4(0.8078431f, 0.5372549f, 0.2745098f, 1);
        }
        else if (GameScore >= Score[3] || GameScore >= Score[4])
        {
            GameScoreTextBox.color = Color.black;
            GamePseudoTextBox.color = Color.black;
        }
        else
        {
            GameScoreTextBox.color = new Vector4(0.6698113f, 0.01579743f, 0.01579743f, 1);
            GamePseudoTextBox.color = new Vector4(0.6698113f, 0.01579743f, 0.01579743f, 1);
        }

        FinishMenu.SetActive(true);
        EventSystem.SetSelectedGameObject(PseudoInput.gameObject);

        FindAnyObjectByType<CameraManager>().ForceStopCam();
        Destroy(FindAnyObjectByType<MovementManager>().gameObject);
        Destroy(FindAnyObjectByType<PauseManager>().gameObject);
    }

    public void UpdatePseudo(TextMeshProUGUI pseudo)
    {
        GamePseudo = pseudo.text;
    }

    public void ReturnMainMenu()
    {
        GamePseudo = GamePseudoTextBox.text;
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
