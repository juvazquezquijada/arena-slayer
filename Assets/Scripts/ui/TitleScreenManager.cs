using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TitleScreenManager : MonoBehaviour
{

    public Button startButton;
    public GameObject mapSelect;
    private void Start()
    {
        
    }

    public void StartGame() // Goes to the map selection menu
    {
        mapSelect.gameObject.SetActive(true);
        startButton.gameObject.SetActive(false);
        
    }
    public void StartWarehouse() // starts the warehouse map
    {
        SceneManager.LoadScene("Warehouse");
    }
    public void StartCityDay() // starts the CityDay map
    {
        SceneManager.LoadScene("CityDay");
    }
    public void StartCityNight()// starts the CityNight map
    {
        SceneManager.LoadScene("CityNight");
    }
    public void StartStore()// starts the store map
    {
        SceneManager.LoadScene("Store");
    }

    public void BackToTitle()
    {
        mapSelect.gameObject.SetActive(false);
        startButton.gameObject.SetActive(true);
    }
}

