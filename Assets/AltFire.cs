using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltFire : MonoBehaviour
{

    public SingleArmGun gun;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            gun.fireRate = 0.1f;
            gun.Use();
            gun.animator.speed = 2;
        }
        else if (Input.GetMouseButtonUp(1)) 
        {
            gun.fireRate = 0.3f;
            gun.animator.speed = 1;
        }
    }
}
