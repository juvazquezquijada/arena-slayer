using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    
    private bool hasPrimary = true;
    private bool hasSecondary = false;
    public GameObject PrimaryWeapon;
    public GameObject SecondaryWeapon;
    public GameObject Player;
    public CanvasManager canvasManager;
    public Shotgun primaryWeapon;
    public PlasmaGun secondaryWeapon;




    void Start()
    {
        canvasManager = CanvasManager.Instance;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !hasPrimary  && Time.timeScale > 0.5 ||// or 
        Input.GetKeyDown(KeyCode.Joystick1Button4) && !hasPrimary && Time.timeScale > 0.5 ) // Switch to primary weapon
        {
            SwitchToPrimaryWeapon();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && !hasSecondary && Time.timeScale > 0.5 || // or
                 Input.GetKeyDown(KeyCode.Joystick1Button5) && !hasSecondary && Time.timeScale > 0.5) // Switch to secondary weapon
        {
            SwitchToSecondaryWeapon();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            SwitchToPrimaryWeapon();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            SwitchToSecondaryWeapon();
        }
    }

    public void SwitchToPrimaryWeapon()
    {
            PrimaryWeapon.SetActive(true);
            Player.GetComponent<Shotgun>().enabled = true;
            SecondaryWeapon.SetActive(false);
            Player.GetComponent<PlasmaGun>().enabled = false;
            hasPrimary = true;
            hasSecondary = false;
        int primaryAmmoValue = primaryWeapon.GetCurrentAmmo();
        canvasManager.UpdateAmmo(primaryAmmoValue);
        Debug.Log("Switched to Primary Weapon");
    }

    public void SwitchToSecondaryWeapon()
    {
            PrimaryWeapon.SetActive(false);
            Player.GetComponent<Shotgun>().enabled = false;
            SecondaryWeapon.SetActive(true);
            Player.GetComponent<PlasmaGun>().enabled = true;
            hasPrimary = false;
            hasSecondary = true;
        int secondaryAmmoValue = secondaryWeapon.GetCurrentAmmo();
        canvasManager.UpdateAmmo(secondaryAmmoValue);
        Debug.Log ("Switched to Secondary Weapon");
    }







}
