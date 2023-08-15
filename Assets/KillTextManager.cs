using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KillTextManager : MonoBehaviour
{
    public TMP_Text killText;
    public float displayTime = 3f;

    void Start()
    {
        killText.gameObject.SetActive(false);
    }

    public void ShowKillText(string playerName)
    {
        killText.text = "You killed " + playerName;
        StartCoroutine(HideKillText());
        killText.gameObject.SetActive(true);
    }

    private IEnumerator HideKillText()
    {
        yield return new WaitForSeconds(displayTime);
        killText.gameObject.SetActive(false);
    }
}
