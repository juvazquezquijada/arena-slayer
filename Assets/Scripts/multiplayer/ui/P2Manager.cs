using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class P2Manager: MonoBehaviour
{
    public TextMeshProUGUI health; //health indicator
    public TextMeshProUGUI ammo; //ammo indicator
    public TextMeshProUGUI score; //score indicator
    public AudioSource audioSource;
    private static P2Manager _instance;
    public GameObject player;
    public TextMeshProUGUI lowAmmoText;
    public TextMeshProUGUI lowHealthText;
    public TextMeshProUGUI outOfAmmoText;
    public bool gameActive = false;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameActive = true;
    }

    public static P2Manager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<P2Manager>();
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
        if (ammo.text == "25" && player.GetComponent<PlasmaGun>().enabled)
        {
            int plasmaGunAmmo = player.GetComponent<PlasmaGun>().GetCurrentAmmo();
            ammo.text = plasmaGunAmmo.ToString();
        }
    }
    public void UpdateScore(int scoreValue)
    {
        if (score != null)
        {
            int currentScore = int.Parse(score.text);
            int newScore = currentScore + scoreValue;
            score.text = newScore.ToString();
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