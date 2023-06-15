using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    
    private bool hasPrimary = true;
    private bool hasSecondary = false;
    private bool hasThird = false;
    public GameObject PrimaryWeapon;
    public GameObject SecondaryWeapon;
    public GameObject ThirdWeapon;
    //public GameObject FourthWeapon;
    //public GameObject FifthWeapon;
    public GameObject Player;
    public CanvasManager canvasManager;
    public Shotgun primaryWeapon;
    public PlasmaGun secondaryWeapon;
    public RPG thirdWeapon;
    //public DBS fourthWeapon;
    // public Sniper fifthWeapon




    void Start()
    {
        canvasManager = CanvasManager.Instance;
        PrimaryWeapon.gameObject.SetActive(true);
        SecondaryWeapon.gameObject.SetActive(false);
        ThirdWeapon.gameObject.SetActive(false);
        Player.GetComponent<Shotgun>().enabled = true;
        Player.GetComponent<PlasmaGun>().enabled = false;
        Player.GetComponent<RPG>().enabled = false;
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
        else if (Input.GetKeyDown(KeyCode.Alpha3) && !hasThird && hasSecondary && Time.timeScale > 0.5 ||
        Input.GetKeyDown(KeyCode.Joystick1Button5) && !hasThird && hasSecondary && Time.timeScale > 0.5)
        {
            SwitchToThirdWeapon();
        }

        else if (Input.GetAxis("Mouse ScrollWheel") > 0f)
    {
        if (hasPrimary)
        {
            SwitchToSecondaryWeapon();
        }
        else if (hasSecondary)
        {
            SwitchToThirdWeapon();
        }
    }
    else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
    {
        if (hasSecondary)
        {
            SwitchToPrimaryWeapon();
        }
        else if (!hasPrimary && !hasSecondary)
        {
            SwitchToSecondaryWeapon();
        }
    }
    }

    public void SwitchToPrimaryWeapon()
    {
        // enables primary
            PrimaryWeapon.SetActive(true);
            Player.GetComponent<Shotgun>().enabled = true;
        // disables secondary
            SecondaryWeapon.SetActive(false);
            Player.GetComponent<PlasmaGun>().enabled = false;
        // disables teritary
            ThirdWeapon.SetActive(false);
            Player.GetComponent<RPG>().enabled = false;
            //FourthWeapon.SetActive(false);
            hasPrimary = true;
            hasSecondary = false;
            hasThird = false;
        int primaryAmmoValue = primaryWeapon.GetCurrentAmmo();
        canvasManager.UpdateAmmo(primaryAmmoValue);
        Debug.Log("Switched to Primary Weapon");
       
    }

    public void SwitchToSecondaryWeapon()
    {
        // disables primary
            PrimaryWeapon.SetActive(false);
            Player.GetComponent<Shotgun>().enabled = false;
        // enables secondary 
            SecondaryWeapon.SetActive(true);
            Player.GetComponent<PlasmaGun>().enabled = true;
        // disables teritary
            ThirdWeapon.SetActive(false);
            Player.GetComponent<RPG>().enabled = false;
            hasPrimary = false;
            hasSecondary = true;
            hasThird = false;
        int secondaryAmmoValue = secondaryWeapon.GetCurrentAmmo();
        canvasManager.UpdateAmmo(secondaryAmmoValue);
        Debug.Log ("Switched to Secondary Weapon");
        Player.GetComponent<RPG>().enabled = false;
    }
    public void SwitchToThirdWeapon()
{
    //disables primary
    PrimaryWeapon.SetActive(false);
    Player.GetComponent<Shotgun>().enabled = false;
    hasPrimary = false;
    //disables secondary
    SecondaryWeapon.SetActive(false);
    Player.GetComponent<PlasmaGun>().enabled = false;
    hasSecondary = false;
    //enables teritary
    ThirdWeapon.SetActive(true);
    Player.GetComponent<RPG>().enabled = true;
    hasThird = true;
    
    

    int thirdAmmoValue = thirdWeapon.GetCurrentAmmo();
    canvasManager.UpdateAmmo(thirdAmmoValue);
    Debug.Log("Switched to Third Weapon");
}









}
