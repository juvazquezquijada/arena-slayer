using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class CanvasManager : MonoBehaviour
{
    public TextMeshProUGUI health; //health indicator
    public TextMeshProUGUI ammo; //ammo indicator
    public TextMeshProUGUI score; //score indicator
    public AudioClip niceSound; // plays when player reaches a certain amount of kills ;)
    public AudioSource audioSource; 
    public bool gameActive = true; 
    public TextMeshProUGUI gameOverText; // game over screen
    public GameObject restartButton; // go back to home screen
    public GameObject retryButton;
    public TextMeshProUGUI outOfAmmoText; // tells the player they are out of ammo
    public TextMeshProUGUI lowAmmoText; // tells the player they have low ammo
    public TextMeshProUGUI lowHealthText; // tells the player they have low health
    public TextMeshProUGUI tutorialText; // text for the tutorial
    private static CanvasManager _instance;
    public AudioClip menuSound;
    public AudioClip pauseSound;
    public AudioClip hoverSound;
    public AudioSource musicAudioSource;
   // used to lock camera when player is dead
    public Camera myCamera;
    //pause screen
    public GameObject pauseMenuPanel;
    private bool isPaused;
    private Quaternion savedRotation;
    private GameObject player;



    void Start()
    {
        audioSource = GetComponent<AudioSource>();
          gameActive = true;
          pauseMenuPanel.SetActive(false);
        isPaused = false;
        player = GameObject.FindGameObjectWithTag("Player");

    }

    void Update()
    {
        // show or hide tutorial text based on T key press
        if (Input.GetKeyDown(KeyCode.T)|| Input.GetKeyDown(KeyCode.Joystick1Button3))
        {
            tutorialText.gameObject.SetActive(true);
        }
        // show pause menu when escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape)|| 
        Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            PauseGame();
        }
        // trigger slowmo
        if (Input.GetKeyDown(KeyCode.Space)&& !isPaused)
        {
            Time.timeScale = 0.3f;
        }

        if (Input.GetKeyUp(KeyCode.Space)&& !isPaused)
        {
            Time.timeScale = 1;
        }

    }

    public static CanvasManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CanvasManager>();
            }

            return _instance;
            
        }
    }

    private void Awake()
    {
        if(_instance !=null && _instance !=this)
        {
            Destroy(this.gameObject);
        }
        else 
        {
            _instance = this;
        }
          
          
    }

    public void UpdateHealth(int healthValue)
    {
        if (health != null)
        {
            health.text = healthValue.ToString();
        }

        UpdateHealthIndicator(healthValue);
    }

    public void UpdateAmmo(int ammoValue) 
    {
        if (ammo != null)
        {
            ammo.text = ammoValue.ToString();
            UpdateAmmoText();
        }
    }
    private void UpdateAmmoText()
{
    if (player.GetComponent<PlasmaGun>().enabled)
    {
        int plasmaGunAmmo = player.GetComponent<PlasmaGun>().GetCurrentAmmo();
        ammo.text = plasmaGunAmmo.ToString();
    }
    else if (player.GetComponent<RPG>().enabled)
    {
        int rpgAmmo = player.GetComponent<RPG>().GetCurrentAmmo();
        ammo.text = rpgAmmo.ToString();
    }
    // Add conditions for other weapons if needed
}

    public void UpdateScore(int scoreValue)
    {
        if (score != null)
        {
            int currentScore = int.Parse(score.text);
            int newScore = currentScore + scoreValue;
            score.text = newScore.ToString();

            if (newScore == 69 && audioSource != null) // check if score is 69 and audio source is assigned
            {
                audioSource.PlayOneShot(niceSound);
            }
        }
    }

    private void UpdateHealthIndicator(int healthValue)
    {
        if (health != null)
        {
            if (healthValue >= 70)
            {
                health.color = Color.blue;
            }
            else if (healthValue >= 25 && healthValue < 75)
            {
                health.color = Color.yellow;
            }
            else
            {
                health.color = Color.red;
            }

        }
    }

    public void GameOver()
{
    gameOverText.gameObject.SetActive(true);
    gameActive = false;
    restartButton.gameObject.SetActive(true);
    retryButton.gameObject.SetActive(true);
    
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
    myCamera.GetComponent<Camera>().enabled = false;
    lowAmmoText.gameObject.SetActive(false);
    lowHealthText.gameObject.SetActive(false);
    outOfAmmoText.gameObject.SetActive(false);

}
     public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        myCamera.GetComponent<Camera>().enabled = false;
        savedRotation = myCamera.transform.rotation;
        Time.timeScale = 0f; // Pause the game
        if (!isPaused)
        {
            // Show pause menu
            
            isPaused = true;
            pauseMenuPanel.SetActive(true);
             musicAudioSource.Pause();
            audioSource.PlayOneShot(pauseSound);
        }
        else
        {
            // Hide pause menu
            Time.timeScale = 1f; // Unpause the game
            isPaused = false;
            pauseMenuPanel.SetActive(false);
            myCamera.GetComponent<Camera>().enabled = true;
            Cursor.visible = false;
            myCamera.transform.rotation = savedRotation;
            musicAudioSource.UnPause();
        }
    }
    public void ResumeGame()
    {
        // Unpause the game
    Time.timeScale = 1f;
    isPaused = false;
    pauseMenuPanel.SetActive(false);

    // Enable the camera and hide the cursor
    myCamera.GetComponent<Camera>().enabled = true;
    Cursor.visible = false;
    Cursor.lockState = CursorLockMode.Locked;
    myCamera.transform.rotation = savedRotation;
    gameActive = true;
    audioSource.PlayOneShot(menuSound);
    musicAudioSource.UnPause();

    }

    public void QuitGame()
    {
        SceneManager.LoadScene("TitleScene");
        AudioListener.pause = false; // Resume the music
        myCamera.GetComponent<Camera>().enabled = true;
        audioSource.PlayOneShot(menuSound);
        isPaused = false;
        Time.timeScale = 1f;
    }
    public void RetryGame()
    {
        Time.timeScale = 1f; // Unpause the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
        audioSource.PlayOneShot(menuSound);
        isPaused = false;
    }
    public void PlayButtonHoverSound(BaseEventData eventData)
    {
    audioSource.PlayOneShot(hoverSound);
    }


    public void OutOfAmmo()
    {
        outOfAmmoText.gameObject.SetActive(true);
        lowAmmoText.gameObject.SetActive(false);
    }
    public void LowAmmo()
    {
        lowAmmoText.gameObject.SetActive(true);
        outOfAmmoText.gameObject.SetActive(false);
    }
    public void HasAmmo()
    {
        outOfAmmoText.gameObject.SetActive(false);
        lowAmmoText.gameObject.SetActive(false);
    }
    public void LowHealth()
    {
        lowHealthText.gameObject.SetActive(true);
    }
    public void HasHealth()
    {
        lowHealthText.gameObject.SetActive(false);
    }
    
}