 using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetPlayerController : MonoBehaviourPunCallbacks, IDamageable
{
    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, smoothTime, yMouseSensitivity;
    [SerializeField] GameObject playerCamera;
    [SerializeField] GameObject cameraHolder, wepHolder, lowHealthAudio;
    [SerializeField] float jumpForce;
    [SerializeField] Item[] items;
    [SerializeField] AudioSource audioSource;
    [SerializeField] Image healthBar, damagedBar;
    [SerializeField] TMP_Text healthbarText;
    [SerializeField] GameObject ui, billBoard;
    [SerializeField] Animator animator;
    public FFAGameManager gameManager;
    public AudioClip hurtSound;
    public AudioClip jumpSound;
    public bool isJumping = false;
    public bool isPaused = false;
    public bool isGrounded = true;
    public string playerName;
    int itemIndex;
    int previousItemIndex = -1;
    float verticalLookRotation;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;
    public float damageBarShrinkTime = 1f;
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaRegenRate = 10f;
    public float sprintStaminaCost = 20f;
    public float jumpStaminaCost = 10f;
    public float refillDelayDuration = 3f;
    private bool isRefillingStamina;
    private float timeSinceLastAction;
    private bool isSprinting;
    private float lastWeaponSwitchTime = 0f;
    public float weaponSwitchCooldown = 0.5f; // Adjust the cooldown duration as needed

    public Image staminaBarImage;

    public SingleShotGun currentWeapon; // Add this field
    private SingleArmGun currentOWWeapon; // Add this field
    CharacterController characterController;
    PhotonView PV;

    const float maxHealth = 100f;
    float currentHealth = maxHealth;
    public GameObject lowHealthText;

    PlayerManager playerManager;

    float healthRegenRate = 10f; // Adjust the regeneration rate as needed
    float lastHitTime = 0f;
    bool isRegenerating = false;




    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        PV = GetComponent<PhotonView>();

        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
        playerName = PhotonNetwork.NickName; // Store the player's name
        gameManager = FFAGameManager.Instance;
    }

    void Start()
    {
        if (PV.IsMine)
        {
            EquipItem(0);
            Destroy(billBoard);
        }
        else
        {
            Destroy(playerCamera);
            Destroy(characterController);
            Destroy(ui);
            ResetWeaponHolderLayer();
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        currentStamina = maxStamina;
        timeSinceLastAction = Time.time;
        damagedBar.fillAmount = currentHealth/maxHealth;
    }

    void Update()
    {

        if (!FFAGameManager.Instance.isGameOver)
        {
            if (!PV.IsMine || isPaused)
                return;
            
                Look();
                Move();
 
            

            for (int i = 0; i < items.Length; i++)
            {
                if (Input.GetKeyDown((i + 1).ToString()) && Time.time - lastWeaponSwitchTime >= weaponSwitchCooldown && currentWeapon != null && !currentWeapon.IsReloadingOrShooting())
                {
                    EquipItem(i);
                    PV.RPC("RPC_PlayWeaponSwitch", RpcTarget.All);
                    break;
                }
            }

            if (Time.time - lastWeaponSwitchTime >= weaponSwitchCooldown && currentWeapon != null && !currentWeapon.IsReloadingOrShooting())
            {
                if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
                {
                    // Scroll up
                    if (itemIndex < items.Length - 1)
                    {
                        EquipItem(itemIndex + 1);
                        lastWeaponSwitchTime = Time.time; // Update the last weapon switch time
                        PV.RPC("RPC_PlayWeaponSwitch", RpcTarget.All);
                    }
                }
                else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
                {
                    // Scroll down
                    if (itemIndex > 0)
                    {
                        EquipItem(itemIndex - 1);
                        lastWeaponSwitchTime = Time.time; // Update the last weapon switch time
                        PV.RPC("RPC_PlayWeaponSwitch", RpcTarget.All);
                    }
                }

                // Check for item switching
                else if (Input.GetKeyDown(KeyCode.Q))
                {
                    // Switch to the previous item
                    EquipItem(itemIndex - 1);
                    lastWeaponSwitchTime = Time.time; // Update the last weapon switch time
                    PV.RPC("RPC_PlayWeaponSwitch", RpcTarget.All);
                }
                else if (Input.GetKeyDown(KeyCode.E))
                {
                    // Switch to the next item
                    EquipItem(itemIndex + 1);
                    lastWeaponSwitchTime = Time.time; // Update the last weapon switch time
                    PV.RPC("RPC_PlayWeaponSwitch", RpcTarget.All);
                }
            }


            if (Input.GetMouseButton(0) && lastWeaponSwitchTime >= weaponSwitchCooldown && !isPaused)
            {
                items[itemIndex].Use();
            }

            if (transform.position.y < -5f) // Die if you fall out of the world
            {
                playerManager.Fall();
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
                    animator.ResetTrigger("PlayerBob");
                }
            }
        }

        else
        {
            GameOver();
        }

        if (currentHealth <= 30)
        {
            lowHealthText.gameObject.SetActive(true);
            lowHealthAudio.SetActive(true);
        }
        else if (currentHealth > 30 || currentHealth <=0)
        {
            lowHealthText.gameObject.SetActive(false);
            lowHealthAudio.SetActive(false);
        }



        // Health Regeneration
        if (isRegenerating && Time.time - lastHitTime >= 5f) // Adjust the delay as needed
        {
            currentHealth += healthRegenRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
            healthBar.fillAmount = currentHealth / maxHealth;
            damagedBar.fillAmount = currentHealth / maxHealth;
            healthbarText.text = currentHealth.ToString("F1"); // Formats to one decimal place

            if (currentHealth >= maxHealth)
            {
                isRegenerating = false;
                lowHealthText.gameObject.SetActive(false);
            }
        }

        damageBarShrinkTime -= Time.deltaTime;
        if (damageBarShrinkTime < 0)
        {
            if (healthBar.fillAmount < damagedBar.fillAmount)
            {
                float shirnkSpeed = 0.75f;
                damagedBar.fillAmount -= shirnkSpeed * Time.deltaTime;
            }
        }


    }

    void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);
        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * yMouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -40f, 40f);

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
                animator.SetTrigger("PlayerBob");
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
        if (_index == previousItemIndex || Time.time - lastWeaponSwitchTime < weaponSwitchCooldown || isPaused)
            return;

        lastWeaponSwitchTime = Time.time; // Update the last weapon switch time

        itemIndex = _index;

        items[itemIndex].itemGameObject.SetActive(true);

        if (previousItemIndex != -1)
        {
            items[previousItemIndex].itemGameObject.SetActive(false);
        }

        previousItemIndex = itemIndex;

        // Call the UpdateAmmoUIOnSwitch method on the currently equipped gun (if it is a SingleShotGun)
        if (items[itemIndex] is SingleShotGun singleShotGun)
        {
            singleShotGun.UpdateAmmoUIOnSwitch();
            singleShotGun.ShowAltArms();
            currentWeapon = singleShotGun;
        }

        if (items[itemIndex] is SingleArmGun singleArmGun)
        {
            singleArmGun.UpdateAmmoUIOnSwitch();
            singleArmGun.ShowAltArms();
            currentOWWeapon = singleArmGun;
        }



        if (PV.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("itemIndex", itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }


    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("itemIndex") && !PV.IsMine && targetPlayer == PV.Owner)
        {
            EquipItem((int)changedProps["itemIndex"]);
        }
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("took damage" + damage);
        
        PV.RPC(nameof(RPC_TakeDamage), PV.Owner, damage);
    }

    void ResetLayersRecursively(Transform obj, int newLayer)
    {
        obj.gameObject.layer = newLayer; // Set the layer of the current object

        // Recursively set the layer for all child objects
        foreach (Transform child in obj)
        {
            ResetLayersRecursively(child, newLayer);
        }
    }

    void ResetWeaponHolderLayer()
    {
        Debug.Log("Resetting weapon layer");

        // Set the layer of the wepHolder GameObject
        cameraHolder.layer = 0; // Change this to match your default layer index

        // Reset the layers of all child objects (including descendants)
        foreach (Transform child in cameraHolder.transform)
        {
            ResetLayersRecursively(child, 0); // Change this to match your default layer index
        }
    }




    [PunRPC]
    void RPC_TakeDamage(float damage, PhotonMessageInfo info)
    {
        audioSource.PlayOneShot(hurtSound);
        currentHealth -= damage;
        damageBarShrinkTime = 1f;
        healthBar.fillAmount = currentHealth / maxHealth;
        healthbarText.text = currentHealth.ToString("F1"); // Formats to one decimal place

        // Set the last hit time and start regeneration
        lastHitTime = Time.time;
        isRegenerating = true;


        if (currentHealth <= 0)
        {
            string killerName = info.Sender.NickName; // Get the killer's name
            Die(killerName); // Pass the killer's name to the Die method
            PlayerManager.Find(info.Sender).GetKill(playerName);

        }
    }


    [PunRPC]
    void RPC_PlayWeaponSwitch()
    {
        animator.SetTrigger("SwitchWeapon");
    }

    void GameOver()
    {
        Destroy(playerCamera);
        Destroy(characterController);
        ui.gameObject.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Die(string killerName)
    {
        playerManager.Die(killerName);
    }

    public void Disable()
    {
        isPaused = true;
    }

    public void Enable()
    {
        isPaused = false;
    }


}