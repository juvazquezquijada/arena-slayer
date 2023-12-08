using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TitleScreenManager : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject startButton;
    public GameObject backButton;
    public Button multiplayerButton;
    public GameObject mapSelect;
    public GameObject mapSelect2;
    public GameObject firstMapButton;
    public GameObject tutorialText;
    public Button tutorialButton;
    public AudioSource audioSource;
    public AudioClip selectSound;
    public AudioClip backSound;
    public GameObject loadingText;
    public GameObject mainMenu;



    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        tutorialText.gameObject.SetActive(false);
        mapSelect.gameObject.SetActive(false);
        multiplayerButton.gameObject.SetActive(true);
        settingsPanel.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Start"))
        {
            StartGame();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToTitle();
        }

    }
    public void StartGame()
    {
        mainMenu.gameObject.SetActive(true);
        startButton.gameObject.SetActive(false);
        audioSource.PlayOneShot(selectSound);
    }

    public void StartBoss()
    {
        SceneManager.LoadScene("BossArena");
    }


    public void StartMultiplayer()
    {
        SceneManager.LoadScene("MPMenu");
    }
    public void StartTraining() // starts the warehouse map
    {
        audioSource.PlayOneShot(selectSound);
        SceneManager.LoadScene("Training");
    }
    public void StartWarehouse() // starts the warehouse map
    {
        audioSource.PlayOneShot(selectSound);
        loadingText.gameObject.SetActive(true);
        SceneManager.LoadScene("Warehouse");
    }
    public void StartCityDay() // starts the CityDay map
    {
        audioSource.PlayOneShot(selectSound);
        loadingText.gameObject.SetActive(true);
        SceneManager.LoadScene("CityDay");
    }
    public void StartCityNight()// starts the CityNight map
    {
        audioSource.PlayOneShot(selectSound);
        loadingText.gameObject.SetActive(true);
        SceneManager.LoadScene("CityNight");

    }
    public void StartStore()// starts the store map
    {
        audioSource.PlayOneShot(selectSound);
        loadingText.gameObject.SetActive(true);
        SceneManager.LoadScene("Store");

    }
    public void StartMPArena()
    {
        audioSource.PlayOneShot(selectSound);
        loadingText.gameObject.SetActive(true);
        SceneManager.LoadScene("MPArena");
    }
    public void StartPool()
    {
        audioSource.PlayOneShot(selectSound);
        loadingText.gameObject.SetActive(true);
        SceneManager.LoadScene("Pool");
    }

    public void BackToTitle()
    {
        settingsPanel.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
        audioSource.PlayOneShot(backSound);
    }

    public void ShowTutorial()
    {
        settingsPanel.gameObject.SetActive(true);
        mainMenu.gameObject.SetActive(false);
        audioSource.PlayOneShot(selectSound);
    }
}