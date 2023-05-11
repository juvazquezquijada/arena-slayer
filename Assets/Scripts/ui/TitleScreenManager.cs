using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TitleScreenManager : MonoBehaviour
{

    public Button startButton;
    public GameObject mapSelect;
    public AudioSource audioSource;
    public AudioClip selectSound;
    public AudioClip backSound;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StartGame() // Goes to the map selection menu
    {
        mapSelect.gameObject.SetActive(true);
        startButton.gameObject.SetActive(false);
        audioSource.PlayOneShot(selectSound);
        
    }
    public void StartWarehouse() // starts the warehouse map
    {
        SceneManager.LoadScene("Warehouse");
        audioSource.PlayOneShot(selectSound);
    }
    public void StartCityDay() // starts the CityDay map
    {
        SceneManager.LoadScene("CityDay");
        audioSource.PlayOneShot(selectSound);
    }
    public void StartCityNight()// starts the CityNight map
    {
        SceneManager.LoadScene("CityNight");
        audioSource.PlayOneShot(selectSound);
    }
    public void StartStore()// starts the store map
    {
        SceneManager.LoadScene("Store");
        audioSource.PlayOneShot(selectSound);
    }

    public void BackToTitle()
    {
        mapSelect.gameObject.SetActive(false);
        startButton.gameObject.SetActive(true);
        audioSource.PlayOneShot(backSound);
    }
}

