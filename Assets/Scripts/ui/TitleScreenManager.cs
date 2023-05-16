using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TitleScreenManager : MonoBehaviour
{

    public Button startButton;
    public GameObject mapSelect;
    public TextMeshProUGUI tutorialText;
    public Button tutorialButton;
    public AudioSource audioSource;
    public AudioClip selectSound;
    public AudioClip backSound;

    private bool usingController = false;
    private Selectable currentSelection;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0 && !usingController)
        {
            // Switch to controller input if horizontal axis is detected
            usingController = true;
            currentSelection = startButton;
            currentSelection.Select();
        }

        if (usingController)
        {
            // Move the current selection using controller input
            if (Input.GetAxis("Horizontal") > 0)
            {
                currentSelection.FindSelectableOnRight().Select();
                currentSelection = currentSelection.FindSelectableOnRight();
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                currentSelection.FindSelectableOnLeft().Select();
                currentSelection = currentSelection.FindSelectableOnLeft();
            }

            if (Input.GetAxis("Vertical") > 0)
            {
                currentSelection.FindSelectableOnUp().Select();
                currentSelection = currentSelection.FindSelectableOnUp();
            }
            else if (Input.GetAxis("Vertical") < 0)
            {
                currentSelection.FindSelectableOnDown().Select();
                currentSelection = currentSelection.FindSelectableOnDown();
            }

            // Trigger button action if "A" button is pressed
            if (Input.GetButtonDown("Submit"))
            {
                currentSelection.GetComponent<Button>().onClick.Invoke();
            }
        }
    }

    public void StartGame()
    {
        mapSelect.gameObject.SetActive(true);
        startButton.gameObject.SetActive(false);
        audioSource.PlayOneShot(selectSound);
        tutorialButton.gameObject.SetActive(false);
        currentSelection = mapSelect.GetComponentInChildren<Button>();
        currentSelection.Select();
    }

    




    public void StartMultiplayer()
    {
        SceneManager.LoadScene("MPMenu");
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
        tutorialText.gameObject.SetActive(false);
        startButton.gameObject.SetActive(true);
        audioSource.PlayOneShot(backSound);
        tutorialButton.gameObject.SetActive(true);
    }

    public void ShowTutorial()
    {
        tutorialText.gameObject.SetActive(true);
        startButton.gameObject.SetActive(false);
        audioSource.PlayOneShot(selectSound);
        tutorialButton.gameObject.SetActive(false);
    }
}

