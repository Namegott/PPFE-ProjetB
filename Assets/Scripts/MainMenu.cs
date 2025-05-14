using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject ControlsMenu;
    [SerializeField] GameObject GlobalButtons;

    [Header("Event System & Button Navigation")]
    [SerializeField] EventSystem EventSystem;
    [SerializeField] Selectable ReturnButton;
    [SerializeField] Selectable ControlsButton;

    private void Start()
    {
        EventSystem = FindFirstObjectByType<EventSystem>();
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
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

    public void Quit()
    {
        Application.Quit();
    }
}
