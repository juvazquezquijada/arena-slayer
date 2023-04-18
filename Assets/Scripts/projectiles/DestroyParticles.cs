using UnityEngine;

public class DestroyParticles : MonoBehaviour
{
    private ParticleSystem particles;

    void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (!particles.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}