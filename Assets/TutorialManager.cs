using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject PlayerWep;
    public GameObject AmmoHUD;
    private static TutorialManager instance;
    
    public void ShowWeapon()
    {
        PlayerWep.SetActive(true);
    }

    public void ShowHUD()
    {
        AmmoHUD.SetActive(true);
    }
}
