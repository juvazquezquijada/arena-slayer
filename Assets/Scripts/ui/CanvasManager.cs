using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    public TextMeshProUGUI health;
    public TextMeshProUGUI ammo; 
    public TextMeshProUGUI score;
    public AudioClip niceSound; 
    public AudioSource audioSource; 
    public bool gameActive = true;
    public TextMeshProUGUI gameOverText;
    public GameObject restartButton;
    public TextMeshProUGUI outOfAmmoText;
    public TextMeshProUGUI lowAmmoText;
    public TextMeshProUGUI lowHealthText;
    public TextMeshProUGUI tutorialText;
    private float tutorialDuration = 5f;
    private float tutorialTimer = 0f;
    private static CanvasManager _instance;
    public Camera myCamera;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
          gameActive = true;
    
          
    }

    void Update()
    {
        // show or hide tutorial text based on T key press
        if (Input.GetKeyDown(KeyCode.T))
        {
            tutorialText.gameObject.SetActive(true);
            tutorialTimer = 0f;
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            tutorialText.gameObject.SetActive(false);
            tutorialTimer = 0f;
        }

        // hide tutorial text after duration has elapsed
        if (tutorialText.gameObject.activeSelf)
        {
            tutorialTimer += Time.deltaTime;
            if (tutorialTimer >= tutorialDuration)
            {
                tutorialText.gameObject.SetActive(false);
                tutorialTimer = 0f;
            }
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
        }
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
    
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
    myCamera.GetComponent<Camera>().enabled = false;
    lowAmmoText.gameObject.SetActive(false);
    lowHealthText.gameObject.SetActive(false);
    outOfAmmoText.gameObject.SetActive(false);

}
    public void RestartGame()
    {
        SceneManager.LoadScene("TitleScene");
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