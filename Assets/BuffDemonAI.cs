using UnityEngine;

public class BuffDemonAI : MonoBehaviour
{
    public Transform player;
    public Animator animator;
    public GameObject fireballPrefab;
    public GameObject shockwavePrefab;
    public float meleeRange = 2f;
    public float fireballCooldown = 3f;
    public float groundPoundCooldown = 5f;
    public float chargeCooldown = 8f;
    public float chargeSpeed = 10f;
    public float chargeDuration = 2f;
    public float chargeStopDistance = 1f;

    private bool canAttack = true;
    private bool isCharging = false;
    private float currentCooldown;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (canAttack)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= meleeRange)
            {
                MeleeAttack();
            }
            else if (currentCooldown <= 0f)
            {
                int randomAttack = Random.Range(0, 3);
                switch (randomAttack)
                {
                    case 0:
                        FireballAttack();
                        currentCooldown = fireballCooldown;
                        break;
                    case 1:
                        GroundPoundAttack();
                        currentCooldown = groundPoundCooldown;
                        break;
                    case 2:
                        ChargeAttack();
                        currentCooldown = chargeCooldown;
                        break;
                }
            }
            else
            {
                currentCooldown -= Time.deltaTime;
            }
        }
    }

    private void MeleeAttack()
    {
        // Trigger melee attack animation
        animator.SetTrigger("MeleeAttack");

        // Implement your melee attack logic here
        Debug.Log("Melee attack!");
        // Apply damage to the player or trigger other relevant actions
    }

    private void FireballAttack()
    {
        // Trigger fireball attack animation
        animator.SetTrigger("FireballAttack");

        // Implement your fireball attack logic here
        Debug.Log("Fireball attack!");

        // Spit three fireballs simultaneously
        for (int i = 0; i < 3; i++)
        {
            GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
            // Set fireball's behavior and direction towards the player
        }
    }

    private void GroundPoundAttack()
    {
        // Trigger ground pound animation
        animator.SetTrigger("GroundPound");

        // Implement your ground pound attack logic here
        Debug.Log("Ground pound attack!");
        // Play the ground pound animation

        // Create shockwave effect
        GameObject shockwave = Instantiate(shockwavePrefab, transform.position, Quaternion.identity);
        // Set shockwave's behavior and damage if it collides with the player
    }

    private void ChargeAttack()
    {
        // Trigger charge animation
        animator.SetTrigger("Charge");

        // Implement your charge attack logic here
        Debug.Log("Charge attack!");
        rb.velocity = (player.position - transform.position).normalized * chargeSpeed;
        Invoke(nameof(StopCharge), chargeDuration);
    }

    private void StopCharge()
    {
        rb.velocity = Vector3.zero;
        isCharging = false;
    }
}
