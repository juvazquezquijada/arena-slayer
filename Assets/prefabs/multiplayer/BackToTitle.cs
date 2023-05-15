using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToTitle : MonoBehaviour
{
    public void BackToSinglePlayer()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
