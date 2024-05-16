using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController1 : MonoBehaviour
{
    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, smoothTime, yMouseSensitivity;
    [SerializeField] GameObject playerCamera;
    [SerializeField] GameObject cameraHolder, weaponHolder, lowHealthAudio;
    [SerializeField] float jumpForce;
    [SerializeField] Item[] items;
    [SerializeField] AudioSource audioSource;
    [SerializeField] Image healthBar, damagedBar;
    [SerializeField] TMP_Text healthbarText;
    [SerializeField] GameObject wepCamera;
    [SerializeField] Animator animator;
    public static PlayerController1 Instance;

    private ScoreManager highScoreManager;
    public TextMeshProUGUI health, score; //health indicator
    public AudioClip pauseSound;
    public AudioClip hurtSound, jumpSound, deathSound, curseSound;
    private bool hasPlayedDeathSound = false;
    public bool isJumping = false;
    public bool isPaused = false;
    public bool isDead = false;
    public bool isCursed = false;
    public GameObject pauseMenuPanel, gameOverScreen, playerHud, curseBarUI;
    int itemIndex;
    int previousItemIndex = -1;
    float verticalLookRotation;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;
    public float curse = 0f;
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaRegenRate = 10f;
    public float sprintStaminaCost = 20f;
    public float jumpStaminaCost = 10f;
    public float refillDelayDuration = 3f;
    private bool isRefillingStamina;
    private float timeSinceLastAction;
    private float fillSpeed = 1f;
    public float damageBarShrinkTime = 1f;
    public bool isSprinting;
    public bool isGrounded = true;
    public bool isBenchPressing = false;
    public Transform armPivot; // Reference to the arm pivot (weapon holder)
    public Image staminaBarImage, curseBar;

    private PlayerGun playerGun;

    public CharacterController characterController;
    public TimerScript timer;
    float maxHealth = 100f;
    float maxCurse = 50f;
    float currentHealth;
    float currentScore = 0f;
    public GameObject lowHealthText;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        highScoreManager = GameObject.FindGameObjectWithTag("HighScoreManager").GetComponent<ScoreManager>();
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        EquipItem(0);
        animator.SetTrigger("SwitchWeapon");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        animator.ResetTrigger("BenchPress");
        currentStamina = maxStamina;
        timeSinceLastAction = Time.time;
        currentHealth = maxHealth;
        damagedBar.fillAmount = healthBar.fillAmount;
    }

    void Update()
    {
        if (isDead || isPaused || isBenchPressing)
        {
            return;
        }
        else if (!isDead || !isPaused|| !isBenchPressing)
        {
            Look(); // Call Look method only if not bench pressing
            Move();

            
        
            for (int i = 0; i < items.Length; i++)
            {
                if (Input.GetKeyDown((i + 1).ToString()))
                {
                    EquipItem(i);
                    animator.SetTrigger("SwitchWeapon");
                    break;
                }
            }

            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f && !isBenchPressing && !playerGun.isReloading)
            {
                // Scroll up
                if (itemIndex < items.Length - 1)
                {
                    EquipItem(itemIndex + 1);
                    animator.SetTrigger("SwitchWeapon");
                }
            }
            else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f && !isBenchPressing && !playerGun.isReloading)
            {
                // Scroll down
                if (itemIndex > 0)
                {
                    EquipItem(itemIndex - 1);
                    animator.SetTrigger("SwitchWeapon");
                }
            }

            if (Input.GetMouseButton(0) && !isBenchPressing)
            {
                items[itemIndex].Use();
            }

            if (transform.position.y < -5f) // Die if you fall out of the world
            {
                Die();
            }

            if (characterController.isGrounded && !isBenchPressing)
            {
                // Reset jumping flag
                isJumping = false;
            }
            if (Input.GetKeyDown(KeyCode.R) && !isBenchPressing)
            {
                // Call the Reload() method on the currently equipped gun
                if (items[itemIndex] is Gun equippedGun)
                {
                    equippedGun.Reload();
                }
            }
            CheckHealth();
            UpdateStatusEffects();
            UpdateHealthUI();

            if (currentHealth <= 30)
            {
                lowHealthText.gameObject.SetActive(true);
                lowHealthAudio.SetActive(true);
            }

            else if (currentHealth > 40)
            {
                lowHealthText.gameObject.SetActive(false);
                lowHealthAudio.SetActive(false);
            }

            

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();
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

        damageBarShrinkTime -= Time.deltaTime;
        if (damageBarShrinkTime < 0)
        {
            if (healthBar.fillAmount < damagedBar.fillAmount)
            {
                float shirnkSpeed = 1f;
                damagedBar.fillAmount -= shirnkSpeed * Time.deltaTime;
            }
        }
    }

    void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);
        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * yMouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
        weaponHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
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
                animator.SetTrigger("PlayerBob");
            }
            else
            {
                isSprinting = false;
                moveAmount = moveDir * walkSpeed;
                animator.ResetTrigger("PlayerBob");
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

    void CheckHealth()
    {
        if (currentHealth <= 0)
        {
            Die();
        }

        if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
    }

    void Die()
    {
        isDead = true;
        GetComponent<CapsuleCollider>().enabled = false;
        animator.SetBool("IsDead", true); // Trigger the death animation
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameOverScreen.gameObject.SetActive(true);
        playerHud.gameObject.SetActive(false);
        Debug.Log("Player is Dead!");
        if (!hasPlayedDeathSound)
        {
            audioSource.PlayOneShot(deathSound);
            hasPlayedDeathSound = true;
        }
        lowHealthAudio.SetActive(false);
        if (currentHealth < 0) currentHealth = 0;
        timer.PlayerDied();
        
        highScoreManager.SetHighScore(currentScore);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Player took" + damage + "damage");
            currentHealth -= damage;
            audioSource.PlayOneShot(hurtSound);
            CheckHealth();
        damageBarShrinkTime = 1f;
    }

    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f; // Pause the game
        if (!isPaused)
        {
            // Show pause menu

            isPaused = true;
            pauseMenuPanel.SetActive(true);
            audioSource.PlayOneShot(pauseSound);
        }
        else
        {
            // Hide pause menu
            Time.timeScale = 1f; // Unpause the game
            isPaused = false;
            pauseMenuPanel.SetActive(false);
            Cursor.visible = false;
        }
    }

    public void ResumeGame()
    {
        // Unpause the game
        Time.timeScale = 1f;
        isPaused = false;
        pauseMenuPanel.SetActive(false);

        // Enable the camera and hide the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    public void QuitGame()
    {
        SceneManager.LoadScene("TitleScene");
        AudioListener.pause = false; // Resume the music
        Time.timeScale = 1f;
    }

    public void RetryGame()
    {
        Time.timeScale = 1f; // Unpause the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
        pauseMenuPanel.SetActive(false);
        isPaused = false;
    }

    void UpdateHealthUI()
    {
        healthBar.fillAmount = currentHealth / maxHealth;
        healthbarText.text = currentHealth.ToString("F1"); // Formats to one decimal place

    }


    private void UpdateStatusEffects()
    {
        if (curse <= 0)
            {
                maxHealth = 100f;
                isCursed = false;
                curseBarUI.SetActive(false);
            }
            
        if (curse == maxCurse && !isCursed)
            {
                isCursed = true; 
            }
        if (isCursed)
        {
            curse -= 3.33f * Time.deltaTime;
            maxHealth = 50f;
            if (currentHealth > 50)
                {
                    currentHealth= 50;
                }
            UpdateCurseUI();
        }
    }
    void UpdateCurseUI() 
    {
        curseBar.fillAmount = (float)curse/maxCurse;

        if (curse > 0)
        {
            curseBarUI.gameObject.SetActive(true);
        }

        if (curse >= 30 && curse < maxCurse)
        {
            audioSource.PlayOneShot(curseSound);
        }

    }

    public void UpdateScore(int scoreValue)
    {
        currentScore += scoreValue;
        score.text = currentScore.ToString();
    }

    public void SetArmPivotRotation(Quaternion rotation)
    {
        // Set the rotation of the arm pivot (weapon holder)
        armPivot.rotation = rotation;
    }

    private IEnumerator ResetCurseAfterDelay(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        curseBarUI.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead)
            return;
        // if player gets health 
        if (other.gameObject.CompareTag("Health"))
        {
            currentHealth += 25;
            if (currentHealth > 100) currentHealth = 100;
            damagedBar.fillAmount = currentHealth/maxHealth;
            Destroy(other.gameObject);
            CheckHealth();
        }
        // if player touches projectiles
        else if (other.gameObject.CompareTag("Fireball") || other.gameObject.CompareTag("EnemyProjectile"))
        {
            TakeDamage(5);
            CheckHealth();
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Curseball"))
        {
            Debug.Log("Player cursed!");
            curse += 10;
            TakeDamage(5);
            UpdateCurseUI();
            CheckHealth();
            // Start the coroutine to reset curse and isCursed after 20 seconds
            if (!isCursed)
            {
                StartCoroutine(ResetCurseAfterDelay(15f));
            }
            StartCoroutine(ResetCurseAfterDelay(15f));
            Destroy(other.gameObject);
        }

        // if player touches enemy
        else if (other.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(5);
            CheckHealth();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("EnemyRocket"))
        {
            TakeDamage(15);
            CheckHealth();
        }
    }
}
