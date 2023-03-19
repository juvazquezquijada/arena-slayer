using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunAnimationController : MonoBehaviour
{
    private Animator animator;
    private bool shotFired = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !shotFired)
        {
            animator.SetTrigger("Fire");
            shotFired = true;
        }

        if (shotFired && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Cocking"))
        {
            ResetShotFired();
        }
    }

    public void ResetShotFired()
    {
        shotFired = false;
    }
  
    public void ResetAnimation()
    {
    GameObject.FindWithTag("Player").GetComponent<PlayerController>().ResetShotFired();
    }

}

