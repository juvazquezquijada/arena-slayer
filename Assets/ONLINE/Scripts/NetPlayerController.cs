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
    [SerializeField] GameObject cameraHolder;
    [SerializeField] float jumpForce;
    [SerializeField] Item[] items;
    [SerializeField] AudioSource audioSource;
    [SerializeField] Image healthBar;
    [SerializeField] TMP_Text healthbarText;
    [SerializeField] GameObject ui;
    [SerializeField] GameObject wepCamera;

    public FFAGameManager gameManager;
    public AudioClip hurtSound;
    public bool isJumping = false;

    int itemIndex;
    int previousItemIndex = -1;
    float verticalLookRotation;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;

    CharacterController characterController;
    PhotonView PV;

    const float maxHealth = 100f;
    float currentHealth = maxHealth;

    PlayerManager playerManager;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        PV = GetComponent<PhotonView>();

        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();

        gameManager = FFAGameManager.Instance;
    }

    void Start()
    {
        if (PV.IsMine)
        {
            EquipItem(0);
        }
        else
        {
            Destroy(playerCamera);
            Destroy(wepCamera);
            Destroy(characterController);
            Destroy(ui);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!FFAGameManager.Instance.isGameOver)
        {
            if (!PV.IsMine)
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
                if (itemIndex >= items.Length - 1)
                {
                    EquipItem(0);
                }
                else
                {
                    EquipItem(itemIndex + 1);
                }
            }
            else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
            {
                if (itemIndex <= 0)
                {
                    EquipItem(items.Length - 1);
                }
                else
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

        }

        else
        {
            GameOver();
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
            // Apply horizontal movement
            moveAmount = moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed);

            // Check for jump input
            if (Input.GetButton("Jump"))
            {
                isJumping = true;
                moveAmount.y = jumpForce;
            }
        }
        else
        {
            // Only apply vertical movement due to gravity
            moveAmount.y += Physics.gravity.y * Time.deltaTime;

            // Reset jumping flag if the player starts descending
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

    [PunRPC]
    void RPC_TakeDamage(float damage, PhotonMessageInfo info)
    {
        audioSource.PlayOneShot(hurtSound);
        currentHealth -= damage;
        healthBar.fillAmount = currentHealth / maxHealth;
        healthbarText.text = currentHealth.ToString();
        if (currentHealth <= 0)
        {
            Die();
            PlayerManager.Find(info.Sender).GetKill();
        }
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
        playerManager.Die();
    }
}
