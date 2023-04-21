using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapSelect : MonoBehaviour
{
    public Button warehouseButton;
    public Button cityDayButton;
    public Button cityNightButton;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void StartWarehouse()
    {
        SceneManager.LoadScene("Warehouse");
    }
    public void StartCityDay()
    {
        SceneManager.LoadScene("CityDay");
    }
    public void StartCityNight()
    {
        SceneManager.LoadScene("CityNight");
    }
}
