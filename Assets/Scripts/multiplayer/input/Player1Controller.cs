using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Controller : MonoBehaviour
{
    //Basic player movement
    public float moveSpeed = 5.0f;
    public float shiftSpeed = 8.0f;
    public float gravity = 9.81f;
    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    //Status of the player
    public int health = 100;   
    public int score = 1;
    public int points = 0;
    public bool isDead = false;
    private Rigidbody rb;
    //Audio clips the player makes
    public AudioClip deathSound;
    public AudioClip healthPickup;
    private AudioSource audioSource;
    public AudioClip hurtSound;

    // Start is called before the first frame update
    void Start()
    {
    //Get CharacterController component
    Application.targetFrameRate = 60;
    controller = GetComponent<CharacterController>();
    rb = GetComponent<Rigidbody>();
    audioSource = GetComponent<AudioSource>();
    P1Manager.Instance.UpdateScore(score);
    P1Manager.Instance.UpdateHealth(health);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(5);
        }
      //Get input axis
      float horizontal = Input.GetAxis("P1Horizontal");
      float vertical = Input.GetAxis("P1Vertical");
      //Move the character controller
      controller.Move(moveDirection * Time.deltaTime);
      //Calculate the movement direction
      Vector3 forward = -transform.forward * vertical;
      Vector3 right = transform.right * horizontal;
      moveDirection = forward + right;
      //Apply gravity
      moveDirection.y -= gravity * Time.deltaTime;

       //controls the players health
        if (health <= 0)
        {

        }
        else if (health <= 40)
        {
            P1Manager.Instance.LowHealth();
        }
        else if (health > 40)
        {
            P1Manager.Instance.HasHealth();
        }
      //Increase move speed when the sprint button is held down
      if (Input.GetButton("P1Sprint"))
      {
        moveDirection *= shiftSpeed;
      }
      else
      {
        moveDirection *= moveSpeed;
      }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
        }
        P1Manager.Instance.UpdateHealth(health);
    } 

    public void UpdateHealth (int health)
    {
        P1Manager.Instance.UpdateHealth(health);
    }
}
