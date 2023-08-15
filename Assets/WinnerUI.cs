using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using TMPro;

public class WinnerUI : MonoBehaviour
{
    public TMP_Text winnerText;

    private void Awake()
    {
      
    }

    public void SetWinner(Player winner)
    {
        if (winner != null)
        {
            winnerText.text = winner.NickName + " has achieved victory!";
        }

        else
        {
            winnerText.text = "No winner";
        }
    }
}
