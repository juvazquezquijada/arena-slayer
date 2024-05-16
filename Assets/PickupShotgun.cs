using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupShotgun : MonoBehaviour
{
    public GameObject playerWep;
    public GameObject AmmoHUD;
    public GameObject PickupText;
    // Start is called before the first frame update
    void Start()
    {
        playerWep.SetActive(false);
        AmmoHUD.SetActive(false);
        PickupText.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player1")
        {
            PickupText.SetActive(true);
            if(Input.GetKey(KeyCode.E))
            {
                this.gameObject.SetActive(false);
                playerWep.SetActive(true);
                AmmoHUD.SetActive(true);
                PickupText.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PickupText.SetActive(false);
    }
}

    
