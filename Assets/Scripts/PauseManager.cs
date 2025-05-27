using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] MovementManager MovementManager;
    [SerializeField] CombatManager CombatManager;
    [SerializeField] AudioSource[] AllAudio;

    [SerializeField] GameObject GlobalMenu;
    [SerializeField] GameObject ControlsMenu;
    [SerializeField] GameObject GlobalButtons;

    [Header("Event System & Button Navigation")]
    [SerializeField] EventSystem EventSystem;
    [SerializeField] Selectable ResumeButton;
    [SerializeField] Selectable ReturnButton;
    [SerializeField] Selectable ControlsButton;

    [Header("Text Animation")]
    [SerializeField] TextMeshProUGUI PauseText;
    [SerializeField] float DelayAnimation;
    [SerializeField] Coroutine Animation;

    private void Start()
    {
        EventSystem = FindFirstObjectByType<EventSystem>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (GlobalMenu.activeInHierarchy == false)
            {
                Pause();
            }
            else if (ControlsMenu.activeInHierarchy == false)
            {
                Unpause();
            }
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;

        AllAudio = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in AllAudio)
        {
            audio.Pause();
        }

        MovementManager.GamePause = true;
        CombatManager.GamePause = true;
        GlobalMenu.SetActive(true);
        EventSystem.SetSelectedGameObject(ResumeButton.gameObject);
        AnimationStarter();
    }

    public void Unpause()
    {
        Time.timeScale = 1f;

        foreach (AudioSource audio in AllAudio)
        {
            audio.UnPause();
        }
        AllAudio = null;

        StopCoroutine(Animation);
        GlobalMenu.SetActive(false);
        MovementManager.GamePause = false;
        CombatManager.GamePause = false;
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
        GlobalButtons.SetActive(true);
        EventSystem.SetSelectedGameObject(ControlsButton.gameObject);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    void AnimationStarter()
    {
        Animation = StartCoroutine(PauseAnimation());
    }

    IEnumerator PauseAnimation()
    {
        PauseText.text = "PAUSE ";
        yield return new WaitForSecondsRealtime(DelayAnimation);
        PauseText.text = "PAUSE .";
        yield return new WaitForSecondsRealtime(DelayAnimation);
        PauseText.text = "PAUSE ..";
        yield return new WaitForSecondsRealtime(DelayAnimation);
        PauseText.text = "PAUSE ...";
        yield return new WaitForSecondsRealtime(DelayAnimation);
        AnimationStarter();
    }
}
