using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDoors : MonoBehaviour
{
    public GameObject PickupText;
    public Animator anim;
    
    void Start()
    {
        PickupText.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player1")
        {
            PickupText.SetActive(true);
            if(Input.GetKey(KeyCode.E))
            {
                anim.SetTrigger("OpenDoor");
                PickupText.SetActive(false);
            }
        }
    }
}
