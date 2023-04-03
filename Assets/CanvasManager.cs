using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    public TextMeshProUGUI health;
    public TextMeshProUGUI ammo; 
    public TextMeshProUGUI score;

    public Image healthIndicator;

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
            score.text = scoreValue.ToString();
        }
    }

    private void UpdateHealthIndicator(int healthValue)
    {
        if (healthIndicator != null)
        {
            if (healthValue >= 75)
            {
                healthIndicator.color = Color.blue;
            }
            else if (healthValue >= 25 && healthValue < 75)
            {
                healthIndicator.color = Color.yellow;
            }
            else
            {
                healthIndicator.color = Color.red;
            }

            healthIndicator.fillAmount = healthValue / 100f;
        }
    }
}
