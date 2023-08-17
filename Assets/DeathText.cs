using UnityEngine;
using TMPro;

public class DeathText : MonoBehaviour
{
    [SerializeField]
    string[] proTips = {
        "Pro Tip: Keep an eye on your surroundings!",
        "Pro Tip: Try to take cover during battles.",
        "Pro Tip: Reload your weapon during downtime.",
        // Add more pro tips as needed
    };

    [SerializeField] public TextMeshProUGUI tipText;

    void Start()
    {
        if (tipText != null)
        {
            // Display a random pro tip from the array
            tipText.text = proTips[Random.Range(0, proTips.Length)];

            Destroy(gameObject, 3f);
        }
    }
}
