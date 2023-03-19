using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float fireRate = 1f;
    public GameObject fireballPrefab;

    private Transform player;
    private Animator anim;
    private bool isKnockedDown = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        InvokeRepeating("Shoot", 0, fireRate);
    }

    void Update()
    {
        if (!isKnockedDown)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance <= 10f)
            {
                anim.SetBool("isWalking", false);
                transform.LookAt(player);
            }
            else
            {
                anim.SetBool("isWalking", true);
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }
        }
    }

    void Shoot()
    {
        if (!isKnockedDown)
        {
            Instantiate(fireballPrefab, transform.position, transform.rotation);
        }
    }

    public void KnockDown()
    {
        isKnockedDown = true;
        anim.SetTrigger("isHit");
        StartCoroutine(GetUp());
    }

    IEnumerator GetUp()
    {
        yield return new WaitForSeconds(5f);
        isKnockedDown = false;
        anim.SetTrigger("getUp");
    }

    public void Death()
    {
        StartCoroutine(Dissolve());
    }

    IEnumerator Dissolve()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
