using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public List<GameObject> weapons;
    private int currentWeaponIndex = 0;

    void Start()
    {
        // Set the first weapon as active
        ActivateWeapon(currentWeaponIndex);
    }

    void Update()
    {
        // Switch to the next weapon if the player presses the switch weapon key (e.g. Tab)
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Deactivate the current weapon
            DeactivateWeapon(currentWeaponIndex);

            // Increment the weapon index and wrap around if necessary
            currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;

            // Activate the new weapon
            ActivateWeapon(currentWeaponIndex);
        }
    }

    void ActivateWeapon(int index)
    {
        weapons[index].SetActive(true);
    }

    void DeactivateWeapon(int index)
    {
        weapons[index].SetActive(false);
    }
}
