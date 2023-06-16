using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TitleScreenManager : MonoBehaviour
{

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
    private bool isMenuActive = false;
    
    

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        isMenuActive = true;
        tutorialText.gameObject.SetActive(false);
        tutorialButton.gameObject.SetActive(true);
        mapSelect.gameObject.SetActive(false);
        startButton.gameObject.SetActive(true);
        multiplayerButton.gameObject.SetActive(true);
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
        mapSelect.gameObject.SetActive(true);
        startButton.gameObject.SetActive(false);
        audioSource.PlayOneShot(selectSound);
        tutorialButton.gameObject.SetActive(false);
        multiplayerButton.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(firstMapButton);
    }

   
    public void StartMultiplayer()
    {
        SceneManager.LoadScene("MPMenu");
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
        mapSelect.gameObject.SetActive(false);
        tutorialText.gameObject.SetActive(false);
        startButton.gameObject.SetActive(true);
        audioSource.PlayOneShot(backSound);
        tutorialButton.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(startButton);
        multiplayerButton.gameObject.SetActive(true);
    }

    public void ShowTutorial()
    {
        tutorialText.gameObject.SetActive(true);
        startButton.gameObject.SetActive(false);
        audioSource.PlayOneShot(selectSound);
        tutorialButton.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(backButton);
    }
}

