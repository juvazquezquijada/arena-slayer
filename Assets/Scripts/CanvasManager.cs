using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    public TextMeshProUGUI health;
    public TextMeshProUGUI ammo; 
    public TextMeshProUGUI score;
    public AudioClip niceSound; 
    public AudioSource audioSource; // add an audio source to the CanvasManager game object and assign it to this field
    private static CanvasManager _instance;
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
          audioSource = GetComponent<AudioSource>();
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
}