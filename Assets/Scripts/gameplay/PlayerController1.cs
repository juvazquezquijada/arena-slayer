using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController1 : MonoBehaviour
{
    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, smoothTime, yMouseSensitivity;
    [SerializeField] GameObject playerCamera;
    [SerializeField] GameObject cameraHolder;
    [SerializeField] float jumpForce;
    [SerializeField] Item[] items;
    [SerializeField] AudioSource audioSource;
    [SerializeField] Image healthBar;
    [SerializeField] TMP_Text healthbarText;
    [SerializeField] GameObject ui;
    [SerializeField] GameObject wepCamera;
    [SerializeField] Animator animator;

    public TextMeshProUGUI health, score; //health indicator

    public AudioClip hurtSound;
    public AudioClip jumpSound;
    public bool isJumping = false;
    public bool isPaused = false;
    public bool isDead = false;

    int itemIndex;
    int previousItemIndex = -1;
    float verticalLookRotation;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;

    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaRegenRate = 10f;
    public float sprintStaminaCost = 20f;
    public float jumpStaminaCost = 10f;
    public float refillDelayDuration = 3f;
    private bool isRefillingStamina;
    private float timeSinceLastAction;
    private bool isSprinting;

    public Image staminaBarImage;

    CharacterController characterController;

    const float maxHealth = 100f;
    float currentHealth = maxHealth;
    public GameObject lowHealthText;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentStamina = maxStamina;
        timeSinceLastAction = Time.time;
    }

    void Update()
    {
        if (isDead)
            return;

            Look();
            Move();

            for (int i = 0; i < items.Length; i++)
            {
                if (Input.GetKeyDown((i + 1).ToString()))
                {
                    EquipItem(i);
                    break;
                }
            }

            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
            {
                // Scroll up
                if (itemIndex < items.Length - 1)
                {
                    EquipItem(itemIndex + 1);
                }
            }
            else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
            {
                // Scroll down
                if (itemIndex > 0)
                {
                    EquipItem(itemIndex - 1);
                }
            }

            if (Input.GetMouseButton(0))
            {
                items[itemIndex].Use();
            }

            if (transform.position.y < -5f) // Die if you fall out of the world
            {
                Die();
            }

            if (characterController.isGrounded)
            {
                // Reset jumping flag
                isJumping = false;
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                // Call the Reload() method on the currently equipped gun
                if (items[itemIndex] is Gun equippedGun)
                {
                    equippedGun.Reload();
                }
            }

    

        if (currentHealth <= 30)
        {
            lowHealthText.gameObject.SetActive(true);
        }

        if (isDead == true)
        {
            moveAmount = Vector3.zero; // stop the movement
            SpawnManager.Instance.GameOver();
            return; // exit the method
        }

    }

    void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);
        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * yMouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    void Move()
    {
        float rotationY = transform.rotation.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0f, rotationY, 0f);

        Vector3 moveDir = rotation * new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        if (characterController.isGrounded)
        {
            // Check for sprint input and adjust movement speed and stamina
            if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0)
            {
                isSprinting = true;
                moveAmount = moveDir * sprintSpeed;
                currentStamina -= sprintStaminaCost * Time.deltaTime;
                timeSinceLastAction = Time.time;
            }
            else
            {
                isSprinting = false;
                moveAmount = moveDir * walkSpeed;
            }

            // Check for jump input and consume stamina
            if (Input.GetButton("Jump") && currentStamina >= jumpStaminaCost)
            {
                isJumping = true;
                moveAmount.y = jumpForce;
                currentStamina -= jumpStaminaCost;
                timeSinceLastAction = Time.time;
                audioSource.PlayOneShot(jumpSound);
            }
        }
        else
        {
            moveAmount.y += Physics.gravity.y * Time.deltaTime;

            if (moveAmount.y < 0)
            {
                isJumping = false;
            }
        }

        // Prevent sprinting when stamina is 0
        if (currentStamina <= 0)
        {
            isSprinting = false;
        }

        // Refill stamina if it has been refillDelayDuration seconds since the last action
        if (Time.time - timeSinceLastAction >= refillDelayDuration)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        }

        // Update stamina bar fill amount
        staminaBarImage.fillAmount = currentStamina / maxStamina;

        // Apply movement
        characterController.Move(moveAmount * Time.deltaTime);
    }
    void EquipItem(int _index)
    {
        if (_index == previousItemIndex)
            return;

        itemIndex = _index;

        items[itemIndex].itemGameObject.SetActive(true);

        if (previousItemIndex != -1)
        {
            items[previousItemIndex].itemGameObject.SetActive(false);
        }

        previousItemIndex = itemIndex;

        // Call the UpdateAmmoUIOnSwitch method on the currently equipped gun (if it is a SingleShotGun)
        if (items[itemIndex] is PlayerGun playerGun)
        {
            playerGun.UpdateAmmoUIOnSwitch();
        }
    }

    void PlayWeaponSwitch()
    {
        animator.SetTrigger("SwitchWeapon");
    }

    void GameOver()
    {
        Destroy(playerCamera);
        Destroy(wepCamera);
        Destroy(characterController);
        Destroy(ui);
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Player is Dead!");
    }
}
